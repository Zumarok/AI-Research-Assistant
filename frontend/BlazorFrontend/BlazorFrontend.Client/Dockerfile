# Use the official .NET SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the .csproj file and restore the dependencies
COPY BlazorFrontend/BlazorFrontend.Client/BlazorFrontend.Client.csproj BlazorFrontend.Client/
RUN dotnet restore "BlazorFrontend.Client/BlazorFrontend.Client.csproj"

# Copy the rest of the application files and build the application
COPY . .
WORKDIR /src/BlazorFrontend/BlazorFrontend.Client
RUN dotnet publish -c Release -o /app/publish

# Use a lightweight server image to serve the Blazor WebAssembly application
FROM nginx:alpine AS runtime
COPY --from=build /app/publish /usr/share/nginx/html

# Expose port 80
EXPOSE 80

# Start Nginx server
CMD ["nginx", "-g", "daemon off;"]
