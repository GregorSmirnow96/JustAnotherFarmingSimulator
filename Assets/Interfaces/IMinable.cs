using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMinable
{
    public void Mine(float damage, int pickaxeTier);
    public bool FullyDegraded();
    public void Degrade();
    public void Regenerate();
    public GameObject GetCollisionParticleSystem();
}
