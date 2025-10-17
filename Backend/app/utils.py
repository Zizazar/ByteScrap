from sqlmodel import Session, select

from app.models.db_models import Saves, Users


def get_user_by_name(db: Session, name: str) -> Users:
    return db.exec(select(Users).filter(Users.name == name)).one_or_none()

def get_user_by_id(db: Session, id: int) -> Users:
    return db.exec(select(Users).filter(Users.id == id)).one_or_none()

def get_user_save_by_name(db: Session, user: Users, name: str):
    return db.exec(select(Saves).filter(Saves.name == name, Saves.owner == user)).one_or_none()