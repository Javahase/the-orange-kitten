using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Networking;

public class ChatAILLMService : LLMService
{
    private const string ApiEndpoint = "https://chat-ai.academiccloud.de/v1";
    private const string CompletionsEndpoint = ApiEndpoint + "/chat/completions";

    private List<RecipeStatement> _statements = new();
    [SerializeField] private Model model;

    private readonly Dictionary<Model, string> _apiModel = new()
    {
        { Model.Apertus_70B_Instruct_2509, "apertus-70b-instruct-2509" },
        { Model.DeepSeek_R1_0528, "deepseek-r1-0528" },
        { Model.DeepSeek_R1_Distill_Llama_70B, "deepseek-r1-distill-llama-70b" },
        { Model.Gemma_3_27B_Instruct, "gemma-3-27b-it" },
        { Model.GLM_4_7, "glm-4.7" },
        { Model.InternVL_3_5_30B_A3B, "internvl3.5-30b-a3b" },
        { Model.Llama_3_1_SauerkrautLM_70B_Instruct, "llama-3.1-sauerkrautlm-70b-instruct" },
        { Model.MedGemma_27B_Instruct, "medgemma-27b-it" },
        { Model.Llama_3_1_8B_Instruct, "meta-llama-3.1-8b-instruct" },
        { Model.LLama_3_3_70B_Instruct, "llama-3.3-70b-instruct" },
        { Model.Mistral_Large_Instruct, "mistral-large-instruct" },
        { Model.GPT_OSS_120B, "openai-gpt-oss-120b" }
    };

    private Dictionary<RecipeStatement, List<Message>> _conversationMap = new();

    private string _apiKey;

    private void Start()
    {
        var file = Resources.Load<TextAsset>("Secrets/api_key");
        _apiKey = file.text;
    }

    public override void InitStatements(List<RecipeStatement> statements)
    {
        _statements = statements;
        Debug.Log("Initializing as BEST as I can!");
    }

    public override void ClearStatements()
    {
        _conversationMap.Clear();
    }

    public override IEnumerator SendMessageToConversation(RecipeStatement stmt, string request, Action<string> callback)
    {
        if (!_conversationMap.ContainsKey(stmt))
        {
            _conversationMap.Add(stmt, new List<Message>() { Message.CreateSystemPrompt("Respond in maximum 50 words.") });
        }
        if (!_conversationMap.TryGetValue(stmt, out var messages)) yield break;

        messages.Add(Message.CreateUserMessage(request));

        var json = new JObject()
        {
            { "model", GetApiModel() },
            { "messages", new JArray(messages.Select(it => it.ToJObject())) },
        };

        using var req = UnityWebRequest.Post(CompletionsEndpoint, json.ToString(), "application/json");
        req.SetRequestHeader("Authorization", "Bearer " + _apiKey);
        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            var result = JObject.Parse(req.downloadHandler.text);
            var message = result["choices"]![0]!["message"]!["content"]!.ToObject<string>();
            callback.Invoke(message);
            messages.Add(Message.CreateAssistantMessage(message));
        }
        else
        {
            Debug.LogError(req.downloadHandler.text);
            Debug.LogError(req.error);
        }
    }

    private string GetApiModel() => _apiModel[model];

    [Serializable]
    private struct Message
    {
        public string content;
        public string role;

        public static Message CreateSystemPrompt(string theContent) => new Message
            { content = theContent, role = "system" };

        public static Message CreateUserMessage(string theContent) => new Message
            { content = theContent, role = "user" };

        public static Message CreateAssistantMessage(string theContent) => new Message
        {
            content = theContent, role = "assistant"
        };

        public string ToJson()
            => JsonUtility.ToJson(this);
        
        public JObject ToJObject() => JObject.Parse(ToJson()); // Sure why not
    }

    [Serializable]
    public enum Model
    {
        /// <summary>
        /// <p>Organization: Swiss AI (Switzerland)</p>
        /// <p>Open: Yes</p>
        /// <p>Knowledge cutoff: Apr 2024</p>
        /// <p>Context window in tokens: 65k</p>
        /// <p>Advantages: Fully open-source, Multilingual</p>
        /// <p>Limitations: None</p>
        ///
        /// See also: <a href="https://docs.hpc.gwdg.de/services/chat-ai/models/index.html"/>
        /// </summary>
        Apertus_70B_Instruct_2509,

        /// <summary>
        /// <p>Organization: DeepSeek (Canada)</p>
        /// <p>Open: Yes</p>
        /// <p>Knowledge cutoff: Dec 2023</p>
        /// <p>Context window in tokens: 32k</p>
        /// <p>Advantages: Great overall performance, reasoning and problem-solving</p>
        /// <p>Limitations: Censorship, political bias</p>
        /// 
        /// See also: <a href="https://docs.hpc.gwdg.de/services/chat-ai/models/index.html"/>
        /// </summary>
        DeepSeek_R1_0528,

        /// <summary>
        /// <p>Organization: DeepSeek (Canada)</p>
        /// <p>Open: Yes</p>
        /// <p>Knowledge cutoff: Dec 2023</p>
        /// <p>Context window in tokens: 32k</p>
        /// <p>Advantages: Great overall performance, faster than R1</p>
        /// <p>Limitations: Censorship, political bias</p>
        /// 
        /// See also: <a href="https://docs.hpc.gwdg.de/services/chat-ai/models/index.html"/>
        /// </summary>
        DeepSeek_R1_Distill_Llama_70B,

        /// <summary>
        /// <p>Organization: Google (United States)</p>
        /// <p>Open: Yes</p>
        /// <p>Knowledge cutoff: Mar 2024</p>
        /// <p>Context window in tokens: 128k</p>
        /// <p>Advantages: Vision, great overall performance</p>
        /// <p>Limitations: None</p>
        /// 
        /// See also: <a href="https://docs.hpc.gwdg.de/services/chat-ai/models/index.html"/>
        /// </summary>
        Gemma_3_27B_Instruct,

        /// <summary>
        /// <p>Organization: Z.ai (Singapore)</p>
        /// <p>Open: Yes</p>
        /// <p>Knowledge cutoff: Apr 2025</p>
        /// <p>Context window in tokens: 200k</p>
        /// <p>Advantages: Great performance</p>
        /// <p>Limitations: None</p>
        /// 
        /// See also: <a href="https://docs.hpc.gwdg.de/services/chat-ai/models/index.html"/>
        /// </summary>
        GLM_4_7,

        /// <summary>
        /// <p>Organization: OpenGVLab (Canada)</p>
        /// <p>Open: Yes</p>
        /// <p>Knowledge cutoff: Aug 2023</p>
        /// <p>Context window in tokens: 40k</p>
        /// <p>Advantages: Vision, lightweight and fast</p>
        /// <p>Limitations: None</p>
        /// 
        /// See also: <a href="https://docs.hpc.gwdg.de/services/chat-ai/models/index.html"/>
        /// </summary>
        InternVL_3_5_30B_A3B,

        /// <summary>
        /// <p>Organization: VAGOsolutions x Meta (Germany)</p>
        /// <p>Open: Yes</p>
        /// <p>Knowledge cutoff: Dec 2023</p>
        /// <p>Context window in tokens: 128k</p>
        /// <p>Advantages: German language skills</p>
        /// <p>Limitations: None</p>
        /// 
        /// See also: <a href="https://docs.hpc.gwdg.de/services/chat-ai/models/index.html"/>
        /// </summary>
        Llama_3_1_SauerkrautLM_70B_Instruct,

        /// <summary>
        /// <p>Organization: Google (United States)</p>
        /// <p>Open: Yes</p>
        /// <p>Knowledge cutoff: Mar 2024</p>
        /// <p>Context window in tokens: 128k</p>
        /// <p>Advantages: Vision, medical knowledge</p>
        /// <p>Limitations: None</p>
        /// 
        /// See also: <a href="https://docs.hpc.gwdg.de/services/chat-ai/models/index.html"/>
        /// </summary>
        MedGemma_27B_Instruct,

        /// <summary>
        /// <p>Organization: Meta (United States)</p>
        /// <p>Open: Yes</p>
        /// <p>Knowledge cutoff: Dec 2023</p>
        /// <p>Context window in tokens: 128k</p>
        /// <p>Advantages: Fast overall performance</p>
        /// <p>Limitations: None</p>
        /// 
        /// See also: <a href="https://docs.hpc.gwdg.de/services/chat-ai/models/index.html"/>
        /// </summary>
        Llama_3_1_8B_Instruct,

        /// <summary>
        /// <p>Organization: Meta (United States)</p>
        /// <p>Open: Yes</p>
        /// <p>Knowledge cutoff: Dec 2023</p>
        /// <p>Context window in tokens: 128k</p>
        /// <p>Advantages: Good overall performance, reasoning and creative writing</p>
        /// <p>Limitations: None</p>
        /// 
        /// See also: <a href="https://docs.hpc.gwdg.de/services/chat-ai/models/index.html"/>
        /// </summary>
        LLama_3_3_70B_Instruct,

        /// <summary>
        /// <p>Organization: Mistral (France)</p>
        /// <p>Open: Yes</p>
        /// <p>Knowledge cutoff: Jul 2024</p>
        /// <p>Context window in tokens: 128k</p>
        /// <p>Advantages: Good overall performance, coding and multilingual reasoning</p>
        /// <p>Limitations: None</p>
        /// 
        /// See also: <a href="https://docs.hpc.gwdg.de/services/chat-ai/models/index.html"/>
        /// </summary>
        Mistral_Large_Instruct,

        /// <summary>
        /// <p>Organization: OpenAI (United States)</p>
        /// <p>Open: Yes</p>
        /// <p>Knowledge cutoff: Jun 2024</p>
        /// <p>Context window in tokens: 128k</p>
        /// <p>Advantages: Great overall performance, fast</p>
        /// <p>Limitations: None</p>
        /// 
        /// See also: <a href="https://docs.hpc.gwdg.de/services/chat-ai/models/index.html"/>
        /// </summary>
        GPT_OSS_120B
    }
}