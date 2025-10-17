from os import getenv
from dotenv import load_dotenv

load_dotenv()

SERVER_ADDRESS = getenv("SERVER_ADDRESS", "0.0.0.0:8080")
DATABASE_URL = getenv("DATABASE_URL", "sqlite:///./test.db")
PASSWORD_HASH_ALGORITHM = getenv("PASSWORD_HASH_ALGORITHM", "HS256")
RANDOM_SECRET = getenv("RANDOM_SECRET", "VeryStrongSecret")