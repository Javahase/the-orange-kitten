using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class LLMService : SingletonObject<LLMService>
{

    public abstract void InitStatements(List<RecipeStatement> statements);

    public abstract void ClearStatements();

    public abstract IEnumerator CreateChatConnection(RecipeStatement stmt, string request, Action<string> callback);
}
