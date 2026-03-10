using System.Xml.Serialization;

namespace Models.Dialogue
{
    /// <summary>
    /// A class-representation of the Dialogue XML-Data, notably a Dialogue-Node within the XML File.<br/>
    /// Used to create <see cref="GeneratedDialogueSequence">GeneratedDialogueSequences</see>
    /// </summary>
    [XmlRoot(ElementName = "Dialogue")]
    public class DialogueData
    {
        /// <summary>
        /// (Mandatory)<br/>
        /// What the background of the frame should be. Based in the <c>Resources</c>-Folder.
        /// </summary>
        [XmlAttribute(AttributeName = "bg")] public string BackgroundPath;
        
        /// <summary>
        /// (Not mandatory)<br/>
        /// What the background of the frame should be. Based in the <c>Resources</c>-Folder.
        /// </summary>
        [XmlAttribute(AttributeName = "portrait")] public string PortraitPath;
        
        /// <summary>
        /// (Not mandatory)<br/>
        /// Who speaks in the current frame. Will get separately displayed above the text box, if set.
        /// </summary>
        [XmlElement()] public string Actor;
        
        /// <summary>
        /// (Mandatory)<br/>
        /// The text displayed in the text box. Will be parsed with special rules.
        /// </summary>
        [XmlElement()] public string Text;

        
        /// <summary>
        /// A class-representation of the LocationFrame XML-Data, notably a LocationFrame-Node within the XML File.<br/>
        /// Used to create <see cref="GeneratedDialogueSequence">GeneratedDialogueSequences</see>
        /// </summary>
        [XmlRoot(ElementName = "LocationFrame")]
        public class LocationData : DialogueData
        {
            /// <summary>
            /// (Mandatory)<br/>
            /// The location displayed in the text frame. Will be centered. Makes <see cref="DialogueData.Text"/> obsolete.
            /// </summary>
            [XmlAttribute(AttributeName = "name")] public string Location;
        }
    }
}