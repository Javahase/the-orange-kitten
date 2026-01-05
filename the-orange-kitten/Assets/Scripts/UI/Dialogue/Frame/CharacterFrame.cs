using TMPro;
using UnityEngine;

public class CharacterFrame : BaseFrame
{
    public string characterName;
    private TMP_Text _characterNameField;

    public override void Init()
    {
        base.Init();
        _characterNameField = transform.Find("DialogueCharacterName/CharacterName").GetComponent<TMP_Text>();
        _characterNameField.text = characterName;
    }
}
