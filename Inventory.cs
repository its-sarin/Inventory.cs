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

using System.Collections;
using System.Collections.Generic;

public class Inventory {

    private List<Loot> table;

    public Inventory() {
        this.table = new List<Loot>();
    }

    // Clear the inventory
    public void Clear() {
        this.table.Clear();
    }

    // Add a piece of loot to inventory with an optional quantity. Returns the added Loot object.
    public Loot Add(Loot loot, int quantity = 1) {
        if (quantity <= 1) {
            this.table.Add(loot);
        } else {
            for (int i = 0; i < quantity; i++) {
                this.table.Add(loot);
            }
        }

        return loot;
    }

    // Remove a piece of loot from inventory
    public void Remove(Loot loot) {
        this.table.Remove(loot);
    }

    /* [Count] - return the total quantity of this loot in inventory */
    public int Count(Loot loot) {
        List<Loot> list;

        list = this.table.FindAll(l => l == loot);

        return list.Count;
    }

    public int Count(string name) {
        List<Loot> list;

        list = this.table.FindAll(l => l.Name == name);

        return list.Count;
    }

    public int Count(int id) {
        List<Loot> list;

        list = this.table.FindAll(l => l.Id == id);

        return list.Count;
    }
    

    /* [Contains] - return whether or not a piece of loot is in inventory */
    public bool Contains(Loot loot) {
        return this.table.Contains(loot);
    }

    public bool Contains(string name) {
        int c = this.table.Count;
        for (int i = 0; i < c; i++) {
            if (this.table[i].Name == name) {
                return true;
            }
        }
        return false;
    } 

    public bool Contains(int id) {
        int c = this.table.Count;
        for (int i = 0; i < c; i++) {
            if (this.table[i].Id == id) {
                return true;
            }
        }
        return false;
    }    
}
