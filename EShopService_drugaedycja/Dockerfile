# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

EXPOSE 8080
EXPOSE 8081

# Копіюємо рішення
COPY *.sln ./

# Копіюємо всі csproj у відповідні папки
COPY EShopService_drugaedycja/*.csproj ./EShopService_drugaedycja/
COPY EShop.Application/*.csproj ./EShop.Application/
COPY EShop.Domain/*.csproj ./EShop.Domain/
COPY EShop.Application.Tests/*.csproj ./EShop.Application.Tests/
COPY EShop.Domain.Tests/*.csproj ./EShop.Domain.Tests/
COPY EShopService.Tests/*.csproj ./EShopService.Tests/
COPY EShopService.IntegrationTests/*.csproj ./EShopService.IntegrationTests/
COPY User.Application/*.csproj ./User.Application/
COPY User.Domain/*.csproj ./User.Domain/
COPY UserService/*.csproj ./UserService/

# Тепер restore буде працювати
RUN dotnet restore

COPY . .

WORKDIR /app/EShopService_drugaedycja
RUN dotnet publish -c Release -o out

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app


COPY --from=build /app/EShopService_drugaedycja/out ./
ENTRYPOINT ["dotnet", "EShopService_drugaedycja.dll"]


