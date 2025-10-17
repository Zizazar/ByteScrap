from typing import List
from unittest.mock import Base
from pydantic import BaseModel
from sqlmodel import Field

from app.models.base_models import WorkBase


class WorkUpload(BaseModel):
    name: str
    description: str | None
    image: str | None
    data: str

class WorkInfo(WorkBase):
    author_name: str

class SearchResponse(BaseModel):
    results: List[WorkInfo]
    count: int

class UserInfo(BaseModel):
    id: int
    name: str

class TokenResponse(BaseModel):
    token: str

class SavesUpload(BaseModel):
    name: str = Field(index=True)
    data: str

class UserAuth(BaseModel):
    name: str 
    password: str # raw password

class CreateSave(BaseModel):
    name: str
    data: str