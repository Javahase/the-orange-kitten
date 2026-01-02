using System.Collections.Generic;

public class MockLLMService : LLMService
{
    public override IEnumerator<string> CreateChatConnection(ChatConnection chatConnection)
    {
        yield return chatConnection.message;
    }
}