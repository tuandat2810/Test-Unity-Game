using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/Dialogue Node")]
public class DialogueNode : ScriptableObject
{
    public string speakerName; // Name of the character speaking (e.g., "Guard", "Merchant").

    [TextArea(3, 8)]
    public string dialogueLine; // The line the NPC says.

    // List of possible player responses to this dialogue line.
    // If this list is empty, it's a linear "click-to-continue" line.
    public List<PlayerResponse> playerResponses = new List<PlayerResponse>();
}
