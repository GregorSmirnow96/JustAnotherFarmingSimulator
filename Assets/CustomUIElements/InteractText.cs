using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractText : MonoBehaviour
{
    public Vector3 offset = new Vector3(0, 0, 0);
    public GameObject textPrefab;
    public string interactionText = "'E' to interact";

    private Canvas rootCanvas;
    private Text floatingText;
    private RectTransform textRectTransform;
    private bool enabled;

    void Start()
    {
        rootCanvas = GameObject.Find("UICanvas").GetComponent<Canvas>();

        GameObject textObject = Instantiate(textPrefab, rootCanvas.transform);
        floatingText = textObject.GetComponent<Text>();

        textRectTransform = textObject.GetComponent<RectTransform>();

        floatingText.text = interactionText;

        //Disable();
    }

/*
    void Update()
    {
        Vector3 worldPosition = transform.position + offset;
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

        textRectTransform.position = screenPosition;
    }

    public void Enable()
    {
        floatingText.gameObject.SetActive(true);
        enabled = true;
    }

    public void Disable()
    {
        floatingText.gameObject.SetActive(false);
        enabled = false;
    }
*/
}
