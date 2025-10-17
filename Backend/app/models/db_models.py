from typing import Optional

from sqlalchemy import JSON
from sqlmodel import Field, Relationship, SQLModel

from app.models.base_models import WorkBase

class Users(SQLModel, table=True):
    id: int = Field(default=None, primary_key=True)
    name: str = Field(index=True)
    password_hash: str

    works: list["WorkInDB"] = Relationship(back_populates="author")
    saves: list["Saves"] = Relationship(back_populates="owner")

class Saves(SQLModel, table=True):
    id: Optional[int] = Field(default=None, primary_key=True)
    owner_id: int = Field(default=None, foreign_key="users.id")
    name: str = Field(index=True)
    data: str

    owner: Users = Relationship(back_populates="saves")

    
class WorkInDB(SQLModel, WorkBase, table=True):
    __tablename__="works"
    author: Users = Relationship(back_populates="works")
    data: str = Field(sa_column=JSON)  # Store JSON data in a JSON column