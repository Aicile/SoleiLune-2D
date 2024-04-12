using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string speaker;
    [TextArea(3, 10)]
    public string text; 

   
    public DialogueLine(string speaker, string text)
    {
        this.speaker = speaker;
        this.text = text;
    }
}
