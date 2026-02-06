using System;
using Newtonsoft.Json.Linq;
using UnityEngine;

[Serializable]
public struct ChatAIMessage
{
    public string content;
    public string role;

    public static ChatAIMessage CreateSystemPrompt(string theContent) => new ChatAIMessage
        { content = theContent, role = "system" };

    public static ChatAIMessage CreateUserMessage(string theContent) => new ChatAIMessage
        { content = theContent, role = "user" };

    public static ChatAIMessage CreateAssistantMessage(string theContent) => new ChatAIMessage
    {
        content = theContent, role = "assistant"
    };

    public string ToJson()
        => JsonUtility.ToJson(this);
        
    public JObject ToJObject() => JObject.Parse(ToJson()); // Sure why not
}