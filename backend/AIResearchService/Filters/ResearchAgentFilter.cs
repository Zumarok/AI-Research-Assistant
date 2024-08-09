#pragma warning disable SKEXP0001, SKEXP0010, SKEXP0020, SKEXP0050

using Microsoft.SemanticKernel;

namespace AIResearchService.Filters;

public class ResearchAgentFilter : IAutoFunctionInvocationFilter
{
    public async Task OnAutoFunctionInvocationAsync(AutoFunctionInvocationContext context, Func<AutoFunctionInvocationContext, Task> next)
    {
        try
        {
            // Example: get chat history
            var chatHistory = context.ChatHistory;

            // Example: get information about all functions which will be invoked
            //var functionCalls = FunctionCallContent.GetFunctionCalls(context.ChatHistory.Last());

            // Calling next filter in pipeline or function itself.
            // By skipping this call, next filters and function won't be invoked, and function call loop will proceed to the next function.
            await next(context);

            switch (context.Function.Name)
            {
                case "start_research":
                    var researchList = context.Result.GetValue<string>();
                    var modified = $"""
                                    The user has received a list of article titles along with their corresponding index in the SearchRepository.
                                    Respond by asking the user which article index they'd like to fetch content for, with no additional commentary.
                                    
                                    Here is the list, for your reference:
                                    {researchList}
                                    """;
                    context.Result = new FunctionResult(context.Result, modified);
                    break;
                case "fetch_content":
                    var result = context.Result.GetValue<string>();
                    //var index = Convert.ToInt32(context.Arguments?.First().Value);
                    var m = $"""
                             

                             {result}
                             """;
                    context.Result = new FunctionResult(context.Result, m);
                    break;
                case "get_repository_titles":
                    break;
                case "clear_repository":
                    break;
                default:
                    break;
            }

            



            // Example: get request sequence index
            //Console.WriteLine($"Request sequence index: {context.RequestSequenceIndex}");

            //// Example: get function sequence index
            //this._logger.LogDebug("Function sequence index: {FunctionSequenceIndex}", context.FunctionSequenceIndex);

            //// Example: get total number of functions which will be called
            //this._logger.LogDebug("Total number of functions: {FunctionCount}", context.FunctionCount);





            //// Example: override function result value
            //context.Result = new FunctionResult(context.Result, "Result from auto function invocation filter");

            //// Example: Terminate function invocation
            //context.Terminate = true;
        }
        catch (Exception e)
        {
            var modified = $"""
                            The tool call has resulted in an error.
                            Please ask the user to try again, and nothing else.
                            """;
            context.Result = new FunctionResult(context.Result, modified);
        }
       
    }
}
