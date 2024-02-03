using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class DialogueOption
{
    public string text;
    public BaseDialogue nextDialogue;
}

[CreateAssetMenu(menuName = "Dialog/OptionDialogue"), Serializable]
public class OptionDialogue : BaseDialogue
{
    public string actorName;
    public string text;
    public List<DialogueOption> options = new List<DialogueOption>();

    public override void Enter()
    {
        if (!string.IsNullOrEmpty(actorName))
        {
            DialogueReader.instance.ShowName();
            DialogueReader.instance.ChangeActorName(actorName);
        }

        DialogueReader.instance.RequestText(text);
        DialogueReader.instance.onLeftMouseButtonDown.AddListener(ShowOptions);
    }

    private void ShowOptions()
    {
        DialogueReader.instance.onLeftMouseButtonDown.RemoveListener(ShowOptions);

        List<string> optionTexts = new List<string>();

        options.ForEach(option => {
            optionTexts.Add(option.text);
        });

        DialogueReader.instance.isSkipping = false;
        DialogueReader.instance.LoadOptions(optionTexts);
        DialogueReader.instance.ShowOptions();
        DialogueReader.instance.HideDialogueBox();

        DialogueReader.instance.onOptionChosen.AddListener(EnterNext);
    }

    private void EnterNext(int index)
    {
        if (options[index].nextDialogue == null) return;

        DialogueReader.instance.HideOptions();
        DialogueReader.instance.UnloadOptions();

        DialogueReader.instance.onOptionChosen.RemoveListener(EnterNext);
        DialogueReader.instance.currentDialogue = options[index].nextDialogue;
        options[index].nextDialogue.Enter();
    }
}
