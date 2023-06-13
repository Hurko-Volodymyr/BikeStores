FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
RUN apt-get update && apt-get install -y wget
COPY . /src
WORKDIR /src/Order/Order.Host
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/mssql/server AS dbcreator
WORKDIR /db
COPY ./DatabaseScripts ./
ENV ACCEPT_EULA=Y
ENV SA_PASSWORD=MyStrongPassword123
CMD /opt/mssql/bin/sqlservr & sleep 30 \
    && /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P MyStrongPassword123 -Q "CREATE DATABASE BikeStores" \
    && /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P MyStrongPassword123 -d BikeStores -i "BikeStores Sample Database - create objects.sql" \
    && /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P MyStrongPassword123 -d BikeStores -i "BikeStores Sample Database - load data.sql" \
    && pkill sqlservr

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "Order.Host.dll"]
