using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using System.Linq;
using UnityEngine.UIElements;

public abstract class DialogueNode : Node
{
    public abstract void Save(string directory, string name = null);
    public abstract BaseDialogue GetDialogue();
}

public class StartingNode : DialogueNode
{
    public static StartingNode Create()
    {
        StartingNode newNode = new StartingNode();
        newNode.capabilities &= ~(Capabilities.Deletable | Capabilities.Copiable);
        newNode.RenderNode();

        return newNode;
    }

    public void RenderNode()
    {
        title = "Start";

        Port outPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
        outPort.name = "OutPort";
        outPort.portName = "   ";
        outputContainer.Add(outPort);

        RefreshExpandedState();
        RefreshPorts();
    }

    public override void Save(string directory, string name = null) {   }
    public override BaseDialogue GetDialogue() { return null; }
}

public class TextDialogueNode : DialogueNode
{
    public TextDialogue textDialogue;

    public static TextDialogueNode Create(TextDialogue textDialogue)
    {
        TextDialogueNode newNode = new TextDialogueNode() { textDialogue = textDialogue };
        newNode.RenderNode();

        return newNode;
    }

    public void RenderNode()
    {
        title = "Text Dialogue";

        Label actorLabel = new Label("Actor");
        extensionContainer.Add(actorLabel);

        ScrollView actorScrollView = new ScrollView(ScrollViewMode.Horizontal);
        actorScrollView.style.width = 200;
        extensionContainer.Add(actorScrollView);

        TextField actorText = new TextField()
        {
            name = string.Empty,
            value = textDialogue.actorName,
            multiline = true,
        };
        actorText.style.whiteSpace = WhiteSpace.Normal;
        actorText.RegisterValueChangedCallback(evt => { textDialogue.actorName = evt.newValue; EditorUtility.SetDirty(textDialogue); });
        actorScrollView.Add(actorText);

        Label dialogueLabel = new Label("Dialogue");
        extensionContainer.Add(dialogueLabel);

        ScrollView scrollView = new ScrollView(ScrollViewMode.Horizontal);
        scrollView.style.width = 200;
        extensionContainer.Add(scrollView);

        TextField dialogueText = new TextField()
        {
            name = string.Empty,
            value = textDialogue.text,
            multiline = true,
        };
        dialogueText.style.whiteSpace = WhiteSpace.Normal;
        dialogueText.RegisterValueChangedCallback(evt => { textDialogue.text = evt.newValue; EditorUtility.SetDirty(textDialogue); });
        scrollView.Add(dialogueText);

        Port inPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
        inPort.name = "InPort";
        inPort.portName = "   ";
        inputContainer.Add(inPort);

        Port outPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
        outPort.name = "OutPort";
        outPort.portName = "   ";
        outputContainer.Add(outPort);

        RefreshExpandedState();
        RefreshPorts();
    }

    public override void Save(string directory, string name = null)
    {
        if (string.IsNullOrEmpty(name))
        {
            name = GUID.Generate() + ".asset";
        }

        AssetDatabase.CreateAsset(textDialogue, directory + "/" + name);
    }

    public override BaseDialogue GetDialogue()
    {
        return textDialogue;
    }
}

public class OptionDialogueNode : DialogueNode
{
    public OptionDialogue optionDialogue;

    public static OptionDialogueNode Create(OptionDialogue optionDialogue)
    {
        OptionDialogueNode newNode = new OptionDialogueNode() { optionDialogue = optionDialogue };

        Button newOptionButton = new Button(() => { newNode.CreateNewOption(); });
        newOptionButton.text = "Add Option";
        newNode.titleButtonContainer.Add(newOptionButton);
        
        newNode.RenderNode();

        return newNode;
    }

    public void RenderNode()
    {
        title = "Option Dialogue";

        Label actorLabel = new Label("Actor");
        extensionContainer.Add(actorLabel);

        ScrollView actorScrollView = new ScrollView(ScrollViewMode.Horizontal);
        actorScrollView.style.width = 200;
        extensionContainer.Add(actorScrollView);

        TextField actorText = new TextField()
        {
            name = string.Empty,
            value = optionDialogue.actorName,
            multiline = true,
        };
        actorText.style.whiteSpace = WhiteSpace.Normal;
        actorText.RegisterValueChangedCallback(evt => { optionDialogue.actorName = evt.newValue; EditorUtility.SetDirty(optionDialogue); });
        actorScrollView.Add(actorText);

        Label dialogueLabel = new Label("Dialogue");
        extensionContainer.Add(dialogueLabel);

        ScrollView scrollView = new ScrollView(ScrollViewMode.Horizontal);
        scrollView.style.width = 200;
        extensionContainer.Add(scrollView);

        TextField dialogueText = new TextField()
        {
            name = string.Empty,
            value = optionDialogue.text,
            multiline = true
        };
        dialogueText.RegisterValueChangedCallback(evt => { optionDialogue.text = evt.newValue; EditorUtility.SetDirty(optionDialogue); });
        scrollView.Add(dialogueText);

        Port inPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
        inPort.name = "InPort";
        inPort.portName = "   ";
        inputContainer.Add(inPort);

        for (int i = 0; i < optionDialogue.options.Count(); i++)
        {
            CreateNewOption(optionDialogue.options.ElementAt(i));
        }

        RefreshExpandedState();
        RefreshPorts();
    }

    public override void Save(string directory, string name = null)
    {
        if (string.IsNullOrEmpty(name))
        {
            name = GUID.Generate() + ".asset";
        }

        AssetDatabase.CreateAsset(optionDialogue, directory + "/" + name);
    }

    public override BaseDialogue GetDialogue()
    {
        return optionDialogue;
    }

    public void CreateNewOption(string text = "New Option")
    {
        DialogueOption newDialogOption = new DialogueOption() {text = text};
        optionDialogue.options.Add(newDialogOption);
        EditorUtility.SetDirty(optionDialogue);
        
        CreateNewOption(newDialogOption);
    }

    public void CreateNewOption(DialogueOption dialogueOption)
    {
        Port outPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
        outPort.name = dialogueOption.text;
        outPort.portName = "   ";
        outputContainer.Add(outPort);

        TextField option = new TextField()
        {
            name = dialogueOption.text,
            value = dialogueOption.text
        };
        option.RegisterValueChangedCallback(evt => { outPort.name = evt.newValue; dialogueOption.text = evt.newValue; EditorUtility.SetDirty(optionDialogue); });
        outPort.contentContainer.Add(option);

        Button deleteButton = new Button(() => { RemoveOption(outPort, dialogueOption); })
        {
            text = "X"
        };
        outPort.contentContainer.Add(deleteButton);

        RefreshExpandedState();
        RefreshPorts();
    }

    private void RemoveOption(Port port, DialogueOption dialogueOption)
    {
        List<Edge> edges = port.connections.ToList();
        
        for (int i = 0; i < edges.Count; i++)
        {
            port.Disconnect(edges[i]);
            edges[i].input.Disconnect(edges[i]);
            edges[i].RemoveFromHierarchy();
        }

        outputContainer.Remove(port);
        RefreshPorts();
        RefreshExpandedState();

        optionDialogue.options.Remove(dialogueOption);
        EditorUtility.SetDirty(optionDialogue);
    }
}

public class EventDialogueNode : DialogueNode
{
    public EventDialogue eventDialogue;

    public static EventDialogueNode Create(EventDialogue eventDialogue)
    {
        EventDialogueNode newNode = new EventDialogueNode() { eventDialogue = eventDialogue };
        newNode.RenderNode();

        return newNode;
    }

    public void RenderNode()
    {
        title = "Event Dialogue";

        SerializedObject serializedObject = new SerializedObject(eventDialogue);
        VisualElement eventSlot = new IMGUIContainer(() => {
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("onTrigger"));
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(eventDialogue);
            }
        });
        extensionContainer.Add(eventSlot);

        Port inPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
        inPort.name = "InPort";
        inPort.portName = "   ";
        inputContainer.Add(inPort);

        Port outPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
        outPort.name = "OutPort";
        outPort.portName = "   ";
        outputContainer.Add(outPort);

        RefreshExpandedState();
        RefreshPorts();
    }

    public override void Save(string directory, string name = null)
    {
        if (string.IsNullOrEmpty(name))
        {
            name = GUID.Generate() + ".asset";
        }

        AssetDatabase.CreateAsset(eventDialogue, directory + "/" + name);
    }

    public override BaseDialogue GetDialogue()
    {
        return eventDialogue;
    }
}
