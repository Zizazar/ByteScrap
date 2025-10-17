import time
from fastapi import FastAPI, Request, Response
from sqlmodel import select, func
import uvicorn
from app import settings
from app.database import SessionDep, init_db
from app.routes.users import router as user_router
from app.routes.workshop import router as workshop_router
from app.routes.saves import router as saves_router
from fastapi.middleware.cors import CORSMiddleware

app = FastAPI()

app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

@app.get("/")
def read_root(db: SessionDep):
    db_size = db.exec(select(func.pg_size_pretty(func.pg_database_size('byte_scrap')))).one_or_none()
    return {"status": "working", "db_size": db_size}

app.include_router(user_router)
app.include_router(workshop_router)
app.include_router(saves_router)


@app.middleware("http")
async def add_process_time_header(request: Request, call_next):
    start_time = time.time()
    response: Response = await call_next(request)
    process_time = time.time() - start_time
    response.headers["X-Process-Time"] = str(process_time)
    return response


app.on_event("startup")(init_db)

if __name__ == "__main__":
    host, port = settings.SERVER_ADDRESS.split(":")
    uvicorn.run(app, host=host, port=int(port))