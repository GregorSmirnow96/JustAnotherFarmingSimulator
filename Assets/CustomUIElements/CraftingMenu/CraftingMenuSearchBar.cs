using UnityEngine;
using TMPro;

public class ResetInputField : MonoBehaviour
{
    public TMP_InputField inputField;
    public string defaultValue = "Search...";

    void Start()
    {
        inputField = GetComponent<TMP_InputField>();
        inputField.onEndEdit.AddListener(OnInputFieldEndEdit);
    }

    void OnInputFieldEndEdit(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            inputField.text = defaultValue;
        }
    }
}
