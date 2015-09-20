/**
 * Custom Loot class helper for LootTable.cs 
 * 
 * Written by Tony Recchia (@then00b) (Copyright © 2015)
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

using System;

namespace LootSystem {

    [Serializable]
    public class Loot {

        // Name of the Loot item
        private string m_name;
        // Rarity of the Loot item, this dictates the likelihood of it being
        // randomly selected by a LootTable object
        private Rarity m_rarity;
        // ID used as an identifier
        private int m_id;
        // How many of this Loot item can exist in one Inventory stack
        private int m_stackLimit;
        // Type of Loot
        private LootType m_lootType;
        // If the inventory should only contain a certain amount of this
        // Loot item, set inventoryLimit to that number (default is int.MaxValue)
        private int m_inventoryLimit;

        // Change these to whatever suits your game
        // Rarity dictates the likelihood of an item being selected from a LootTable
        public enum Rarity { common = 500, uncommon = 250, rare = 50, epic = 10, legendary = 1 };
        public enum LootType { ingredient, equipment, consumable, block, upgrade, relic, trinket };

        public Loot(string name, 
                    int id, 
                    Rarity rarity = Rarity.common, 
                    int stackLimit = 99, 
                    LootType lootType = LootType.ingredient, 
                    int inventoryLimit = int.MaxValue) {
            m_name = name;
            m_id = id;
            m_rarity = rarity;
            m_stackLimit = stackLimit;
            m_lootType = lootType;
            m_inventoryLimit = inventoryLimit;
        }

        public string Name {
            get { return m_name; }
            set { m_name = value; }
        }        

        public int Id {
            get { return m_id; }
            set { m_id = value; }
        }

        public Rarity LootRarity {
            get { return m_rarity; }
            set { m_rarity = value; }
        }

        public int StackLimit {
            get { return m_stackLimit; }
            set { m_stackLimit = value; }
        }

        public LootType Type {
            get { return m_lootType; }
            set { m_lootType = value; }
        }

        public int InventoryLimit {
            get { return m_inventoryLimit; }
            set { m_inventoryLimit = value; }
        }

        public override bool Equals(object obj) {
            if (GetHashCode() == obj.GetHashCode())
                return true;
            return false;
        }

        public override int GetHashCode() {
            unchecked {
                int hash = 47;

                hash = hash * 227 + m_id.GetHashCode();

                return hash;
            }
        }
    }
}
