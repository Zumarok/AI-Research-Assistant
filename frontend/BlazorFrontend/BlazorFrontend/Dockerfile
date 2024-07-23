# Use the official .NET SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the .csproj file and restore the dependencies
COPY BlazorFrontend/BlazorFrontend/BlazorFrontend.csproj BlazorFrontend/
RUN dotnet restore "BlazorFrontend/BlazorFrontend.csproj"

# Copy the rest of the application files and build the application
COPY . .
WORKDIR /src/BlazorFrontend/BlazorFrontend
RUN dotnet publish -c Release -o /app/publish

# Use the official .NET runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Configure the entry point for the application
ENTRYPOINT ["dotnet", "BlazorFrontend.dll"]

# Expose the port the application runs on
EXPOSE 80
