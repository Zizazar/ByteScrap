from typing import Annotated
from fastapi import Depends
from sqlmodel import SQLModel, create_engine, Session, text
import os

from app.settings import DATABASE_URL

engine = create_engine(DATABASE_URL)

def init_db():
    SQLModel.metadata.create_all(engine)
    with Session(engine) as db:
        db.exec(text("CREATE EXTENSION IF NOT EXISTS pg_trgm;"))
        db.commit()

def get_session() -> Session:
    return Session(engine)

SessionDep = Annotated[Session, Depends(get_session)]