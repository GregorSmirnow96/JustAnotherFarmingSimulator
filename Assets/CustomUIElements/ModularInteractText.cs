using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModularInteractText : MonoBehaviour
{
    public Vector3 offset = new Vector3(0, 0.42f, 0);
    public GameObject textPrefab;
    public bool showInCenterOfScreen = false;

    public GameObject textObject;

    private Canvas rootCanvas;
    private TMP_Text floatingText;
    private RectTransform textRectTransform;
    private bool enableTextAfterUpdatingPosition;

    void Awake()
    {
        rootCanvas = GameObject.Find("UICanvas").GetComponent<Canvas>();

        Debug.Log(gameObject.name);
        textObject = Instantiate(textPrefab, rootCanvas.transform);
        floatingText = textObject.GetComponent<TMP_Text>();

        textRectTransform = textObject.GetComponent<RectTransform>();

        Disable();
    }

    public void SetText(string newText)
    {
        floatingText.text = newText;
    }

    void Update()
    {
        Vector3 screenPosition;

        if (showInCenterOfScreen)
        {
            int x = (Screen.width / 2);
            int y = (Screen.height / 2);
            screenPosition = new Vector3(x + offset.x, y + offset.y, 0);
        }
        else
        {
            Vector3 worldPosition = transform.position + offset;
            screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
        }

        textRectTransform.position = screenPosition;

        floatingText.enabled = enableTextAfterUpdatingPosition;
    }

    public void Enable()
    {
        enableTextAfterUpdatingPosition = true;
    }

    public void Disable()
    {
        enableTextAfterUpdatingPosition = false;
    }

    void OnDestroy()
    {
        Destroy(textObject);
    }
}
