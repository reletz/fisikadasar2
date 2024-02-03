using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using System.Linq;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.IO;
using UnityEditor.SceneManagement;

public class DialogueGraphView : GraphView
{
    private bool disableChecking;
    private StartingNode startingNode;
    public DialogueGraph currentDialogueGraph;
    public string currentDialoguesFolder;

    public DialogueGraphView()
    {
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        GridBackground gridBackground = new GridBackground();
        Insert(0, gridBackground);
        gridBackground.StretchToParentSize();

        graphViewChanged = OnGraphChange;
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        List<Port> compatiblePorts = new List<Port>();

        for (int i = 0; i < ports.Count(); i++)
        {
            if (ports.ElementAt(i).node != startPort.node && ports.ElementAt(i) != startPort && ports.ElementAt(i).direction != startPort.direction)
            {
                compatiblePorts.Add(ports.ElementAt(i));
            }
        }

        return compatiblePorts;
    }

    public void CreateNewTextDialogue()
    {
        if (currentDialogueGraph == null) return;

        TextDialogue newTextDialogue = ScriptableObject.CreateInstance<TextDialogue>();
        AssetDatabase.CreateAsset(newTextDialogue, currentDialoguesFolder + "\\" + GUID.Generate() + ".asset");

        TextDialogueNode newTextDialogueNode = TextDialogueNode.Create(newTextDialogue);
        currentDialogueGraph.dialogues.Add(new(newTextDialogue, newTextDialogueNode.GetPosition()));
        EditorUtility.SetDirty(currentDialogueGraph);

        AddElement(newTextDialogueNode);
    }

    public void CreateNewOptionDialogue()
    {
        if (currentDialogueGraph == null) return;

        OptionDialogue newOptionDialogue = ScriptableObject.CreateInstance<OptionDialogue>();
        AssetDatabase.CreateAsset(newOptionDialogue, currentDialoguesFolder + "\\" + GUID.Generate() + ".asset");

        OptionDialogueNode newOptionDialogueNode = OptionDialogueNode.Create(newOptionDialogue);
        currentDialogueGraph.dialogues.Add(new(newOptionDialogue, newOptionDialogueNode.GetPosition()));
        EditorUtility.SetDirty(currentDialogueGraph);

        AddElement(newOptionDialogueNode);
    }
    public void CreateNewEventDialogue()
    {
        if (currentDialogueGraph == null) return;

        EventDialogue newEventDialogue = ScriptableObject.CreateInstance<EventDialogue>();
        AssetDatabase.CreateAsset(newEventDialogue, currentDialoguesFolder + "\\" + GUID.Generate() + ".asset");

        EventDialogueNode newEventDialogueNode = EventDialogueNode.Create(newEventDialogue);
        currentDialogueGraph.dialogues.Add(new(newEventDialogue, newEventDialogueNode.GetPosition()));
        EditorUtility.SetDirty(currentDialogueGraph);

        AddElement(newEventDialogueNode);
    }

    public void ClearGraph()
    {
        disableChecking = true;

        edges.ForEach(edge => {
            edge.input.Disconnect(edge);
            edge.output.Disconnect(edge);
            RemoveElement(edge);
        });

        nodes.ForEach(node => {
            if (node == startingNode) return;
            RemoveElement(node);
        });

        disableChecking = false;
    }

    private Node GenerateStartingNode()
    {
        startingNode = StartingNode.Create();
        currentDialogueGraph.startingPosition = startingNode.GetPosition();

        return startingNode;
    }

    private GraphViewChange OnGraphChange(GraphViewChange change)
    {
        if (disableChecking) return change;

        if (change.edgesToCreate != null)
        {
            change.edgesToCreate.ForEach(edge => {
                if (edge.output.node == startingNode)
                {
                    currentDialogueGraph.startingPoint = ((DialogueNode) edge.input.node).GetDialogue();
                    EditorUtility.SetDirty(currentDialogueGraph);
                }
                else if (edge.output.node is TextDialogueNode textNode)
                {
                    textNode.textDialogue.nextDialogue = ((DialogueNode) edge.input.node).GetDialogue();
                    EditorUtility.SetDirty(textNode.textDialogue);
                }
                else if (edge.output.node is OptionDialogueNode optionNode)
                {
                    optionNode.optionDialogue.options.Find(option => { return option.text == edge.output.name; }).nextDialogue = ((DialogueNode) edge.input.node).GetDialogue();
                    EditorUtility.SetDirty(optionNode.optionDialogue);
                }
                else if (edge.output.node is EventDialogueNode eventNode)
                {
                    eventNode.eventDialogue.nextDialogue = ((DialogueNode) edge.input.node).GetDialogue();
                    EditorUtility.SetDirty(eventNode.eventDialogue);
                }
            });
        }

        if (change.elementsToRemove != null)
        {
            change.elementsToRemove.ForEach(element => {
                if (element.GetType() == typeof(Edge))
                {
                    Edge edge = (Edge) element;

                    if (edge.output.node == startingNode)
                    {
                        currentDialogueGraph.startingPoint = null;
                        EditorUtility.SetDirty(currentDialogueGraph);
                    }
                    else if (edge.output.node is TextDialogueNode textNode)
                    {
                        textNode.textDialogue.nextDialogue = null;
                        EditorUtility.SetDirty(textNode.textDialogue);
                    }
                    else if (edge.output.node is OptionDialogueNode optionNode)
                    {
                        optionNode.optionDialogue.options.Find(option => { return option.text == edge.output.name; }).nextDialogue = null;
                        EditorUtility.SetDirty(optionNode.optionDialogue);
                    }
                    else if (edge.output.node is EventDialogueNode eventNode)
                    {
                        eventNode.eventDialogue.nextDialogue = null;
                        EditorUtility.SetDirty(eventNode.eventDialogue);
                    }
                }
                else if (element is DialogueNode)
                {
                    DialogueNode node = (DialogueNode) element;

                    int index = currentDialogueGraph.dialogues.FindIndex(pair => { return pair.dialogue == node.GetDialogue(); });
                    currentDialogueGraph.dialogues.RemoveAt(index);
                    EditorUtility.SetDirty(currentDialogueGraph);

                    string assetPath = AssetDatabase.GetAssetPath(node.GetDialogue());
                    AssetDatabase.DeleteAsset(assetPath);
                }
            });
        }

        if (change.movedElements != null)
        {
            change.movedElements.ForEach(element => {
                if (!(element is DialogueNode)) return;
                if (element == startingNode)
                {
                    currentDialogueGraph.startingPosition = startingNode.GetPosition();
                    EditorUtility.SetDirty(currentDialogueGraph);
                    return;
                }

                DialogueNode node = (DialogueNode) element;

                int index = currentDialogueGraph.dialogues.FindIndex(pair => { return pair.dialogue == node.GetDialogue(); });
                currentDialogueGraph.dialogues[index] = new(node.GetDialogue(), node.GetPosition());
                EditorUtility.SetDirty(currentDialogueGraph);
            });
        }

        return change;
    }

    public void GenerateGraph()
    {
        disableChecking = true;

        if (startingNode == null)
        {
            AddElement(GenerateStartingNode());
        }

        List<(BaseDialogue dialogue, DialogueNode node)> generated = new List<(BaseDialogue dialogue, DialogueNode node)>();

        currentDialogueGraph.dialogues.ForEach( dialogueNode => {
            BaseDialogue dialogue = dialogueNode.dialogue;
            Rect position = dialogueNode.nodePosition;

            if (dialogue is TextDialogue)
            {
                TextDialogue textDialogue = (TextDialogue) dialogue;

                TextDialogueNode newNode = TextDialogueNode.Create(textDialogue);
                newNode.SetPosition(position);

                generated.Add((textDialogue, newNode));
                AddElement(newNode);
            }
            else if (dialogue is OptionDialogue)
            {
                OptionDialogue optionDialogue = (OptionDialogue) dialogue;

                OptionDialogueNode newNode = OptionDialogueNode.Create(optionDialogue);
                newNode.SetPosition(position);

                generated.Add((optionDialogue, newNode));
                AddElement(newNode);
            }
            else if (dialogue is EventDialogue)
            {
                EventDialogue eventDialogue = (EventDialogue) dialogue;

                EventDialogueNode newNode = EventDialogueNode.Create(eventDialogue);
                newNode.SetPosition(position);

                generated.Add((eventDialogue, newNode));
                AddElement(newNode);
            }
        });

        generated.ForEach( x => {
            if (x.dialogue is TextDialogue)
            {
                TextDialogue textDialogue = (TextDialogue) x.dialogue;
                TextDialogueNode node = (TextDialogueNode) x.node;

                if (textDialogue.nextDialogue == null) return;

                Port outPort = node.outputContainer.Q<Port>("OutPort");
                Port inPort = generated.Find(x => { return x.dialogue == textDialogue.nextDialogue; }).node.inputContainer.Q<Port>("InPort");

                Edge newEdge = outPort.ConnectTo(inPort);
                AddElement(newEdge);
            }
            else if (x.dialogue is OptionDialogue)
            {
                OptionDialogue optionDialogue = (OptionDialogue) x.dialogue;
                OptionDialogueNode node = (OptionDialogueNode) x.node;

                for (int i = 0; i < optionDialogue.options.Count(); i++)
                {
                    if (optionDialogue.options[i].nextDialogue == null) continue;

                    Port outPort = node.outputContainer.Q<Port>(optionDialogue.options[i].text);
                    Port inPort = generated.Find(x => { return x.dialogue == optionDialogue.options[i].nextDialogue; }).node.inputContainer.Q<Port>("InPort");

                    Edge newEdge = outPort.ConnectTo(inPort);
                    AddElement(newEdge);
                }
            }
            else if (x.dialogue is EventDialogue)
            {
                EventDialogue eventDialogue = (EventDialogue) x.dialogue;
                EventDialogueNode node = (EventDialogueNode) x.node;

                if (eventDialogue.nextDialogue == null) return;

                Port outPort = node.outputContainer.Q<Port>("OutPort");
                Port inPort = generated.Find(x => { return x.dialogue == eventDialogue.nextDialogue; }).node.inputContainer.Q<Port>("InPort");

                Edge newEdge = outPort.ConnectTo(inPort);
                AddElement(newEdge);
            }
        });

        if (currentDialogueGraph.startingPoint != null)
        {
            Port outPort = startingNode.outputContainer.Q<Port>("OutPort");
            Port inPort = generated.Find(x => { return x.dialogue == currentDialogueGraph.startingPoint; }).node.inputContainer.Q<Port>("InPort");

            Edge newEdge = outPort.ConnectTo(inPort);
            AddElement(newEdge);
        }

        startingNode.SetPosition(currentDialogueGraph.startingPosition);

        disableChecking = false;
    }
}

public class DialogueGraphEditorWindow : EditorWindow
{
    private DialogueGraphView graphView;
    private Toolbar toolbar;
    private Label fileInfoLabel;

    [MenuItem("Graphs/Dialogue", validate = false)]
    public static void ShowDialogueGraphEditor()
    {
        DialogueGraphEditorWindow window = GetWindow<DialogueGraphEditorWindow>();
        window.titleContent = new GUIContent("Dialogue Editor");
    }

    private void OnEnable()
    {
        ConstructGraphView();
        rootVisualElement.Add(graphView);

        ConstructToolbar();
        rootVisualElement.Add(toolbar);
    }

    private void OnDisable()
    {
        rootVisualElement.Remove(graphView);
        rootVisualElement.Remove(toolbar);
    }

    private void ConstructToolbar()
    {
        toolbar = new Toolbar();

        Button textDialogueCreateButton = new Button(() => { graphView.CreateNewTextDialogue(); });
        textDialogueCreateButton.text = "Create Text Dialogue";
        toolbar.Add(textDialogueCreateButton);

        Button optionDialogueCreateButton = new Button(() => { graphView.CreateNewOptionDialogue(); });
        optionDialogueCreateButton.text = "Create Option Dialogue";
        toolbar.Add(optionDialogueCreateButton);

        Button eventDialogueCreateButton = new Button(() => { graphView.CreateNewEventDialogue(); });
        eventDialogueCreateButton.text = "Create Event Dialogue";
        toolbar.Add(eventDialogueCreateButton);

        Button loadDataButton = new Button(() => { LoadGraph(); });
        loadDataButton.text = "Open";
        toolbar.Add(loadDataButton);

        fileInfoLabel = new Label("No file opened");
        fileInfoLabel.style.unityTextAlign = TextAnchor.MiddleRight;
        fileInfoLabel.style.flexGrow = 1;
        fileInfoLabel.style.right = 0;
        fileInfoLabel.style.paddingRight = 10;
        toolbar.Add(fileInfoLabel);
    }

    private void ConstructGraphView()
    {
        graphView = new DialogueGraphView()
        {
            name = "Dialogue Graph"
        };

        graphView.StretchToParentSize();
    }

    private void LoadGraph()
    {
        string graphPath = EditorUtility.SaveFilePanel("Open Dialogue Graph", "Assets", "DialogueGraph", "asset");

        if (string.IsNullOrEmpty(graphPath)) return;

        fileInfoLabel.text = Path.GetFileName(graphPath);

        string relativeGraphPath = "Assets" + graphPath.Substring(Application.dataPath.Length);

        if (!File.Exists(graphPath))
        {
            DialogueGraph newDialogueGraph = ScriptableObject.CreateInstance<DialogueGraph>();
            AssetDatabase.CreateAsset(newDialogueGraph, relativeGraphPath);

            while (string.IsNullOrEmpty(newDialogueGraph.savePath))
            {
                string rawPath = EditorUtility.OpenFolderPanel("Select a folder to save dialogue assets", "Assets", "Dialogues");
                newDialogueGraph.savePath = "Assets" + rawPath.Substring(Application.dataPath.Length);
            }
        }

        graphView.currentDialogueGraph = (DialogueGraph) AssetDatabase.LoadAssetAtPath(relativeGraphPath, typeof(DialogueGraph));
        graphView.currentDialoguesFolder = graphView.currentDialogueGraph.savePath;

        graphView.ClearGraph();
        graphView.GenerateGraph();
    }
}
