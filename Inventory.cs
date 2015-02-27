using UnityEngine;
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

    // Add a piece of loot to inventory
    public void Add(Loot loot) {
        this.table.Add(loot);
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
