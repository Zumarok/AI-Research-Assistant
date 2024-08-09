using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace AIResearchService.Interfaces;

public interface ISemanticKernel
{
    Task<FunctionResult> InvokePromptAsync(string prompt);
    Task<ChatMessageContent> ChatAsync(ChatHistory history, PromptExecutionSettings settings);

}

