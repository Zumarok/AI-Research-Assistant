# AI Research Assistant Project

## Overview

This project showcases an AI Research Assistant that interacts with users through a chat interface. The AI, known as the Research Agent, takes research topics from users and assigns tasks to specialized AI agents. These agents gather information from various sources such as Wikipedia, YouTube, Reddit, etc. The collected data is stored by a Repository Agent, which allows the Research Agent to provide well-informed responses using Retrieval-Augmented Generation (RAG) techniques.

## Technologies Used

- **Frontend**: Blazor
- **Backend**: ASP.NET 8.0 with AutoGen for .NET, Microsoft Semantic Kernel
- **AI Integration**: Python with FastAPI and Transformers, LM Studio with Llama-3-Groq-8B-Tool-Use-GGUF model
- **Containerization**: Docker
- **CI/CD**: GitHub Actions

## Project Structure

    .
    ├── backend
    │   └── AIResearchService
    │       ├── Dockerfile
    │       ├── Program.cs
    │       ├── Startup.cs
    │       └── ...
    ├── frontend
    │   ├── Dockerfile
    │   ├── Program.cs
    │   ├── Startup.cs
    │   └── ...
    ├── python-service
    │   ├── Dockerfile
    │   ├── main.py
    │   ├── requirements.txt
    │   └── ...
    ├── docker-compose.yml
    └── README.md

## Getting Started

### Prerequisites

- Docker
- Docker Compose
- LM Studio (running locally)
- .NET 8.0 SDK

### Cloning the Repository

    git clone https://github.com/Zumarok/AI-Research-Assistant.git
    cd ai-research-assistant

## Running the Application

### Use Docker Compose to build and run the application:

    docker-compose up --build

Ensure that LM Studio is running locally with the following configuration:

    Base URL: http://localhost:1234/v1
    API Key: lm-studio

This command will build the Docker images and start the containers. The services will be accessible at:

    Frontend: http://localhost:8080
    Backend: http://localhost:5000
    Python Service: http://localhost:8000

### Project Details
## Frontend

The frontend is built with Blazor, providing a responsive and interactive chat interface for users to interact with the AI Research Assistant.

## Backend (AIResearchService)

The backend is developed with .NET 8.0, using AutoGen for .NET to orchestrate multiple AI agents and Microsoft Semantic Kernel to manage and query the research data.
- Framework: ASP.NET 8.0
- Dockerfile:
    - Exposes ports 8080 and 8081.
    - Runs the service using dotnet AIResearchService.dll.
    - Uses the mcr.microsoft.com/dotnet/aspnet:8.0 and mcr.microsoft.com/dotnet/sdk:8.0 base images for runtime and build stages, respectively.

## Python Service

The Python service is implemented using FastAPI. It includes various AI agents that gather information from different sources and send it to the Repository Agent.

## Local LLM

The LLM (Large Language Model) is run locally using LM Studio with the Llama-3-Groq-8B-Tool-Use-GGUF model. Ensure LM Studio is running and accessible at http://localhost:1234/v1 with the API key lm-studio.

## CI/CD Pipeline

The project uses GitHub Actions for Continuous Integration and Continuous Deployment (CI/CD). On every push to the main branch, the pipeline:

- Builds the Docker images for each component.
- Pushes the images to Docker Hub.
- Deploys the services using Docker Compose.

### License

This project is licensed under the MIT License. See the LICENSE file for details.