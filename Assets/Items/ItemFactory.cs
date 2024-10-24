using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ItemMetaData;

public class ItemFactory
{
    private static ItemFactory instance;

    public static ItemFactory GetInstance() =>
        instance == null
            ? instance = new ItemFactory()
            : instance;

    private ItemTypeRepo itemTypeRepo;

    private ItemFactory()
    {
        itemTypeRepo = ItemTypeRepo.GetInstance();
    }

    private void CreateItem()
    {
        ;
    }
}
