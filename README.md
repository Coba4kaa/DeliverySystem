Для запуска данного Вэб приложения необходимо:
1. Склонировать репозиторий
2. Создать бд Postgre, соответствующую строке подключения в Appsettings
3. Накатить миграции: dotnet ef database update
4. Запустить бэк
5. Перейти в директорию фронта и запустить его: npm run start