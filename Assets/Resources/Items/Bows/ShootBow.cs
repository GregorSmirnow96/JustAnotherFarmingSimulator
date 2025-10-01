using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShootBow : MonoBehaviour, IUsable
{
    public Animator containerAnimator;
    public Animator bowAnimator;
    public GameObject notchedArrowPosition;
    public float shootCooldown = 0.91f;
    public float damageMulti = 1f;

    private PlayerStats stats;
    private bool arrowExists;
    private GameObject spawnedArrow;
    private float lastShootTime;
    
    void Start()
    {
        stats = SceneProperties.playerTransform.GetComponent<PlayerStats>();
    }

    public void Use()
    {
        if (Time.time >= lastShootTime + shootCooldown)
        {
            Toolbar.instance.SetScrollingEnabled(false);
            bowAnimator.SetTrigger("Draw");
            containerAnimator.SetTrigger("Aim");
            arrowExists = false;
            StartCoroutine(HandleDraw());
        }
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
                    GameObject arrowPrefab = arrowItem.type.groundItemPrefab;
                    spawnedArrow = Instantiate(arrowPrefab, notchedArrowPosition.transform);
                    GroundItem groundItemScript = spawnedArrow.GetComponent<GroundItem>();
                    Destroy(groundItemScript);
                }

                arrowExists = true;
            }

            yield return null;
        }

        if (drawTimer >= drawTime && spawnedArrow != null)
        {
            spawnedArrow.transform.parent = null;
            // The arrow is now spawned. Implement a FireArrow script to control it's motion :)
            IArrow arrowScript = spawnedArrow.GetComponent<IArrow>();
            arrowScript.ScaleDamage(damageMulti);
            arrowScript.Fire();
        }

        spawnedArrow = null;

        bowAnimator.SetTrigger("Release");

        lastShootTime = Time.time;

        yield return new WaitForSeconds(0.12f);
        containerAnimator.SetTrigger("Unaim");
    }
}
