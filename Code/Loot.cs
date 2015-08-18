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

        private string name;
        private int weight;
        private int id;
        private LootType lootType;

        // Change these to whatever suits your game
        public enum LootType { material, equipment, consumable, block, upgrade, relic };

        public Loot(string name, int id, int weight = 1, LootType lootType = LootType.material) {
            this.name = name;
            this.id = id;
            this.weight = weight;
            this.lootType = lootType;
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

        public LootType Type {
            get { return this.lootType; }
            set { this.lootType = value; }
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
}
