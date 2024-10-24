using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int health;
    public int initialHealth;

    public List<Action> callbacks = new List<Action>();

    public void TakeDamage(int damage)
    {
        health -= damage;
        callbacks.ForEach(callback => callback());
        if (health <= 0) Destroy(gameObject);
    }

    public void RegisterCallback(Action callback)
    {
        callbacks.Add(callback);
    }
}
