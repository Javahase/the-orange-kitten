using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class ChatAIUtil
{
    /// <summary>
    /// Chat AI general API endpoint.
    /// </summary>
    private const string ApiEndpoint = "https://chat-ai.academiccloud.de/v1";

    /// <summary>
    /// The completions endpoint for completing messages.
    /// </summary>
    private const string CompletionsEndpoint = ApiEndpoint + "/chat/completions";

    public static IEnumerator SendMessage(
        string apiKey,
        ChatAIModel.Model model,
        List<ChatAIMessage> chatHistory)
    {
        var messages = new JArray(chatHistory.Select(it => it.ToJObject()));
        var json = new JObject()
        {
            { "model", ChatAIModel.GetApiModel(model) },
            { "messages", messages },
        };

        using var req = UnityWebRequest.Post(CompletionsEndpoint, json.ToString(), "application/json");
        req.SetRequestHeader("Authorization", "Bearer " + apiKey);
        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            var raw = req.downloadHandler.text;
            var result = JObject.Parse(raw)?["choices"] as JArray;
            var messageObj = result?.ElementAt(0) as JObject;
            
            var message = (messageObj?["message"] as JObject)?["content"]?.ToObject<string>();
            
            yield return WebEither<string, ChatAIMessage>.Either($"Malformed Json: {raw}", ChatAIMessage.CreateAssistantMessage(message));
        }
        else
        {
            yield return WebEither<string, ChatAIMessage>.Left($"Error: {req.error}\nMessage: {req.downloadHandler.text}");
        }
    }
}