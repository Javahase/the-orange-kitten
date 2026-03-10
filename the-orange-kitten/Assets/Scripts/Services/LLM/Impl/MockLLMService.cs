using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

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

    public override IEnumerator SendMessageToConversation(RecipeStatement stmt, string request)
    {
        yield return request;
    }

    public override void SetTopicPrompt(string prompt)
    {
    }

    public override List<RecipeStatement> GetStatements() => _statements;
}