using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;


/// <summary>
/// Structure Base Frame:
///
/// <BaseFrame bg="asset">
///     <Text>
///         Dialogue Text.
///     </Text>
/// </BaseFrame>
///
/// <NamedFrame bg="asset">
///     <Actor>Name</Actor>
///     <Text>
///         Dialogue Text.
///     </Text>
/// </NamedFrame>
///
/// 
/// </summary>
public class DialogueParser
{
    private readonly string _content;

    public DialogueParser(string content)
    {
        _content = content;
    }

    public List<DialogueData> ParseDialogue()
    {
        var resultList = new List<DialogueData>();
        var doc = new XmlDocument();
        doc.LoadXml(_content);
        
        var serializer = XmlSerializer.FromTypes(new[]
            { 
                typeof(DialogueData.CharacterDialogueData), typeof(DialogueData)
            }
        );
        foreach (XmlNode frame in doc.DocumentElement!.ChildNodes)
        {
            Debug.Log(frame.OuterXml);
            using XmlReader reader = new XmlNodeReader(frame);
            resultList.Add(serializer.First(s => s.CanDeserialize(reader)).Deserialize(reader) as DialogueData);
        }
        return resultList;
    }
}