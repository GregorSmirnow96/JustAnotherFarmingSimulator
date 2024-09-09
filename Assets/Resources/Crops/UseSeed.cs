using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseSeed : MonoBehaviour, IUsable
{
    public GameObject plantPrefab;
    public float maxInteractionRange = 4;

    public void Use()
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxInteractionRange))
        {
            Vector3 hitPoint = hit.point;
            Instantiate(plantPrefab, hitPoint, Quaternion.identity);
            Toolbar.instance.DeleteEquippedItem();
        }
    }
}
