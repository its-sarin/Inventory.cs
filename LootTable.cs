/**
 * Rebuilt in C# by Tony Recchia (@then00b)
 * 
 * Based on LootTable.js by John Watson (Copyright © 2015)
 * https://github.com/jotson/LootTable.js/blob/master/LootTable.js
 * 
 * Licensed under the terms of the MIT License
 * ---
 * 
 * Note: LootTable.cs uses a custom Loot.cs class to create Loot items
 * 
 * LootTable is used to make a random choice among a weighted list of alternatives
 * for item drops, map generation, and many other processes. Here's a good overview
 * of loot tables: http://www.lostgarden.com/2014/12/loot-drop-tables.html
 *
 * Example:
 *
 * LootTable lt = new LootTable();
 * lt.Add(new Loot("sword", 0), 20);
 * lt.Add(new Loot("shield", 1), 5);
 * lt.Add(new Loot("gold", 2), 100);
 * 
 * Loot item = lt.Choose(); // most likely gold
 */

using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using System;

[Serializable]
public class LootTable {

    //private List<Loot> table;
    private Dictionary<Loot, float> table;

    public LootTable() {
        this.table = new Dictionary<Loot, float>();
    }

    public void Clear() {
        this.table.Clear();
    }

    /**
     * Add an item
     *
     * Weights are arbitrary, not percentages, and don't need to add up to 100.
     * If one item has a weight of 2 and another has a weight of 1, the first item
     * is twice as likely to be chosen. If quantity is given, then calls to Choose()
     * will only return that item while some are available. Each Choose() that
     * selects that item will reduce its quantity by 1.
     *
     * 
     * new LootTable().Add(new Loot(string name, int weight), 2);
     */

    public LootTable Add(Loot loot, float quantity = Mathf.Infinity) {
        // If quantity is not Infinity, if value is 0 or less, set to Infinity. 
        // Otherwise round the quantity to ensure whole numbers and continue.
        if (quantity != Mathf.Infinity) {
            quantity = quantity <= 0 ? Mathf.Infinity : Mathf.Round(quantity);
        }

        this.table.Add(loot, quantity);

        return this;
    }

    /* Choose a Loot object from the LootTable based on its weight. 
     * Returns the chosen Loot item
     */
    public Loot Choose() {
        if (this.table.Count == 0) return null;

        List<Loot> list = new List<Loot>(this.table.Keys);
        int c = list.Count;
        int i;
        int totalWeight = 0;
        Loot loot;

        for (i = 0; i < c; i++) {
            loot = list[i];
            if (this.table[loot] > 0) {
                totalWeight += loot.Weight;
            }
        }

        int choice = 0;
        int weight = 0;
        float randomNumber = Mathf.Floor(Random.value * totalWeight + 1);

        for (i = 0; i < c; i++) {
            loot = list[i];
            if (this.table[loot] <= 0) continue;

            weight += loot.Weight;
            if (randomNumber <= weight) {
                choice = i;
                break;
            }
        }

        Loot chosenItem = list[choice];
        this.table[list[choice]]--;

        return chosenItem;
    }
}