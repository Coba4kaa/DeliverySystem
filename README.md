Для успешного запуска данного Вэб-приложения необходимо:
1. Выполнить команду git clone https://github.com/Coba4kaa/DeliverySystem
2. Создать базу данных Postgres с такой строкой подключения: "Host=localhost;Database=delivery_system;Username=postgres;Password=postgres"
3. Сделать миграции:
   dotnet ef migrations add InitialCreate
   dotnet ef database update
5. Бэкенд можно сразу запускать как проект
6. Для запуска фронта надо перейти в директорию реакт проекта cd .\delivery_system_frontend\
7. Запустить команду npm install (если установлен Node js)
8. Запустить сервер npm run start 
