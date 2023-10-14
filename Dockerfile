# Use a imagem base do .NET Core SDK para construir a aplicação
FROM mcr.microsoft.com/dotnet/core/sdk:6.0 AS build
WORKDIR /app

# Copie o arquivo do projeto e restaure as dependências
COPY WeatherApi/WeatherApi.csproj .
RUN dotnet restore

# Copie todo o conteúdo e construa a aplicação
COPY WeatherApi/ .
RUN dotnet build -c Release -o out

# Use a imagem base do .NET Core para executar a aplicação
FROM mcr.microsoft.com/dotnet/core/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

# Defina a porta dinâmica do Heroku
CMD ASPNETCORE_URLS="http://*:$PORT" dotnet WeatherApi.dll
