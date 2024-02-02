using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueGraph : ScriptableObject
{
    public BaseDialogue startingPoint;
    public List<(BaseDialogue, Rect)> dialogues = new List<(BaseDialogue, Rect)>();
    public Rect startingPosition;
    public string savePath;
}
