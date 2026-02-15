
using System.Xml.Serialization;

[XmlRoot(ElementName = "BaseDialogue")]
public class DialogueData
{

    [XmlAttribute(AttributeName = "bg")]
    public string BackgroundPath;
    [XmlElement()]
    public string Text;


    [XmlRoot(ElementName = "CharacterDialogue")]
    public class CharacterDialogueData : DialogueData
    {
        [XmlElement(ElementName = "Actor")]
        public string CharacterName;

        [XmlAttribute(AttributeName = "curr_portrait")]
        public string CurrentPortrait;

    }
}
