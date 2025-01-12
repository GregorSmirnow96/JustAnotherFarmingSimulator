using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundItem : MonoBehaviour
{
    private static string groundItemTag = "GroundItem";
    private static string lootBeamPrefabPath = "LootBeam";

    public string itemId;
    public bool hasLootBeam = true;

    public float setTagDelay = 0f;

    public Item item { get; set; }

    void Start()
    {
        item = new Item(itemId);
        StartCoroutine(SetTag());

        if (hasLootBeam)
        {
            GameObject lootBeamPrefab = Resources.Load<GameObject>(lootBeamPrefabPath);
            Instantiate(lootBeamPrefab, transform);
        }
    }

    private IEnumerator SetTag()
    {
        yield return new WaitForSeconds(setTagDelay);
        gameObject.tag = groundItemTag;
    }
}
