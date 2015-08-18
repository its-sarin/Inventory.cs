/**
 * Inventory.cs by Tony Recchia (@then00b) (Copyright © 2015)
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
 * lt.Add(new Loot("sword", 0), 20);
 * lt.Add(new Loot("shield", 1), 5);
 * lt.Add(new Loot("gold", 2), 100);
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
using Mathf = UnityEngine.Mathf;

namespace LootSystem {

    [Serializable]
    public class Inventory {

        private Dictionary<Loot, int> table;

        private float inventorySize;
        private float lootLimit;

        public Inventory(float inventorySize = Mathf.Infinity) {
            this.table = new Dictionary<Loot, int>();
            this.inventorySize = inventorySize;
        }

        // [Clear] - Clear the inventory
        public void Clear() {
            this.table.Clear();
        }

        // [Add] - Add a Loot item to inventory with an optional quantity. Returns the added Loot object.
        public bool Add(Loot loot, int quantity = 1) {
            if (this.table.ContainsKey(loot)) {
                this.table[loot] += quantity;
                return true;
            } else if (this.table.Count < inventorySize) {
                this.table.Add(loot, quantity);
                return true;
            }

            return false;
        }

        // [Remove] - Remove a Loot item from inventory
        public bool Remove(Loot loot, int quantity = 1) {
            if (this.table.ContainsKey(loot)) {
                if (this.table[loot] - quantity > 1) {
                    this.table[loot] -= quantity;
                    return true;
                } else {
                    this.table.Remove(loot);
                    return true;
                }
            }

            return false;
        }

        /* [Total] - returns the total number of items in inventory */
        public int Total() {
            return this.table.Count;
        }

        /* [Quantity] - returns the quantity of a given item in inventory */
        public int Quantity(Loot loot) {
            return this.table[loot];
        }

        public int Quantity(string name) {
            List<Loot> list = new List<Loot>(this.table.Keys);
            int c = list.Count;

            for (int i = 0; i < c; i++) {
                if (list[i].Name == name) return this.table[list[i]];
            }

            return 0;
        }

        public int Quantity(int id) {
            List<Loot> list = new List<Loot>(this.table.Keys);
            int c = list.Count;

            for (int i = 0; i < c; i++) {
                if (list[i].Id == id) return this.table[list[i]];
            }

            return 0;
        }

        /* [Contains] - return whether or not a piece of loot is in inventory */
        public bool Contains(Loot loot) {
            return this.table.ContainsKey(loot);
        }

        public bool Contains(string name) {
            List<Loot> list = new List<Loot>(this.table.Keys);
            int c = list.Count;

            for (int i = 0; i < c; i++) {
                if (list[i].Name == name) return true;
            }

            return false;
        }

        public bool Contains(int id) {
            List<Loot> list = new List<Loot>(this.table.Keys);
            int c = list.Count;

            for (int i = 0; i < c; i++) {
                if (list[i].Id == id) return true;
            }

            return false;
        }

        public Dictionary<Loot, int> Contents {
            get { return this.table; }
        }


    }
}
