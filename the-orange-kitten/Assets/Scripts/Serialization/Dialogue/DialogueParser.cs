using System;
using System.Collections.Generic;
using System.Xml;
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
        foreach (XmlNode frame in doc.ChildNodes)
        {
            switch (frame.Name)
            {
                case "BaseFrame":
                    resultList.Add(ParseBaseData(frame));
                    break;
                case "NamedFrame":
                    resultList.Add(ParseCharacterData(frame));
                    break;
                default:
                    throw new NullReferenceException("Data not found");
            }
        }

        return resultList;
    }

    private DialogueData ParseBaseData(XmlNode node)
    {
        var backgroundGraphic = node.Attributes!["bg"].InnerText;
        var text = node.SelectSingleNode("Text")!.InnerText;

        return new DialogueData(backgroundGraphic, text);
    }

    private DialogueData.CharacterDialogueData ParseCharacterData(XmlNode node)
    {
        var baseData = ParseBaseData(node);
        var character = node.SelectSingleNode("Actor")!.InnerText;

        return new DialogueData.CharacterDialogueData(baseData, character);
    }
}
