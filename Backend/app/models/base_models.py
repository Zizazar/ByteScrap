from datetime import datetime, timezone
from typing import Optional
from pydantic import BaseModel
from sqlmodel import Field


class WorkBase(BaseModel):
    id: int = Field(primary_key=True)
    author_id: int = Field(foreign_key="users.id")
    name: Optional[str]
    description: Optional[str] = Field(default=None)
    image: Optional[str] = Field(default=None)
    uploaded_at: datetime = Field(default_factory=lambda: datetime.now(timezone.utc))