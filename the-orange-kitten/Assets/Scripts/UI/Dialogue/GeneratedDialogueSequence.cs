using Models.Dialogue;
using Serialization.Dialogue;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GeneratedDialogueSequence : DialogueSequence
{
    [SerializeField] private TextAsset dialogueFile;

    [Header("Prefab Resources")] [SerializeField]
    private CharacterFrame baseFramePrefab;

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
                case DialogueData.LocationData locationData:
                    CreateLocationFrame(locationData);
                    break;
                case not null:
                    CreateCharacterFrame(dialogue);
                    break;
                case null:
                    Debug.LogError("Unknown Frame Type!");
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

    private void CreateLocationFrame(DialogueData.LocationData data)
    {
        var baseFrame = InstantiateFrameBase(baseFramePrefab, data).GetComponent<BaseFrame>();
        baseFrame.locationText = true;
        baseFrame.dialogueText = $"Location:^1 {data.Location}";
    }

    private void CreateCharacterFrame(DialogueData data)
    {
        var characterFrame = InstantiateFrameBase(baseFramePrefab, data).GetComponent<CharacterFrame>();
        characterFrame.characterName = data.Actor;
        if (data.PortraitPath != "")
        {
            characterFrame.characterImage = Resources.Load<Sprite>(data.PortraitPath);
        }
    }
}