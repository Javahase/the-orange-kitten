using UnityEngine;

public class GeneratedDialogueSequence: DialogueSequence
{

    [SerializeField] private TextAsset dialogueFile;
    
    [Header("Prefab Resources")]
    [SerializeField] private BaseFrame baseFramePrefab;

    [SerializeField] private CharacterFrame characterFramePrefab;
    
    public override void OnInitialize()
    {
        var parser = new DialogueParser(dialogueFile.text);
        var data = parser.ParseDialogue();
        foreach (var dat in data)
        {
            // Do something...
        }
        
        base.OnInitialize();
    }
}