using AIResearchService.Interfaces;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace AIResearchService.Agents;

public class KernelWrapper(Kernel kernel, IChatCompletionService chat) : ISemanticKernel
{
    public async Task<FunctionResult> InvokePromptAsync(string prompt)
    {
        return await kernel.InvokePromptAsync(prompt);
    }

    public async Task<ChatMessageContent> ChatAsync(ChatHistory history, PromptExecutionSettings settings)
    {
        //kernel.Plugins.TryGetPlugin("Research", out var k);
        //k.TryGetFunction("start_research", out var f);
        //var s = k.get
        return await chat.GetChatMessageContentAsync(history, kernel: kernel, executionSettings: settings);
    }

    public string GetFunctions()
    {
        return "";
    }


}
