#pragma warning disable SKEXP0001, SKEXP0010, SKEXP0020, SKEXP0050

using Microsoft.SemanticKernel;
using AutoGen.Core;
using AutoGen.LMStudio;
using AIResearchService.Configuration;
using AIResearchService.Agents;
using AIResearchService.Common;
using AIResearchService.Filters;
using AIResearchService.Interfaces;
using AIResearchService.Plugins;
using AIResearchService.Repositories;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Memory;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.Connectors.Chroma;
using AIResearchService.Services;
using Serilog;

namespace AIResearchService;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            var builder = WebApplication.CreateBuilder(args);
            var logConfiguration = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration);

            Log.Logger = logConfiguration.CreateLogger();

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add Repository
            builder.Services.AddSingleton<ISearchRepository, SearchRepository>();

            // Add Google Search
            builder.Services.AddSingleton<IGoogleSearch, GoogleSearch>(sp =>
                new GoogleSearch(sp.GetRequiredService<ISearchRepository>()));
            builder.Services.AddLogging(cfg => cfg.AddSerilog());

            // Add Memory
            builder.Services.AddSingleton(_ =>
            {
                var memoryBuilder = new MemoryBuilder();
                var client = new HttpClient(new LocalHostServer("http://localhost:1234/v1/embeddings"));
                memoryBuilder.WithAzureOpenAITextEmbeddingGeneration("localhost",
                    "https://localhost:1234/v1/embeddings",
                    "lm-studio", "nomic-ai", httpClient: client);
                var chromaMemoryStore = new ChromaMemoryStore("https://127.0.0.1:8000");
                memoryBuilder.WithMemoryStore(chromaMemoryStore);

                return memoryBuilder.Build();
            });

            // Add ResearchService
            builder.Services.AddSingleton<IResearchService, ResearchService>();

            // Add Semantic Kernel
            builder.Services.AddSingleton<ISemanticKernel, KernelWrapper>(sp =>
            {
                var researchService = sp.GetRequiredService<IResearchService>();
                var repository = sp.GetRequiredService<ISearchRepository>();
                var client = new HttpClient(new LocalHostServer("http://localhost:1234/v1/chat/completions"));
                var kBuilder = Kernel.CreateBuilder()
                    .AddOpenAIChatCompletion(modelId: "llama3.1", apiKey: null, endpoint: new Uri("http://localhost:11434")); // With Ollama OpenAI API endpoint
                // .AddOpenAIChatCompletion(modelId: "llama3.1", apiKey: "lm-studio", httpClient: client);  // OR LM Studio

                kBuilder.Plugins.AddFromObject(new ResearchPlugin(researchService, repository), "Research");
                kBuilder.Services.AddSingleton<IAutoFunctionInvocationFilter, ResearchAgentFilter>();
                kBuilder.Services.AddLogging(l => l
                    .SetMinimumLevel(LogLevel.Trace)
                    .AddConsole()
                    .AddDebug());

                var kernel = kBuilder.Build();
                return new KernelWrapper(kernel, kernel.GetRequiredService<IChatCompletionService>());
            });

            // Add Agents
            builder.Services.AddSingleton<IResearchAgent, ResearchAgent>(sp =>
            {
                var kernel = sp.GetRequiredService<ISemanticKernel>();
                var memory = sp.GetRequiredService<ISemanticTextMemory>();
                return new ResearchAgent(kernel, memory);
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
