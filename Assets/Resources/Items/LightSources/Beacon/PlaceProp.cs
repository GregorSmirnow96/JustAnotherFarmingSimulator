using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlaceProp : MonoBehaviour, IUsable
{
    private string terrainObjectNamePrefix = "Terrain";

    public GameObject placedObjectPrefab;
    public float maxInteractionRange = 6;
    public float objectHeight;
    public float objectRadius;
    public GameObject redIndicatorPrefab;
    public GameObject greenIndicatorPrefab;

    private LayerMask collisionMask;
    private LayerMask placementMask;

    private GameObject indicator;
    private bool indicatorIsRed;
    private bool indicatorIsGreen;

    void Start()
    {
        collisionMask = ~LayerMask.GetMask("Spell", "Terrain");
        placementMask = ~LayerMask.GetMask("Spell");
    }

    void Update()
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);
        bool showIndicator = false;

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxInteractionRange, placementMask))
        {
            GameObject hitObject = hit.collider.gameObject;
            if (hitObject.name.StartsWith(terrainObjectNamePrefix))
            {
                showIndicator = true;

                Vector3 hitPoint = hit.point;
                bool targetLocationIsValid = ValidatePlacementLocation(hitPoint);
                SetIndicator(targetLocationIsValid, hitPoint);
            }
        }

        if (!showIndicator && indicator != null)
        {
            // Not looking at the nearby errain. Delete indicator if it exists and do nothing.
            Destroy(indicator);
            indicatorIsGreen = false;
            indicatorIsRed = false;
        }
    }

    public void Use()
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);

        Debug.Log("HELLO?");
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxInteractionRange))
        {
            GameObject hitObject = hit.collider.gameObject;
            if (hitObject.name.StartsWith(terrainObjectNamePrefix))
            {
                Vector3 hitPoint = hit.point;

                bool targetLocationIsValid = ValidatePlacementLocation(hitPoint);
                if (targetLocationIsValid)
                {
                    Instantiate(placedObjectPrefab, hitPoint, Quaternion.identity);
                    Toolbar.instance.DeleteEquippedItem();
                }
            }
        }
    }

    private bool ValidatePlacementLocation(Vector3 hitPoint)
    {

        Vector3 point1 = hitPoint;
        Vector3 point2 = hitPoint + Vector3.up * objectHeight;

        Collider[] hits = Physics.OverlapCapsule(point1, point2, objectRadius, collisionMask);

        return hits.Length == 0;
    }

    private void SetIndicator(bool targetLocationIsValid, Vector3 targetLocation)
    {
        GameObject prefabToSpawn = targetLocationIsValid ? greenIndicatorPrefab : redIndicatorPrefab;
        if (indicator == null)
        {
            // Indicator does not exists, so spawn it.
            indicator = Instantiate(prefabToSpawn, targetLocation, Quaternion.identity);
        }
        else
        {
            // Indicator already exists, so recreate it if the color changed, move it if it didn't.
            if (targetLocationIsValid && indicatorIsRed || !targetLocationIsValid && indicatorIsGreen)
            {
                // Delete the red indicator, spawn a green one.
                Destroy(indicator);
                indicator = Instantiate(prefabToSpawn, targetLocation, Quaternion.identity);
            }
            else
            {
                // The color did not change. Update the position.
                indicator.transform.position = targetLocation;
            }
        }

        indicatorIsGreen = targetLocationIsValid;
        indicatorIsRed = !targetLocationIsValid;
    }

    void OnDestroy()
    {
        Destroy(indicator);
    }
}
