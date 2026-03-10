using System;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Models.LLM
{
    /// <summary>
    /// Represents a message object as required by ChatAI. Can be serialized into JSON.
    ///
    /// <seealso href="https://developers.openai.com/api/reference/resources/chat#(resource)%20chat.completions%20%3E%20(model)%20chat_completion_message_param%20%3E%20(schema)"/>
    /// </summary>
    [Serializable]
    public struct ChatAIMessage
    {
        /// <summary>
        /// The content of the message.
        /// </summary>
        public string content;
        
        /// <summary>
        /// Who sent the message. Can consist of:
        /// <list type="bullet">
        /// <item><description><b>system</b>: The highest priority prompt. Used for a general rule framework.</description></item>
        /// <item><description><b>user</b>: A message sent by the user.</description></item>
        /// <item><description><b>assistant</b>: A message sent by the LLM.</description></item>
        /// </list>
        /// </summary>
        public string role;

        /// <summary>
        /// Creates a system prompt message.
        /// </summary>
        /// <param name="theContent">The message.</param>
        /// <returns>A <see cref="ChatAIMessage"/>-Object with "system"-role.</returns>
        public static ChatAIMessage CreateSystemPrompt(string theContent) =>
            new() { content = theContent, role = "system" };

        
        /// <summary>
        /// Creates a message sent by the user.
        /// </summary>
        /// <param name="theContent">The message.</param>
        /// <returns>A <see cref="ChatAIMessage"/>-Object with "user"-role.</returns>
        public static ChatAIMessage CreateUserMessage(string theContent) =>
            new() { content = theContent, role = "user" };

        /// <summary>
        /// Creates a message sent by the LLM.
        /// </summary>
        /// <param name="theContent">The message.</param>
        /// <returns>A <see cref="ChatAIMessage"/>-Object with "assistant"-role.</returns>
        public static ChatAIMessage CreateAssistantMessage(string theContent) => new()
        {
            content = theContent, role = "assistant"
        };

        /// <summary>
        /// Converts this object to JSON.
        /// </summary>
        /// <returns>A json-string.</returns>
        public string ToJson()
            => JsonUtility.ToJson(this);

        /// <summary>
        /// Converts this object into a <see cref="JObject"/>.
        /// </summary>
        /// <returns>A <see cref="JObject"/> with fields "role" and "content".</returns>
        public JObject ToJObject() => JObject.Parse(ToJson()); // Sure why not
    }
}