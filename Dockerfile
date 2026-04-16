FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["TappiruServer.csproj", "."]
RUN dotnet restore "./TappiruServer.csproj"
COPY . .
RUN dotnet publish "TappiruServer.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
# Устанавливаем библиотеку для Kerberos (нужна для SSL-подключения к PostgreSQL)
RUN apt-get update && apt-get install -y libgssapi-krb5-2 && rm -rf /var/lib/apt/lists/*
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TappiruServer.dll"]