from typing import Annotated
from fastapi import APIRouter, Depends, HTTPException, Response
from fastapi.params import Query
from sqlmodel import select

from app.auth import JWTBearer, TokenDep, authenticate_user, get_current_user, get_password_hash, sign_jwt
from app.database import SessionDep
from app.models.db_models import Users
from app.models.response_models import TokenResponse, UserAuth, UserInfo
from app.utils import get_user_by_name


router = APIRouter(
    prefix="/users",
    tags=["users"]
)

@router.get("/", dependencies=[Depends(JWTBearer())])
def read_users(
    db: SessionDep,
    offset: int = 0,
    limit: Annotated[int, Query(le=100)] = 100,
) -> list[Users]:
    users = db.exec(select(Users).offset(offset).limit(limit)).all()
    return users

@router.post("/register")
def register_user(payload: UserAuth, db: SessionDep):
    if get_user_by_name(db, payload.name):
        raise HTTPException(
            status_code=403,
            detail=f"User {payload.name} already exists",
        )
    
    hashed_password = get_password_hash(payload.password)
    user = Users(**payload.model_dump(), password_hash=hashed_password)  # https://fastapi.tiangolo.com/tutorial/extra-models/#multiple-models

    db.add(user)
    db.commit()
    db.refresh(user)
    return Response(sign_jwt(user.id))

@router.post("/login")
def login_user(payload: UserAuth, db: SessionDep):
    user = authenticate_user(db, payload.name, payload.password)
    if not user:
        raise HTTPException(
            status_code=403,
            detail=f"Invalid password or login",
        )
    return Response(sign_jwt(user.id))

@router.get("/current", dependencies=[Depends(JWTBearer())])
def test_auth(db: SessionDep, token: TokenDep):
    current_user = get_current_user(db, token)
    if not current_user:
        raise HTTPException(
            status_code=401,
            detail=f"Invalid token",
        )
    return UserInfo(id=current_user.id, name=current_user.name)

