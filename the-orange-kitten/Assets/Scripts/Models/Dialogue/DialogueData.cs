using System.Xml.Serialization;

[XmlRoot(ElementName = "Dialogue")]
public class DialogueData
{
    [XmlAttribute(AttributeName = "bg")] public string BackgroundPath;
    [XmlAttribute(AttributeName = "portrait")] public string PortraitPath;
    [XmlElement()] public string Actor;
    [XmlElement()] public string Text;

    [XmlRoot(ElementName = "LocationFrame")]
    public class LocationData : DialogueData
    {
        [XmlAttribute(AttributeName = "name")] public string Location;
    }
}