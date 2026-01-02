using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LLMService : SingletonObject<LLMService>
{
    public abstract IEnumerator<string> CreateChatConnection(ChatConnection chatConnection);

}
