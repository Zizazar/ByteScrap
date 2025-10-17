* Сделана система сборки [код](https://github.com/Zizazar/ByteScrap/blob/master/ByteScrapGame/Assets/_Project/Scripts/ElectricitySystem/Systems/BuildingSystem.cs)
* Сделана система симуляции схем [код](https://github.com/Zizazar/ByteScrap/blob/master/ByteScrapGame/Assets/_Project/Scripts/ElectricitySystem/Systems/CircuitManager.cs)
* Написан бэкенд на fastapi с авторизацией по jwt токенам и загрузкой/сохранением данных в postgres [подробнее](https://github.com/Zizazar/ByteScrap/tree/master/Backend)
* docker compose для запуска сервиса с зависимостями [конфиг](https://github.com/Zizazar/ByteScrap/blob/master/Backend/docker-compose.yml)
* Сделана интеграция бэкенда с игрой [код](https://github.com/Zizazar/ByteScrap/blob/master/ByteScrapGame/Assets/_Project/Scripts/Api/GameApi.cs)
* Сделана система сохранения схем/уровней в облако / локально через json [код](https://github.com/Zizazar/ByteScrap/blob/master/ByteScrapGame/Assets/_Project/Scripts/ElectricitySystem/Systems/SaveSystem.cs)
* Сделаны пользовательские интерфейсы: главного меню, настроек, выбора уровня, авторизации
* Реализована система целей [код](https://github.com/Zizazar/ByteScrap/blob/master/ByteScrapGame%2FAssets%2F_Project%2FScripts%2FLevelAndGoals%2FGoalSystem.cs)
* Реализованы команды для отладки [код](https://github.com/Zizazar/ByteScrap/blob/master/ByteScrapGame%2FAssets%2F_Project%2FScripts%2FCommands%2FDebugCommands.cs)