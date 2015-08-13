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
using System;

[Serializable]
public class Loot {

    private string name;
    private int weight;
    private float quantity;
    private int id;

    public Loot(string name, int id, int weight = 1) {
        this.name = name;
        this.id = id;
        this.weight = weight;
    }

    public string Name {
        get { return this.name; }
        set { this.name = value; }
    }

    public int Weight {
        get { return this.weight; }
        set { this.weight = value; }
    }

    public int Id {
        get { return this.id; }
        set { this.id = value; }
    }

    public override bool Equals(object obj) {
        if (GetHashCode() == obj.GetHashCode())
            return true;
        return false;
    }

    public override int GetHashCode() {
        unchecked {
            int hash = 47;

            hash = hash * 227 + this.id.GetHashCode();

            return hash;
        }
    }
}
