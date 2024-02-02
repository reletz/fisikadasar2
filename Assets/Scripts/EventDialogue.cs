using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Dialog/EventDialogue")]
public  class EventDialogue : BaseDialogue
{
    public UnityEvent onTrigger;
    public BaseDialogue nextDialogue;

    public override void Enter()
    {
        onTrigger.Invoke();
    }

    private void EnterNext()
    {
        if (nextDialogue == null) return;

        DialogueReader.instance.currentDialogue = nextDialogue;
        nextDialogue.Enter();
    }
}