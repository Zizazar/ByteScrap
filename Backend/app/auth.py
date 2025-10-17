import time
from typing import Annotated
from fastapi import Depends, HTTPException, Request
from fastapi.security import HTTPAuthorizationCredentials, HTTPBearer
import jwt
from passlib.context import CryptContext
from sqlmodel import Session

from app import settings
from app.database import SessionDep
from app.utils import get_user_by_id, get_user_by_name

pwd_context = CryptContext(schemes=["bcrypt"])

def get_token(request: Request):
    token = request.headers.get('Authorization')
    if not token:
        raise HTTPException(status_code=401, detail='Token not found')
    return token.split(" ")[1]

TokenDep = Annotated[str, Depends(get_token)]



def get_current_user(db: SessionDep, token: TokenDep):
    payload = decode_jwt(token)

    user_id = payload.get('user_id')
    if not user_id:
        raise HTTPException(status_code=401, detail='Не найден ID пользователя')

    user = get_user_by_id(db, int(user_id))
    if not user:
        raise HTTPException(
            status_code=404, 
            detail='User not found'
            )

    return user

def verify_password(plain_password, hashed_password):
    return pwd_context.verify(plain_password, hashed_password)

def get_password_hash(password):
    return pwd_context.hash(password)

def authenticate_user(db: Session, username: str, password: str):
    user = get_user_by_name(db, username)
    if not user:
        return False
    if not verify_password(password, user.password_hash):
        return False
    return user

def sign_jwt(user_id: int):
    payload = {
        "user_id": str(user_id),
        "expires": time.time() + 30*60*60*24
    }
    token = jwt.encode(payload, settings.RANDOM_SECRET, algorithm=settings.PASSWORD_HASH_ALGORITHM)
    return token

def decode_jwt(token: str):
    try:
        decoded_token = jwt.decode(token, settings.RANDOM_SECRET, algorithms=[settings.PASSWORD_HASH_ALGORITHM])
        return decoded_token if decoded_token["expires"] >= time.time() else None
    except jwt.DecodeError as e:
        raise HTTPException(
            status_code=401, 
            detail=f'Invalid Token: {e}'
            )
    



class JWTBearer(HTTPBearer):
    def __init__(self, auto_error: bool = True):
        super(JWTBearer, self).__init__(auto_error=auto_error)

    async def __call__(self, request: Request):
        credentials: HTTPAuthorizationCredentials = await super(JWTBearer, self).__call__(request)
        if credentials:
            if not credentials.scheme == "Bearer":
                raise HTTPException(status_code=403, detail="Invalid authentication scheme.")
            if not self.verify_jwt(credentials.credentials):
                raise HTTPException(status_code=403, detail="Invalid token or expired token.")
            return credentials.credentials
        else:
            raise HTTPException(status_code=403, detail="Invalid authorization code.")

    def verify_jwt(self, jwtoken: str) -> bool:
        isTokenValid: bool = False

        try:
            payload = decode_jwt(jwtoken)
        except:
            payload = None
        if payload:
            isTokenValid = True

        return isTokenValid
    