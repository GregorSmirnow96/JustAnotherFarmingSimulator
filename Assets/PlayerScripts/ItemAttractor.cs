using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAttractor : MonoBehaviour
{
    private readonly string groundItemTag = "GroundItem";

    public float attractRange = 3.0f;
    public float pickupRange = 0.4f;
    public float minAttractSpeed = 2.0f;
    public float maxAttractSpeed = 7.0f;
    private Toolbar playerToolbar;

    void Start()
    {
        playerToolbar = gameObject.GetComponent<Toolbar>();
    }

    void Update()
    {
        if (playerToolbar.IsFull)
        {
            return;
        }

        GameObject[] groundItems = GameObject.FindGameObjectsWithTag(groundItemTag);
        foreach (GameObject groundItem in groundItems)
        {
            Vector3 nonVerticalPlayerPosition = new Vector3(gameObject.transform.position.x, 0.0f, gameObject.transform.position.z);
            Vector3 nonVerticalItemPosition = new Vector3(groundItem.transform.position.x, 0.0f, groundItem.transform.position.z);
            Vector3 vectorToPlayer = nonVerticalPlayerPosition - nonVerticalItemPosition;
            float distanceToPlayer = vectorToPlayer.magnitude;
            if (distanceToPlayer <= pickupRange)
            {
                string groundItemId = groundItem.GetComponent<GroundItem>().itemId;
                bool succeeded = playerToolbar.AddItem(groundItemId);
                if (succeeded)
                {
                    Destroy(groundItem);
                }
            }
            else if (distanceToPlayer <= attractRange)
            {
                float attractionStrength = (attractRange - distanceToPlayer) / attractRange;
                float attractionSpeed = (maxAttractSpeed - minAttractSpeed) * attractionStrength + minAttractSpeed;
                float timeScalar = Math.Min(Time.deltaTime, 1.0f);
                Vector3 movementVector = vectorToPlayer.normalized * attractionSpeed * timeScalar;
                groundItem.transform.position += movementVector;
            }
        }
    }
}
