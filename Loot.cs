/**
 * Custom Loot class helper for LootTable.cs 
 * 
 * Written by Tony Recchia (@then00b)
 * 
 * Based on LootTable.js by John Watson (Copyright © 2015)
 * https://github.com/jotson/LootTable.js/blob/master/LootTable.js
 * 
 * Licensed under the terms of the MIT License
 * ---
 * 
 * Feel free to customize this class to add more fields if desired. LootTable.cs 
 * only requires name, weight, and quanity.
 * 
 */

using UnityEngine;
using System.Collections;

public class Loot : MonoBehaviour {

	private string name;
    private int weight;
    private float quantity;

    public Loot(string name, int weight = 1, float quantity = Mathf.Infinity) {
        this.name = name;
        this.weight = weight;
        this.quantity = quantity == Mathf.Infinity ? quantity : Mathf.Floor(quantity);
    }

    public string Name {
        get { return name; }
        set { name = value; }
    }

    public int Weight {
        get { return weight; }
        set { weight = value; }
    }

    public float Quantity {
        get { return quantity; }
        set { quantity = value; }
    }

}
