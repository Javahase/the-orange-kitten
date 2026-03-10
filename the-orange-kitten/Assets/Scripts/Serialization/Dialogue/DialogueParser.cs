using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Models.Dialogue;
using UnityEngine;

namespace Serialization.Dialogue
{
    /// <summary>
    /// Parses dialogue from any XML-String.
    /// </summary>
    public class DialogueParser
    {
        /// <summary>
        /// The XML-String that should be parsed.
        ///
        /// For more info, check out the <c>Dialogue.dtd</c> file.
        /// </summary>
        private readonly string _content;

        /// <summary>
        /// Initializes a dialogue parser using direct content for text.
        /// </summary>
        /// <param name="content">An XML-String that should be parsed.</param>
        public DialogueParser(string content)
        {
            _content = content;
        }

        /// <summary>
        /// Initializes a dialogue parser using a <see cref="TextAsset"/>.
        /// </summary>
        /// <param name="dialogueResource">The dialogue file.</param>
        public DialogueParser(TextAsset dialogueResource) : this(dialogueResource.text)
        {
        }

        /// <summary>
        /// Parses the <see cref="_content">content</see> into <see cref="DialogueData"/>.
        /// </summary>
        public List<DialogueData> ParseDialogue()
        {
            var resultList = new List<DialogueData>();
            var doc = new XmlDocument();
            doc.LoadXml(_content);

            var serializer = XmlSerializer.FromTypes(new[]
                {
                    typeof(DialogueData.LocationData), typeof(DialogueData)
                }
            );
            foreach (XmlNode frame in doc.DocumentElement!.ChildNodes)
            {
                using XmlReader reader = new XmlNodeReader(frame);
                resultList.Add(serializer.First(s => s.CanDeserialize(reader)).Deserialize(reader) as DialogueData);
            }

            return resultList;
        }
    }
}