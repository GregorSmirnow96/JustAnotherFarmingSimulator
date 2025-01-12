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
        "Assets/Malbers Animations/Animals Packs/01 Forest Pack/Rabbit/Rabbit.prefab",
        new List<string>() { "Carrots" }) { }
}

public class RaccoonSpawnData : AnimalSpawnData
{
    public RaccoonSpawnData() : base(
        "Raccoon",
        18f,
        6f,
        "Assets/Malbers Animations/Animals Packs/01 Forest Pack/Raccoon/Raccoon.prefab",
        new List<string>() { "_Carrots" }) { }
}

public class BoarSpawnData : AnimalSpawnData
{
    public BoarSpawnData() : base(
        "Boar",
        18f,
        6f,
        "Assets/Malbers Animations/Animals Packs/01 Forest Pack/Boar/Boar.prefab",
        new List<string>() { "_Carrots" }) { }
}

public class FoxSpawnData : AnimalSpawnData
{
    public FoxSpawnData() : base(
        "Fox",
        18f,
        6f,
        "Assets/Malbers Animations/Animals Packs/01 Forest Pack/Fox/Fox.prefab",
        new List<string>() { "_BlueBerryBush" }) { }
}

public class DeerSpawnData : AnimalSpawnData
{
    public DeerSpawnData() : base(
        "Deer",
        18f,
        6f,
        "Assets/Malbers Animations/Animals Packs/01 Forest Pack/Deer/Deer.prefab",
        new List<string>() { "_Carrots" }) { }
}

public class CougarSpawnData : AnimalSpawnData
{
    public CougarSpawnData() : base(
        "Cougar",
        18f,
        6f,
        "Assets/Malbers Animations/Animals Packs/01 Forest Pack/Cougar/Leopard.prefab",
        new List<string>() { "_Wheat" }) { }
}

public class WolfSpawnData : AnimalSpawnData
{
    public WolfSpawnData() : base(
        "Wolf",
        18f,
        6f,
        "Assets/Malbers Animations/Animals Packs/01 Forest Pack/Wolf/Wolf.prefab",
        new List<string>() { "Wheat" }) { }
}

public class BearSpawnData : AnimalSpawnData
{
    public BearSpawnData() : base(
        "Bear",
        18f,
        6f,
        "Assets/Malbers Animations/Animals Packs/01 Forest Pack/Bear/Bear.prefab",
        new List<string>() { "_BlueBerryBush" }) { }
}

public class MooseSpawnData : AnimalSpawnData
{
    public MooseSpawnData() : base(
        "Moose",
        18f,
        6f,
        "Assets/Malbers Animations/Animals Packs/01 Forest Pack/Moose/Moose.prefab",
        new List<string>() { "_BlueBerryBush" }) { }
}

public class TigerSpawnData : AnimalSpawnData
{
    public TigerSpawnData() : base(
        "Tiger",
        18f,
        6f,
        "Assets/Malbers Animations/Animals Packs/01 Forest Pack/Tiger/Tiger.prefab",
        new List<string>() { "BlueBerryBush" }) { }
}
