\# Система бронирования номеров в отеле



Курсовой проект по дисциплине «Кроссплатформенная среда исполнения программного обеспечения»



\## Технологии



\- .NET 8

\- ASP.NET Core Blazor Server

\- Entity Framework Core (CodeFirst)

\- SQLite

\- ASP.NET Core Identity

\- Docker



\## Архитектура



\- \*\*HotelBooking.Core\*\* — модели, DbContext, сервисы бизнес-логики

\- \*\*HotelBooking.Web\*\* — Blazor Server приложение



\## Запуск



\### Локально



```bash

dotnet restore

dotnet ef database update --project HotelBooking.Core --startup-project HotelBooking.Web

dotnet run --project HotelBooking.Web 



\## Демо-данные

Админ: admin@admin.hotel / Admhotel1!

