using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShootBow : MonoBehaviour, IUsable
{
    private Animator animator;
    private PlayerStats stats;
    private bool arrowExists;
    private GameObject spawnedArrow;

    void Start()
    {
        animator = GetComponent<Animator>();
        stats = SceneProperties.playerTransform.GetComponent<PlayerStats>();
    }

    public void Use()
    {
        Toolbar.instance.SetScrollingEnabled(false);
        animator.SetTrigger("Draw");
        arrowExists = false;
        StartCoroutine(HandleDraw());
    }

    private IEnumerator HandleDraw()
    {
         const float drawTime = 1.3f;

        float drawTimer = 0f;
        while (Input.GetMouseButton(0)) // Left mouse is held down
        {
            drawTimer += Time.deltaTime;
            if (drawTimer >= drawTime && !arrowExists)
            {
                // Get arrow from equipped arrows. Equipment isn't implemented yet, so get an arrow from the inventory.
                Inventory inventory = Inventory.instance;

                Item arrowItem = inventory.quiver;
                if (arrowItem?.type is ArrowItemType)
                {
                    arrowItem.stackSize--;
                    if (arrowItem.stackSize == 0)
                    {
                        inventory.quiver = null;
                    }
                    GameObject ironArrowPrefab = arrowItem.type.groundItemPrefab;
                    spawnedArrow = Instantiate(ironArrowPrefab, transform.Find("NotchedArrowPosition"));
                }

                arrowExists = true;
            }

            yield return null;
        }

        if (drawTimer >= drawTime && spawnedArrow != null)
        {
            spawnedArrow.transform.parent = null;
            // The arrow is now spawned. Implement a FireArrow script to control it's motion :)
            Arrow arrowScript = spawnedArrow.GetComponent<Arrow>();
            arrowScript.Fire();
        }

        spawnedArrow = null;

        animator.SetTrigger("Release");
    }
}
