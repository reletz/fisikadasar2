using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DialogueReader : MonoBehaviour
{
    public static DialogueReader instance;

    public BaseDialogue currentDialogue;
    public float typeSpeed;
    public float autoWaitTime;
    public bool isTyping;
    public bool isHidden;
    public bool isReceivingInput;
    public bool isSkipping;
    public bool isAuto;

    public UnityEvent onLeftMouseButtonDown;
    public UnityEvent onRightMouseButtonDown;
    public UnityEvent<int> onOptionChosen;

    [SerializeField] private TextMeshProUGUI nameField;
    [SerializeField] private TextMeshProUGUI dialogueField;
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private GameObject optionsParent;
    [SerializeField] private Option optionPrefab;

    private string requestedText = "TestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTest";
    private string text = "";
    private List<Option> optionObjects = new List<Option>();
    private float timer;

    public void Initialize()
    {
        if (instance != null && instance != this) return;

        instance = this;
    }

    public void RequestText(string request)
    {
        requestedText = request;
        isTyping = true;
    }

    public void ChangeActorName(string newName)
    {
        nameField.SetText(newName);
    }

    public void Hide()
    {
        isHidden = true;
        HideDialogueBox();
        HideOptions();
    }

    public void Show()
    {
        isHidden = false;
        ShowDialogueBox();
        ShowOptions();
    }

    public void ShowName()
    {
        nameField.color = new Color(255, 255, 255, 1);
        dialogueField.rectTransform.anchorMax = new Vector2(dialogueField.rectTransform.anchorMax.x, 0.75f);
    }

    public void HideName()
    {
        nameField.color = new Color(255, 255, 255, 0);
        dialogueField.rectTransform.anchorMax = new Vector2(dialogueField.rectTransform.anchorMax.x, 1);
    }

    public void HideDialogueBox()
    {
        dialogueBox.SetActive(false);
    }

    public void ShowDialogueBox()
    {
        dialogueBox.SetActive(true);
    }

    public void LoadOptions(IEnumerable<string> options)
    {
        IEnumerator<string> enumerator = options.GetEnumerator();

        while (enumerator.MoveNext())
        {
            Option newOption = Instantiate(optionPrefab, optionsParent.transform);
            newOption.ChangeLabel(enumerator.Current);
            int index = optionObjects.Count;
            newOption.button.onClick.AddListener(() => { onOptionChosen.Invoke(index); });
            optionObjects.Add(newOption);
        }
    }

    public void UnloadOptions()
    {
        optionObjects.ForEach(option => {
            Destroy(option.gameObject);
        });

        optionObjects = new List<Option>();
    }

    public void ShowOptions()
    {
        optionsParent.SetActive(true);
    }

    public void HideOptions()
    {
        optionsParent.SetActive(false);
    }

    private void TypeNext()
    {
        isTyping = true;

        if (!requestedText.StartsWith(text))
        {
            text = "";
        }

        if (requestedText.Length > text.Length)
        {
            text += requestedText[text.Length];
        }
        else
        {
            timer = 0;
            isTyping = false;
        }
    }

    public void SkipToggle()
    {
        isSkipping = !isSkipping;
    }

    public void AutoToggle()
    {
        isAuto = !isAuto;
    }

    public void DisplayText()
    {
        dialogueField.SetText(text);
    }

    public void ClickCallback()
    {
        if (isReceivingInput)
        {
            if (isHidden)
            {
                Show();
                return;
            }

            if (isTyping)
            {
                isTyping = false;
                text = requestedText;
                DisplayText();
                return;
            }

            onLeftMouseButtonDown.Invoke();
        }
    }

    void Awake()
    {
        Initialize();
        currentDialogue.Enter();
    }

    void Update()
    {
        if (isSkipping)
        {
            isTyping = false;
            text = requestedText;
            DisplayText();
            onLeftMouseButtonDown.Invoke();
            return;
        }

        if (isTyping)
        {
            timer += Time.deltaTime;

            if (timer > 60/typeSpeed)
            {
                timer = timer - 60/typeSpeed;
                TypeNext();
            }

            DisplayText();
        }

        if (isAuto && (text == requestedText))
        {
            timer += Time.deltaTime;

            if (timer > autoWaitTime)
            {
                onLeftMouseButtonDown.Invoke();
                timer = 0;
            }
        }
    }
}
