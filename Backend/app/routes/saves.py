
from ctypes.wintypes import HACCEL
from typing import Annotated
from fastapi import APIRouter, Depends, HTTPException, Query
from sqlmodel import select

from app.auth import JWTBearer, TokenDep, get_current_user
from app.database import SessionDep
from app.models.db_models import Saves
from app.models.response_models import CreateSave
from app.utils import get_user_save_by_name


router = APIRouter(
    prefix="/saves", 
    tags=["saves"]
    )

@router.get("/", dependencies=[Depends(JWTBearer())])
def get_all_saves(db: SessionDep,
                  token: TokenDep,
                  offset: int = 0,
    limit: Annotated[int, Query(le=100)] = 100
    ):
    current_user = get_current_user(db, token)
    return db.exec(select(Saves).where(Saves.owner_id == current_user.id).offset(offset).limit(limit)).all()

@router.post("/create", dependencies=[Depends(JWTBearer())])
def create_save(payload: CreateSave, db: SessionDep, token: TokenDep):
    current_user = get_current_user(db, token)
    save = Saves(**payload.model_dump(), owner_id=current_user.id)
    db.add(save)
    db.commit()
    db.refresh(save)
    return HTTPException(status_code=201, detail={"message": f"Save {save.name} created", "save_id": save.id})


@router.get("/{save_id}", dependencies=[Depends(JWTBearer())])
def get_save_by_id(save_id: str, db: SessionDep, token: TokenDep
    ):
    current_user = get_current_user(db, token)
    save = get_user_save_by_name(db, current_user, save_id)
    if not save or save.owner_id != current_user.id:
        raise HTTPException(status_code=404, detail="Save not found for this user")
    return save

@router.delete("/{save_id}", dependencies=[Depends(JWTBearer())])
def delete_save_by_id(save_id: int, db: SessionDep, token: TokenDep
    ):
    current_user = get_current_user(db, token)
    save = get_user_save_by_name(db, current_user, save_id)
    if not save or save.owner_id != current_user.id:
        raise HTTPException(status_code=404, detail="Save not found for this user")
    db.delete(save)
    db.commit()
    return HTTPException(status_code=200, detail={"message": f"Save with ID {save_id} deleted"})

@router.post("/{save_id}", dependencies=[Depends(JWTBearer())])
def update_save_by_id(save_id: str, payload: CreateSave, 
                      db: SessionDep, token: TokenDep
    ):
    current_user = get_current_user(db, token)
    save = get_user_save_by_name(db, current_user, save_id)
    if save:
        if save.owner_id != current_user.id:
            raise HTTPException(status_code=403, detail="You are not the owner of this save")
        save.name = payload.name
        save.data = payload.data
    else:
        save = Saves(**payload.model_dump(), owner_id=current_user.id)
    db.add(save)
    db.commit()
    db.refresh(save)
    return HTTPException(status_code=200, detail={"message": f"Save with ID {save_id} updated"})
