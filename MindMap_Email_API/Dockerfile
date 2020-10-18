#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["MindMap_Email_API/MindMap_Email_API.csproj", "MindMap_Email_API/"]
RUN dotnet restore "MindMap_Email_API/MindMap_Email_API.csproj"
COPY . .
WORKDIR "/src/MindMap_Email_API"
RUN dotnet build "MindMap_Email_API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MindMap_Email_API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MindMap_Email_API.dll"]