FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY HotelBooking.Core/HotelBooking.Core.csproj HotelBooking.Core/
COPY HotelBooking.Web/HotelBooking.Web.csproj HotelBooking.Web/

RUN dotnet restore HotelBooking.Web/HotelBooking.Web.csproj

COPY HotelBooking.Core/ HotelBooking.Core/
COPY HotelBooking.Web/ HotelBooking.Web/

RUN dotnet publish HotelBooking.Web/HotelBooking.Web.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "HotelBooking.Web.dll"]