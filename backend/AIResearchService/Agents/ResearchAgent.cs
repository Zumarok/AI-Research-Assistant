#pragma warning disable SKEXP0001, SKEXP0010, SKEXP0020, SKEXP0050

using AIResearchService.Common;
using AIResearchService.Interfaces;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.Memory;

namespace AIResearchService.Agents;

public class ResearchAgent(ISemanticKernel kernel, ISemanticTextMemory memory) : IResearchAgent
{
    private readonly ChatHistory _history = [];
    private readonly OpenAIPromptExecutionSettings _settings = new()
    {
        Temperature = 0.1f,
        ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
    };

    public async Task<ChatMessageContent> InitAsync()
    {
        // Define the agent's role and initial message
        _history.AddSystemMessage("""
                                  You are Research-Agent, a capable and helpful AI agent that helps the user research topics.
                                  - You discuss a research topic with the user, and can call tools/functions when needed.
                                  - If the user responds with only a number, ex '0', assume they want you to fetch the content for that index fom the SearchRepository.
                                  - Think very carefully before calling functions.
                                  - When discussing the topic you can ONLY discuss the information that your tool calls return. Do not discuss the topic outside of the information that was returned.
                                  - If a tool call fails, only respond by telling the user so.
                                  - If a tool call result has very little info, just summarize what is in the result.
                                  - If the tool call result returns JSON data, parse it and discuss the content in detail. Focus on the actual information, including the content of comments, the authors, what each comment is responding to, the scores (upvotes), and the dates posted.
                                  - Do not mention or reference the data structure or format itself.
                                  
                                  A typical interaction with the user would go as follows:
                                  1) You greet the user and ask for a topic of research.
                                  2) The user give you a topic to research, and maybe a time frame.
                                  3) You use the 'ResearchPlugin-start_research' tool to fetch web results for this topic which will return a list of article titles to you and the user.
                                  4) You ask the user which of the web result topics they'd like to discuss.
                                  5) The user will choose one of the web results and ask you to fetch the content.
                                  6) You use the 'ResearchPlugin-fetch_content' tool to get the content fot that article. The returned data may be a video transcript, or a json formatted reddit thread. 
                                  7) You summarize the contents of the article and give a summary to the user, and ask if they'd like more information about that article.
                                  """);

        _history.AddUserMessage("Please introduce yourself and ask for a topic");

        //AddSimulatedHistory();

        // Generate the response using the Semantic Kernel
        var response = await kernel.ChatAsync(_history, _settings);
        _history.Add(response);

        return response;
    }

    public async Task<ChatMessageContent> ChatWithUserAsync(string userInput)
    {
        // userInput = "Let's research Local LLMs for the last week.";
        // Add the user's message to the chat history
        _history.AddUserMessage(userInput);

        var response = await kernel.ChatAsync(_history, _settings);
        _history.Add(response);

        return response;
    }

    /// <summary>
    /// Asks the LLM to determine if the fetched search result is relative to the topic.
    /// </summary>
    public async Task<ChatMessageContent> VerifyFetchedTitle(SearchResult search)
    {
        // Add the user's message to the chat history
        var chatHistoryCopy = new ChatHistory(_history);

        chatHistoryCopy.AddUserMessage($"""
                                       Determine if the following SearchResult is relevant to the topic. If it is, store the SearchResult to the SearchRepository, if it is not, simply reply 'Not Relevant'.
                                       {search.ToString()}
                                       """);

        // Generate the response using the Semantic Kernel
        var response = await kernel.ChatAsync(chatHistoryCopy, _settings);
        _history.Add(response);

        return response;
    }

    public async Task<bool> ResetAsync()
    {
        _history.Clear();
        return true;
    }

    public async Task<ChatMessageContent> GetContent(string title)
    {
        // _history.AddUserMessage($"Please fetch the content for {title} from the SearchRepository and give me a detailed summary.");
        _history.AddUserMessage(title);
        var response = await kernel.ChatAsync(_history, _settings);
        _history.Add(response);

        return response;
    }

    public void AddUserMessage(string message)
    {
        _history.AddUserMessage(message);
    }
    
    private void AddSimulatedHistory()
    {
        

        _history.AddAssistantMessage("""
                                     **Research Agent**
                                     
                                     Hello! I'm ResearchAgent, your personal research assistant. I'll dive through the vast amounts of online content on Reddit and YouTube to gather information relevant to our conversation. If you have a question or query on any subject, I'm here to help.
                                     
                                     To start an investigation, please specify what topic would you like me to explore. Don't hesitate to give me specific details, even if it's as simple as "generalities of cats."
                                     
                                     Go ahead, and let my research skills take over!
                                     """);

        _history.AddUserMessage("Research The best new AI Aided programming tools");

        var toolCall = new ChatMessageContent(AuthorRole.Assistant, new ChatMessageContentItemCollection()
        {
            new TextContent()
            {
                ModelId = "llama3.1",
                Metadata = new Dictionary<string, object?>()
                {
                    { "Id", "chatcmpl-296" },
                    { "Created", "2024-08-04T15:30:03+00:00" },
                    { "SystemFingerprint", "fp_ollama" },
                    { "FinishReason", "tool_calls" },
                    {
                        "ChatResponseMessage.FunctionToolCalls", new Dictionary<string, object?>
                        {
                            { "Name", "Research-start_research" },
                            {
                                "Arguments",
                                "{\u0022timeFrame\u0022:\u0022PastWeek\u0022,\u0022topic\u0022:\u0022new AI Aided programming tools\u0022}"
                            },
                            { "Id", "call_g250bhws" }
                        }
                    }
                }
            },
            new FunctionCallContent("start_research", "Research", "call_g250bhws", new KernelArguments()
            {
                { "timeFrame", "PastWeek" },
                { "topic", "new AI Aided programming tools" }
            })
        })
        {
            Content = "",
        };

        _history.Add(toolCall);

        // Simulate the tool response (as it would be returned by the actual tool)
        var toolResponse = new ChatMessageContent(AuthorRole.Tool, new ChatMessageContentItemCollection()
        {
            new TextContent()
            {
                ModelId = "llama3.1",
                Metadata = new Dictionary<string, object?>()
                {
                    { "Id", "toolresponse-001" },
                    { "Created", "2024-08-04T15:32:03+00:00" },
                    { "SystemFingerprint", "fp_tool" },
                    { "FinishReason", "success" }
                },
                Text = """
                       "[0]: Best AI Tools and Helpers Apps for Software Developers in 2024",
                       "[1]: Best AI Coding Tools for Developers in 2024",
                       "[2]: AI and the Future of Software Engineering",
                       "[3]: Tabnine Live: What\u2019s the best LLM for software development?",
                       "[4]: 5 Best AI Assistant Tools for Developers in 2024 | 5 AI Tools ...",
                       "[5]: NEW AGENTLESS AI Software Development",
                       "[6]: Mastering AI for Software Development: Essential Tips \u0026 ...",
                       "[7]: AI-Powered Software Development: Beyond Code Generation"
                       """
            },
            new FunctionResultContent("start_research", "Research", "call_g250bhws", result: new string(
                """
                "[0]: Best AI Tools and Helpers Apps for Software Developers in 2024",
                "[1]: Best AI Coding Tools for Developers in 2024",
                "[2]: AI and the Future of Software Engineering",
                "[3]: Tabnine Live: What\u2019s the best LLM for software development?",
                "[4]: 5 Best AI Assistant Tools for Developers in 2024 | 5 AI Tools ...",
                "[5]: NEW AGENTLESS AI Software Development",
                "[6]: Mastering AI for Software Development: Essential Tips \u0026 ...",
                "[7]: AI-Powered Software Development: Beyond Code Generation"
                """
                )
            )

        });

        _history.Add(toolResponse);

        // The assistant's message responding with the list and asking for further exploration
        _history.AddAssistantMessage("""
                                     "[0]: Best AI Tools and Helpers Apps for Software Developers in 2024",
                                     "[1]: Best AI Coding Tools for Developers in 2024",
                                     "[2]: AI and the Future of Software Engineering",
                                     "[3]: Tabnine Live: What\u2019s the best LLM for software development?",
                                     "[4]: 5 Best AI Assistant Tools for Developers in 2024 | 5 AI Tools ...",
                                     "[5]: NEW AGENTLESS AI Software Development",
                                     "[6]: Mastering AI for Software Development: Essential Tips \u0026 ...",
                                     "[7]: AI-Powered Software Development: Beyond Code Generation"

                                     Here is the list of subjects that I've found. Which would you like to explore?
                                     """);

        _history.AddUserMessage("Let's take a look at 4");

        var toolCallGetContent = new ChatMessageContent(AuthorRole.Assistant, new ChatMessageContentItemCollection()
        {
            new TextContent()
            {
                ModelId = "llama3.1",
                Metadata = new Dictionary<string, object?>()
                {
                    {"Id", "chatcmpl-296"},
                    {"Created", "2024-08-04T15:32:03+00:00"},
                    {"SystemFingerprint", "fp_ollama"},
                    {"FinishReason", "tool_calls"},
                    {"ChatResponseMessage.FunctionToolCalls", new Dictionary<string, object?>
                    {
                        {"Name","Research-fetch_content"},
                        {"Arguments", "{\u0022index\u0022:\u00224\u0022}"},
                        {"Id", "call_g251bhws"}
                    }}
                }
            },
            new FunctionCallContent("fetch_content", "Research", "call_g251bhws", new KernelArguments()
            {
                {"index", "4"}
            })
        });

        _history.Add(toolCallGetContent);

        // Simulate the tool response (as it would be returned by the actual tool)
        var toolResponseGetContent = new ChatMessageContent(AuthorRole.Tool, new ChatMessageContentItemCollection()
        {
            new TextContent()
            {
                ModelId = "llama3.1",
                Metadata = new Dictionary<string, object?>()
                {
                    {"Id", "toolresponse-002"},
                    {"Created", "2024-08-04T15:34:03+00:00"},
                    {"SystemFingerprint", "fp_tool"},
                    {"FinishReason", "success"}
                },
                Text = """
                       Intro
                       0:00
                       [Music]
                       0:02
                       hey what's up guys
                       0:04
                       here it is a full and complete routine
                       0:06
                       for balancing facial asymmetries
                       0:08
                       and it will help you also to tone your
                       0:10
                       skin and facial muscles
                       0:12
                       do it two to three times every day and
                       0:14
                       check the results after two weeks
                       0:16
                       now let's get started
                       0:19
                       [Music]
                       0:21
                       the first exercise the great joan open
                       0:23
                       up your mouth as if you would naturally
                       MASSETER
                       0:25
                       yawn
                       0:26
                       this exercise will help you to fix your
                       0:28
                       uneven jaw
                       0:33
                       good job let's go for another set this
                       0:35
                       is not an artificial movement
                       0:37
                       but it's a natural opening of your lower
                       0:40
                       chin
                       0:40
                       just like yawning
                       0:45
                       okay the next exercise the puffy cheek
                       ZYGOMATICUS
                       0:48
                       make your cheeks puffy and move the air
                       0:50
                       from left to right and right to left
                       0:52
                       this exercise will tone and balance your
                       0:54
                       zygomatics muscles
                       0:56
                       the muscles on your cheeks
                       """
            },
            new FunctionResultContent("fetch_content", "Research", "call_g251bhws", result: new string(
                    """
                    Intro
                    0:00
                    [Music]
                    0:02
                    hey what's up guys
                    0:04
                    here it is a full and complete routine
                    0:06
                    for balancing facial asymmetries
                    0:08
                    and it will help you also to tone your
                    0:10
                    skin and facial muscles
                    0:12
                    do it two to three times every day and
                    0:14
                    check the results after two weeks
                    0:16
                    now let's get started
                    0:19
                    [Music]
                    0:21
                    the first exercise the great joan open
                    0:23
                    up your mouth as if you would naturally
                    MASSETER
                    0:25
                    yawn
                    0:26
                    this exercise will help you to fix your
                    0:28
                    uneven jaw
                    0:33
                    good job let's go for another set this
                    0:35
                    is not an artificial movement
                    0:37
                    but it's a natural opening of your lower
                    0:40
                    chin
                    0:40
                    just like yawning
                    0:45
                    okay the next exercise the puffy cheek
                    ZYGOMATICUS
                    0:48
                    make your cheeks puffy and move the air
                    0:50
                    from left to right and right to left
                    0:52
                    this exercise will tone and balance your
                    0:54
                    zygomatics muscles
                    0:56
                    the muscles on your cheeks
                    """
                )
            )
        });

        _history.Add(toolResponseGetContent);

        _history.AddAssistantMessage("""
                                     This YouTube video discusses specific facial exercises and guides the viewer through them. Are there any specific details about the video you would like answered? 
                                     """);

        _history.AddUserMessage("Please clear the Repository and let's research a new subject.");

        var toolCallClearRepo = new ChatMessageContent(AuthorRole.Assistant, new ChatMessageContentItemCollection()
        {
            new TextContent()
            {
                ModelId = "llama3.1",
                Metadata = new Dictionary<string, object?>()
                {
                    {"Id", "chatcmpl-296"},
                    {"Created", "2024-08-04T15:32:03+00:00"},
                    {"SystemFingerprint", "fp_ollama"},
                    {"FinishReason", "tool_calls"},
                    {"ChatResponseMessage.FunctionToolCalls", new Dictionary<string, object?>
                    {
                        {"Name","Research-clear_repository"},
                        {"Id", "call_g253bhws"}
                    }}
                }
            },
            new FunctionCallContent("fetch_content", "Research", "call_g253bhws")
        });

        _history.Add(toolCallClearRepo);

        // Simulate the tool response (as it would be returned by the actual tool)
        var toolResponseClear = new ChatMessageContent(AuthorRole.Tool, new ChatMessageContentItemCollection()
        {
            new TextContent()
            {
                ModelId = "llama3.1",
                Metadata = new Dictionary<string, object?>()
                {
                    {"Id", "toolresponse-004"},
                    {"Created", "2024-08-04T15:35:03+00:00"},
                    {"SystemFingerprint", "fp_tool"},
                    {"FinishReason", "success"}
                }
            },
            new FunctionResultContent("fetch_content", "Research", "call_g253bhws", result: "")
        });

        _history.Add(toolResponseClear);

        _history.AddAssistantMessage("Let's start fresh with a new research topic. What subject would you like to explore?");
        _history.AddUserMessage("Let's research Local LLMs for the last week");
    }

}
