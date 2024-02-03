using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialog/TextDialogue"), Serializable]
public class TextDialogue : BaseDialogue
{
    public string actorName;
    public string text;
    public BaseDialogue nextDialogue;

    public override void Enter()
    {
        DialogueReader.instance.ShowDialogueBox();

        if (!string.IsNullOrEmpty(actorName))
        {
            DialogueReader.instance.ShowName();
            DialogueReader.instance.ChangeActorName(actorName);
        }
        else
        {
            DialogueReader.instance.HideName();
        }

        DialogueReader.instance.RequestText(text);
        DialogueReader.instance.onLeftMouseButtonDown.AddListener(EnterNext);
    }

    private void EnterNext()
    {
        if (nextDialogue == null) return;

        DialogueReader.instance.onLeftMouseButtonDown.RemoveListener(EnterNext);
        DialogueReader.instance.currentDialogue = nextDialogue;
        nextDialogue.Enter();
    }
}
