public class DialogueData
{

    public string BackgroundPath;
    public string Text;

    public DialogueData(string backgroundPath, string text)
    {
        BackgroundPath = backgroundPath;
        Text = text;
    }

    public class CharacterDialogueData : DialogueData
    {
        public string CharacterName;
        
        public CharacterDialogueData(string backgroundPath, string text, string characterName) : base(backgroundPath, text)
        {
            CharacterName = characterName;
        }

        public CharacterDialogueData(DialogueData baseData, string characterName) : this(baseData.BackgroundPath,
            baseData.Text, characterName) {}
    }
}
