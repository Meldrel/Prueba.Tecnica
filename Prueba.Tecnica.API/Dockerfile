#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Prueba.Tecnica.API/Prueba.Tecnica.API.csproj", "Prueba.Tecnica.API/"]
COPY ["Prueba.Tecnica.Aplication/Prueba.Tecnica.Aplication.csproj", "Prueba.Tecnica.Aplication/"]
COPY ["Prueba.Tecnica.Infrastructure/Prueba.Tecnica.Infrastructure.csproj", "Prueba.Tecnica.Infrastructure/"]
COPY ["Prueba.Tecnica.Domain/Prueba.Tecnica.Domain.csproj", "Prueba.Tecnica.Domain/"]
RUN dotnet restore "Prueba.Tecnica.API/Prueba.Tecnica.API.csproj"
COPY . .
WORKDIR "/src/Prueba.Tecnica.API"
RUN dotnet build "Prueba.Tecnica.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Prueba.Tecnica.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Prueba.Tecnica.API.dll"]