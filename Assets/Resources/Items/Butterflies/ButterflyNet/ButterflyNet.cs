using System;
using System.Collections;
using UnityEngine;

public class ButterflyNet : MonoBehaviour, IUsable
{
    public Animator animator;
    public float swingCooldown = 1f;
    public Collider netCollider;

    private float nextSwingTime = 0f;
    private bool isSwinging = false;
    private Inventory inventory;    

    float previousSwingTime = 0f;

    private void Start()
    {
        inventory = Inventory.instance;
    }

    public void Use()
    {
        if (Time.time >= nextSwingTime && !isSwinging)
        {
            StartCoroutine(SwingNet());
        }
    }

    private IEnumerator SwingNet()
    {
        isSwinging = true;

        animator.SetTrigger("Swing");
        nextSwingTime = Time.time + swingCooldown;

        float swingDuration = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(0.1f);
        netCollider.enabled = true;
        yield return new WaitForSeconds(swingDuration - 0.1f);

        isSwinging = false;
        netCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider otherCollider)
    {
        Debug.Log(1);
        GameObject collidedObject = otherCollider.gameObject;

        ButterflyGroup butterflyScript = collidedObject.GetComponent<ButterflyGroup>();
        if (butterflyScript != null)
        {
            Debug.Log(2);
            bool hasInventorySpace = !inventory.IsFull();
            if (hasInventorySpace)
            {
                Debug.Log(3);
                Item butterflyWingItem = new Item(butterflyScript.wingItemName);
                inventory.AddItemToInventory(butterflyWingItem);

                Destroy(collidedObject);
            }
        }
    }
}