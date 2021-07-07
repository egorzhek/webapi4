#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# install libgdiplus for System.Drawing
RUN apt-get update && \
    apt-get install -y --allow-unauthenticated libgdiplus libc6-dev

# install x11 for System.Drawing
RUN apt-get update && \
    apt-get install -y --allow-unauthenticated libx11-dev

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["webapi4/webapi4.csproj", "webapi4/"]
RUN dotnet restore "webapi4/webapi4.csproj"
COPY . .
WORKDIR "/src/webapi4"
RUN dotnet build "webapi4.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "webapi4.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "webapi4.dll"]