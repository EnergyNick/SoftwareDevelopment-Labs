FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["GrpcChat/GrpcChat.csproj", "GrpcChat/"]
RUN dotnet restore "GrpcChat/GrpcChat.csproj"
COPY . .
WORKDIR "/src/GrpcChat"
RUN dotnet build "GrpcChat.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GrpcChat.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GrpcChat.dll"]
