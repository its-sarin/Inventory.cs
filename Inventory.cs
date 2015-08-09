/**
 * Inventory.cs by Tony Recchia (@then00b)
 * Inventory class for use with LootTable.cs and Loot.cs
 * 
 * LootTable.cs is based on LootTable.js by John Watson (Copyright © 2015)
 * https://github.com/jotson/LootTable.js/blob/master/LootTable.js
 * 
 * Licensed under the terms of the MIT License
 * ---
 * 
 * Inventory.cs is meant to be used in conjunction with Loot.cs and
 * optionally LootTable.cs.
 *
 * Example:
 * 
 * LootTable lt = new LootTable();
 * Inventory inv = new Inventory();
 * 
 * lt.Add(new Loot("sword", 20));
 * lt.Add(new Loot("shield", 5));
 * lt.Add(new Loot("gold", 100));
 * 
 * Loot item = lt.Choose(); // most likely gold
 * 
 * inv.Add(item); // Adds loot stored in item variable to this inventory
 * inv.Add(lt.Choose()); // Adds loot randomly chosen from LootTable to this inventory
 * 
 * inv.Contains(item); // Returns 'true'
 * inv.Count(item); // Returns 1;
 * 
 */

using System.Collections.Generic;
using System;

[Serializable]
public class Inventory {

    private Dictionary<Loot, int> table;

    public Inventory() {
        this.table = new Dictionary<Loot, int>();
    }

    // [Clear] - Clear the inventory
    public void Clear() {
        this.table.Clear();
    }

    // [Add] - Add a Loot item to inventory with an optional quantity. Returns the added Loot object.
    public Loot Add(Loot loot, int quantity = 1) {
        if (this.table.ContainsKey(loot)) {
            this.table[loot] += quantity;
        } else {
            this.table.Add(loot, quantity);
        }        

        return loot;
    }

    // [Remove] - Remove a Loot item from inventory
    public void Remove(Loot loot, int quantity = 1) {
        if (this.table.ContainsKey(loot)) {
            if (this.table[loot] - quantity > 1) {
                this.table[loot] -= quantity;
            } else {
                this.table.Remove(loot);                
            }
        }            
    }

    /* [Total] - returns the total number of items in inventory */
    public int Total() {
        return this.table.Count;
    }

    /* [Count] - returns the quanityt of a given item in inventory */
    public int Count(Loot loot) {
        return this.table[loot];
    }

    public int Count(string name) {
        foreach (KeyValuePair<Loot, int> item in this.table) {
            if (item.Key.Name == name) {
                return item.Value;
            }
        }

        return 0;
    }

    public int Count(int id) {
        foreach (KeyValuePair<Loot, int> item in this.table) {
            if (item.Key.Id == id) {
                return item.Value;
            }
        }

        return 0;
    }

    /* [Contains] - return whether or not a piece of loot is in inventory */
    public bool Contains(Loot loot) {
        return this.table.ContainsKey(loot);
    }

    public bool Contains(string name) {
        foreach (KeyValuePair<Loot, int> item in this.table) {
            if (item.Key.Name == name) {
                return true;
            }
        }

        return false;
    }

    public bool Contains(int id) {
        foreach (KeyValuePair<Loot, int> item in this.table) {
            if (item.Key.Id == id) {
                return true;
            }
        }

        return false;
    }

    public Dictionary<Loot, int> Show() {
        return this.table;
    }

    
}
