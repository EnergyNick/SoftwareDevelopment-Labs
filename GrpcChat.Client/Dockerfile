FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["GrpcChat.Client/GrpcChat.Client.csproj", "GrpcChat.Client/"]
RUN dotnet restore "GrpcChat.Client/GrpcChat.Client.csproj"
COPY . .
WORKDIR "/src/GrpcChat.Client"
RUN dotnet build "GrpcChat.Client.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GrpcChat.Client.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GrpcChat.Client.dll"]
