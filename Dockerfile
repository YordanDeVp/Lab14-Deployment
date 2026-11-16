FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish "Lab11-Sapacayo-Yordan/Lab11-Sapacayo-Yordan.csproj" --configuration Release --output /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 10000
ENTRYPOINT ["sh", "-c", "exec dotnet Lab11-Sapacayo-Yordan.dll --urls http://0.0.0.0:${PORT:-10000}"]
