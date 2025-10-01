using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UseSeed : MonoBehaviour, IUsable
{
    private string terrainObjectNamePrefix = "Terrain";

    public GameObject plantPrefab;
    public float maxInteractionRange = 4;
    public bool isLargePlantSeed;

    private GameObject redIndicatorPrefab;
    private GameObject greenIndicatorPrefab;
    private GameObject indicator;
    private bool indicatorIsRed;
    private bool indicatorIsGreen;

    void Start()
    {
        redIndicatorPrefab = Resources.Load<GameObject>("Plants/PlantingIndicator/IndicatorError");
        greenIndicatorPrefab = Resources.Load<GameObject>("Plants/PlantingIndicator/IndicatorSuccess");
    }

    void Update()
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);
        bool showIndicator = false;

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxInteractionRange))
        {
            GameObject hitObject = hit.collider.gameObject;
            if (hitObject.name.StartsWith(terrainObjectNamePrefix))
            {
                showIndicator = true;

                Vector3 hitPoint = hit.point;
                bool targetLocationIsValid = ValidatePlantingLocation(hitPoint);
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

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxInteractionRange))
        {
            GameObject hitObject = hit.collider.gameObject;
            if (hitObject.name.StartsWith(terrainObjectNamePrefix))
            {
                Vector3 hitPoint = hit.point;

                bool targetLocationIsValid = ValidatePlantingLocation(hitPoint);
                if (targetLocationIsValid)
                {
                    Instantiate(plantPrefab, hitPoint, Quaternion.identity);
                    Toolbar.instance.DeleteEquippedItem();
                }
            }
        }
    }

    private bool ValidatePlantingLocation(Vector3 hitPoint)
    {
        if (!isLargePlantSeed) return true;

        List<PlantStageGrowth> plantStageGrowths = FindObjectsOfType<PlantStageGrowth>().ToList();
        plantStageGrowths.ForEach(psg => Debug.Log($"{psg.gameObject.name} minDist: {psg.minLargePlantDistance}"));

        return !plantStageGrowths.Any(psg => (psg.transform.position - hitPoint).magnitude < psg.minLargePlantDistance);
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
