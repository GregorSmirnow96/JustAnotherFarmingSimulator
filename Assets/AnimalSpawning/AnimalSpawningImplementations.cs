using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ItemMetaData;

public class RabbitSpawnData : AnimalSpawnData
{
    public RabbitSpawnData() : base(
        "Rabbit",
        18f,
        6f,
        "Animals/Rabbit",
        new List<string>() { "Carrot" }) { }
}
