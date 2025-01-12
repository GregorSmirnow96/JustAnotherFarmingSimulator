using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemQuantityText : MonoBehaviour
{
    public GameObject quantityTextPrefab;
    public bool quantityTextIsGranchild = true;

    private int quantity;
    private GameObject textContainer;
    private TMP_Text text;

    void Start()
    {
        quantity = 1;

        Transform parentTransform = quantityTextIsGranchild ? transform.GetChild(0) : transform;
        textContainer = Instantiate(quantityTextPrefab, parentTransform);

        text = textContainer.GetComponent<TMP_Text>();
    }

    void Update()
    {
        text.text = quantity.ToString();
    }

    public void SetQuantity(int newQuantity)
    {
        quantity = newQuantity;
    }

    public void SetActive(bool isActive)
    {
        textContainer.SetActive(isActive);
    }
}
