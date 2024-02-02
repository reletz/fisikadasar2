using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textField;
    [SerializeField] public Button button;

    public void ChangeLabel(string text)
    {
        textField.SetText(text);
    }
}
