FROM microsoft/aspnetcore-build:2.0 AS build-env
WORKDIR /app

# Copy everything else and build
COPY src/ ./

# Copy csproj and restore as distinct layers
RUN dotnet restore BuildTrigger.Web/BuildTrigger.Web.csproj
RUN dotnet publish -c Release -o out ./BuildTrigger.Web/BuildTrigger.Web.csproj

# Build runtime image
FROM microsoft/aspnetcore:2.0
WORKDIR /app
COPY --from=build-env /app/BuildTrigger.Web/out .
ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "BuildTrigger.Web.dll"]