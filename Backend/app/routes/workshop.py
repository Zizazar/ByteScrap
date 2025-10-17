from fastapi import APIRouter, Depends, HTTPException, Response
from fastapi.logger import logger
from sqlmodel import select, func

from app.auth import JWTBearer, TokenDep, get_current_user
from app.database import SessionDep
from app.models.db_models import WorkInDB
from app.models.response_models import SearchResponse, WorkInfo, WorkUpload

router = APIRouter(
    prefix="/workshop",
    tags=["workshop"]
)

@router.post("/upload", dependencies=[Depends(JWTBearer())])
def work_upload(payload: WorkUpload, db: SessionDep, token: TokenDep):
    current_user = get_current_user(db, token)
    works = WorkInDB(**payload.model_dump(), author_id=current_user.id)
    db.add(works)
    db.commit()
    db.refresh(works)
    logger.info(f"{current_user.name} загрузил работу {works.name}")
    return {
            'message': f'Работа {works.name} загружена', 
            "work_id": works.id
            }

@router.get("/all")
def get_works(db: SessionDep, offset: int = 0, limit: int = 20):
    works = db.exec(select(WorkInDB).offset(offset).limit(limit)).all()
    return [WorkInfo(**work.model_dump(), author_name=work.author.name) for work in works]

@router.get("/{work_id}/download", summary="Returns work by ID.")
def download_work_by_id(work_id: int, db: SessionDep):
    work = db.get(WorkInDB, work_id)
    if not work:
        raise HTTPException(
            status_code=404,
            detail=f"Work with ID {work_id} not found",
        )
    headers = {'Content-Disposition': f'attachment; filename="work_{work_id}.json"'}
    return Response(content=work.data, media_type="application/json", headers=headers)

@router.get("/{work_id}", summary="Returns work info by ID.")
def get_work_by_id(work_id: int, db: SessionDep):
    work = db.get(WorkInDB, work_id)
    if not work:
        raise HTTPException(
            status_code=404,
            detail=f"Work with ID {work_id} not found",
        )
    return WorkInfo(**work.model_dump(), author_name=work.author.name)


@router.get("/fts", summary="Returns FTS Matches.") #https://habr.com/ru/companies/beeline_cloud/articles/742214/
async def match_ts(db: SessionDep, term: str = None, lim: int = 20, of: int = None):

    if term and len(term) > 1:

        columns = func.coalesce(WorkInDB.name, '').concat(func.coalesce(WorkInDB.description, ''))
        columns = columns.self_group() 
        
        q = select(
            WorkInDB
            ).where(
                # все строки, для которых коэффициент совпадения больше текущего лимита
                columns.bool_op('%')(term),
            ).order_by(
                # добавим ранжирование по коэффициенту совпадения
                func.similarity(columns, term).desc(), 
            ).offset(of).limit(lim)
        
        db.exec(select(func.set_limit('0.1')))
        res = db.exec(q).fetchall()
        
        if len(res) > 0:
            return SearchResponse(
                results=[WorkInfo(**work.model_dump(), author_name=work.author.name) for work in res],
                count=len(res)
                )
        else:
            raise HTTPException(
            status_code=404,
            detail=f"Not Found",
        )
    else:
        works = db.exec(select(WorkInDB).offset(of).limit(lim)).fetchall()
        return SearchResponse(
            results=[WorkInfo(**work.model_dump(), author_name=work.author.name) for work in works],
            count=len(works)
        )