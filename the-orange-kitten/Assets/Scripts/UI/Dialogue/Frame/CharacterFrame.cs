using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterFrame : BaseFrame
{
    public string characterName;
    public Sprite characterImage;
    private TMP_Text _characterNameField;
    private Image _characterImage;

    public override void OnInitialize()
    {
        base.OnInitialize();
        _characterNameField = transform.Find("DialogueCharacterName/CharacterName").GetComponent<TMP_Text>();
        _characterNameField.text = characterName;
        _characterImage = transform.Find("CharacterSprite").GetComponent<Image>();
        if (string.IsNullOrEmpty(characterName))
        {
            _characterNameField.gameObject.SetActive(false);
        }
        
        if (characterImage)
        {
            _characterImage.sprite = characterImage;
        }
        else
        {
            _characterImage.gameObject.SetActive(false);
        }
    }
}
