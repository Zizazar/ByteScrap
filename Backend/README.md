# Бэкенд игры


### Requirements

- Docker
- Docker Compose

### Installation

1. Clone the repository:
   ```
   git clone https://github.com/Zizazar/ByteScrap.git
   cd Backend
   ```

2. Создайте файл `.env` в корне и настройте  DATABASE_URL.\
Пример: `DATABASE_URL = postgresql+psycopg2://user:pass@localhost:5432/byte_scrap`

### Running the Application

```
docker-compose up --build
```

### Services

- FastAPI: [localhost:8000](http://localhost:8000)
- Docs (Swagger): [localhost:8000/docs](http://localhost:8000/docs)
- Adminer: [localhost:8080](http://localhost:8080)