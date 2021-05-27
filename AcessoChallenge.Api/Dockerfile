FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 5486

ENV ASPNETCORE_URLS=http://+:5486

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["AcessoChallenge.Api/AcessoChallenge.Api.csproj", "AcessoChallenge.Api/"]
RUN dotnet restore "AcessoChallenge.Api/AcessoChallenge.Api.csproj"
COPY . .
RUN dotnet build "AcessoChallenge.Api/AcessoChallenge.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet test "AcessoChallenge.UnitTests/AcessoChallenge.UnitTests.csproj" -c Release
RUN dotnet publish "AcessoChallenge.Api/AcessoChallenge.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AcessoChallenge.Api.dll"]
