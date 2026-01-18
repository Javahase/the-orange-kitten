using UnityEngine;
using UnityEngine.UI;

public class GeneratedDialogueSequence : DialogueSequence
{
    [SerializeField] private TextAsset dialogueFile;

    [Header("Prefab Resources")] [SerializeField]
    private BaseFrame baseFramePrefab;

    [SerializeField] private CharacterFrame characterFramePrefab;

    public override void OnInitialize()
    {
        if (Initialized)
            return;
        var parser = new DialogueParser(dialogueFile);
        var dialogues = parser.ParseDialogue();
        foreach (var dialogue in dialogues)
        {
            switch (dialogue)
            {
                case DialogueData.CharacterDialogueData characterDialogueData:
                    CreateCharacterFrame(characterDialogueData);
                    break;
                case not null:
                    CreateBaseFrame(dialogue);
                    break;
                default:
                    Debug.LogError(
                        $"Couldn't load Generated Dialogue Sequence \"{gameObject.name}\": Failed parsing dialogue file \"{dialogueFile.name}\"."
                    );
                    break;
            }
        }

        base.OnInitialize();
    }

    private GameObject InstantiateFrameBase(BaseFrame prefab, DialogueData data)
    {
        var baseFrameObj = Instantiate(prefab.gameObject, transform);
        var baseFrame = baseFrameObj.GetComponent<BaseFrame>();
        var image = baseFrame.GetComponent<Image>();
        baseFrame.dialogueText = data.Text;
        if (data.BackgroundPath != "")
        {
            image.sprite = Resources.Load<Sprite>(data.BackgroundPath);
        }

        return baseFrameObj;
    }

    private void CreateBaseFrame(DialogueData data) => InstantiateFrameBase(baseFramePrefab, data);

    private void CreateCharacterFrame(DialogueData.CharacterDialogueData data)
    {
        var characterFrame = InstantiateFrameBase(characterFramePrefab, data).GetComponent<CharacterFrame>();
        characterFrame.characterName = data.CharacterName;
    }
}