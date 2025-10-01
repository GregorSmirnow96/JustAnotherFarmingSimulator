using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ThrowBomb : MonoBehaviour, IUsable
{
    public GameObject inFlightBombPrefab;

    private bool hasBeenThrown;

    public void Use()
    {
        if (!hasBeenThrown)
        {
            Throw();
            hasBeenThrown = true;
        }
    }

    private void Throw()
    {
        Instantiate(inFlightBombPrefab, transform.position, transform.rotation);
        Toolbar.instance.DeleteEquippedItem();
        Destroy(gameObject);
    }
}
