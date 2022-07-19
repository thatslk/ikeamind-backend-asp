#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 5001
ENV ASPNETCORE_URLS=http://+:5000

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["ikeamind-backend/ikeamind-backend.csproj", "ikeamind-backend/"]
COPY ["ikeamind-backend.Infrastructure/ikeamind-backend.Infrastructure.csproj", "ikeamind-backend.Infrastructure/"]
COPY ["ikeamind-backend.Core/ikeamind-backend.Core.csproj", "ikeamind-backend.Core/"]
RUN dotnet restore "ikeamind-backend/ikeamind-backend.csproj"
COPY . .
WORKDIR "/src/ikeamind-backend"
RUN dotnet build "ikeamind-backend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ikeamind-backend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ikeamind-backend.dll"]