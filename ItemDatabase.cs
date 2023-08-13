using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    void Start()
    {
        items.Add(new Item("Corpse Herb", "Reagent", "Flora that grows on the ground atop buried bodies. It's said that the dissapation of Fey from the dead makes the surroundings fertile, allowing the Corpse Herb to grow.", 1));
        items.Add(new Item("Lumber", "Reagent", "Tree...", 1));
    }
}

