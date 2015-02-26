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
 * lt.add(new Loot("sword", 20));
 * lt.add(new Loot("shield", 5));
 * lt.add(new Loot("gold", 100));
 * 
 * Loot item = lt.Choose(); // most likely gold
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LootTable : MonoBehaviour {

    private List<Loot> table;

    public LootTable() {
        this.table = new List<Loot>();        
    }

    public void Clear() {
        this.table.Clear();
    }

    /**
     * Add an item
     *
     * Weights are arbitrary, not percentages, and don't need to add up to 100.
     * If one item has a weight of 2 and another has a weight of 1, the first item
     * is twice as likely to be chosen. If quantity is given, then calls to choose()
     * will only return that item while some are available. Each choose() that
     * selects that item will reduce its quantity by 1.
     *
     * 
     * new Loot(string name, int weight, float quantity);
     */

    public void Add(Loot loot) {
        this.table.Add(loot);
    }

    /* Choose a Loot object from the LootTable based on its weight. 
     * Returns the string Name of the Loot
     */
    public string Choose() {
        if (this.table.Count == 0) return null;
        
        int i;
        Loot v;
        int totalWeight = 0;

        for (i = 0; i < this.table.Count; i++) {
            v = this.table[i];
            if (v.Quantity > 0) {
                totalWeight += v.Weight;
            }
        }

        int choice = 0;
        float randomNumber = Mathf.Floor(Random.value * totalWeight + 1);
        int weight = 0;

        for (i = 0; i < this.table.Count; i++) {
            v = this.table[i];
            if (v.Quantity <= 0) continue;

            weight += v.Weight;
            if (randomNumber <= weight) {
                choice = i;
                break;
            }
        }

        Loot chosenItem = this.table[choice];
        this.table[choice].Quantity--;

        return chosenItem.Name;
    }
}