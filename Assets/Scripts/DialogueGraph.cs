using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueNodeData
{
    public BaseDialogue dialogue;
    public Rect nodePosition;

    public DialogueNodeData(BaseDialogue newDialogue, Rect newNodePosition)
    {
        dialogue = newDialogue;
        nodePosition = newNodePosition;
    }
}

[Serializable]
public class DialogueGraph : ScriptableObject
{
    public BaseDialogue startingPoint;
    public List<DialogueNodeData> dialogues = new List<DialogueNodeData>();
    public Rect startingPosition;
    public string savePath;
}
