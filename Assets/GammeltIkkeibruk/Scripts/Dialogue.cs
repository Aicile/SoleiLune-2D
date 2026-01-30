using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
public class Dialogue : ScriptableObject
{
    public DialogueLine[] lines;

    // Method to get formatted lines
    public string GetLine(int index, string replacement)
    {
        if (index >= 0 && index < lines.Length)
        {
            return lines[index].text.Replace("{potion}", replacement);
        }
        return "";
    }
}
