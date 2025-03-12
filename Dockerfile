# Stage 1: Base image (Runtime environment)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 3000
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:3000  

# Stage 2: Build environment
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy only the necessary project files to restore dependencies (better caching)
COPY BlindBoxSS.API/*.csproj ./BlindBoxSS.API/
COPY Repositories/*.csproj ./Repositories/
COPY DAO/*.csproj ./DAO/
COPY Models/*.csproj ./Models/
COPY Services/*.csproj ./Services/

# Restore dependencies
RUN dotnet restore ./BlindBoxSS.API/BlindBoxSS.API.csproj

# Copy the entire source code after restoring dependencies
COPY . .

# Build the application
RUN dotnet publish "BlindBoxSS.API/BlindBoxSS.API.csproj" -c Release -o /app/publish

# Stage 3: Final runtime
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "BlindBoxSS.API.dll"]
