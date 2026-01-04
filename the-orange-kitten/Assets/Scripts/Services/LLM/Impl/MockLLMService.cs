using System;
using System.Collections;
using System.Collections.Generic;

public class MockLLMService : LLMService
{

    List<RecipeStatement> _statements = new();

    public override void InitStatements(List<RecipeStatement> statements)
    {
        _statements = statements;
    }
    
    public override void ClearStatements()
    {
        _statements.Clear();
    }

    public override IEnumerator CreateChatConnection(RecipeStatement stmt, string request, Action<string> callback)
    {
        yield return request;
        callback.Invoke(request);
    }

}