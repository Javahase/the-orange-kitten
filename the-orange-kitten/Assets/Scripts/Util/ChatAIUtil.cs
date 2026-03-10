using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
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
        List<ChatAIMessage> chatHistory,
        int timeout = 0)
    {
        var messages = new JArray(chatHistory.Select(it => it.ToJObject()));
        var json = new JObject()
        {
            { "model", ChatAIModel.GetApiModel(model) },
            { "messages", messages },
        };

        using var req = UnityWebRequest.Post(CompletionsEndpoint, json.ToString(), "application/json");
        req.SetRequestHeader("Authorization", "Bearer " + apiKey);
        req.timeout = timeout;
        yield return req.SendWebRequest();

        yield return
            req.result != UnityWebRequest.Result.Success
                ? WebEither<string, ChatAIMessage>.Left($"Error: {req.error}\nMessage: {req.downloadHandler.text}")
                : WebEither<string, string>.Right(req.downloadHandler.text)
                    .MapNonNull(JObject.Parse, $"Couldn't parse text json: {req.downloadHandler.text}")
                    .GetAsJArray("choices")
                    .MapNonNull(choices => choices.First() as JObject, "Empty JArray!")
                    .GetAsJObject("message")
                    .GetAsString("content")
                    .Map(ChatAIMessage.CreateAssistantMessage);
    }
}