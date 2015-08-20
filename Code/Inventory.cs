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

using UnityEngine;
using System.Collections.Generic;
using System;
using Mathf = UnityEngine.Mathf;

namespace LootSystem {

    public class GridVector {
        public int x; public int y;

        public GridVector(int x, int y) {
            this.x = x;
            this.y = y;
        }
    }

    [Serializable]
    public class Inventory {

        // The inventory is a two dimensional array of Loot, quantity Dictionaries.
        // This allows for multiple stacks of the same Loot item inside of one Inventory.
        private Dictionary<Loot, int>[,] grid;

        private int rowLength;

        // The total number of grid cells in the inventory
        private int inventorySize;
        // The number of duplicate items allowed in one grid cell
        private float stackLimit;

        // Inventory size should be a multiple of rowLength
        public Inventory(int inventorySize = 10, int rowLength = 5, float stackLimit = Mathf.Infinity) {
            this.inventorySize = inventorySize;
            this.rowLength = rowLength;
            this.stackLimit = stackLimit;

            grid = new Dictionary<Loot, int>[rowLength, inventorySize / rowLength];            
        }        

        // [Clear] - Clears the inventory
        public void Clear() {
            int xLength = grid.GetLength(0);
            int yLength = grid.GetLength(1);

            for (int x = 0; x < xLength; x++) {
                for (int y = 0; y < yLength; y++) {
                    grid[x, y].Clear();
                }
            }
        }

        // [Add] - Add a Loot item to inventory with an optional quantity. Returns the added Loot object.
        public Inventory Add(Loot loot, int quantity = 1) {
            GridVector vStack = FindFreeStack(loot);
            GridVector vEmpty = FindEmptyCell();

            if (vEmpty == null && vStack == null)
                return this;

            if (loot.Stackable && vStack != null) {
                int x = vStack.x;
                int y = vStack.y;

                if ((grid[x, y][loot] + quantity) <= stackLimit) {
                    grid[x, y][loot] += quantity;

                    return this;
                } else {
                    int added = ((int)stackLimit - grid[x, y][loot]);
                    grid[x, y][loot] += added;
                    quantity -= added;
                    
                    return Add(loot, quantity);
                }
            } else if (vEmpty != null) {
                int x = vEmpty.x;
                int y = vEmpty.y;

                grid[x, y] = new Dictionary<Loot, int>(1);

                if (quantity <= stackLimit) {
                    grid[x, y].Add(loot, quantity);

                    return this;
                } else {
                    int added = (int)stackLimit;
                    grid[x, y].Add(loot, added);
                    quantity -= added;

                    return Add(loot, quantity);
                }
            }

            return this;
        }

        // [Remove] - Remove a Loot item from inventory
        public Inventory Remove(Loot loot, int quantity = 1) {
            GridVector vStack = FindAnyStack(loot);

            if (quantity <= 0 || vStack == null) {
                return this;
            } else {
                int x = vStack.x;
                int y = vStack.y;

                if ((grid[x, y][loot] - quantity) > 0) {
                    grid[x, y][loot] -= quantity;

                    return this;
                } else {
                    int removed = grid[x, y][loot];
                    grid[x, y] = null;
                    quantity -= removed;

                    return Remove(loot, quantity);
                }
            }
        }

        public Inventory Remove(int id, int quantity = 1) {
            Loot loot = GetLoot(id);

            if (loot == null)
                return this;

            return Remove(loot, quantity);
        }

        public Inventory Remove(string name, int quantity = 1) {
            Loot loot = GetLoot(name);

            if (loot == null)
                return this;

            return Remove(loot, quantity);
        }

        /* [FindEmptyStack] - Finds the next available open grid cell, returns coordinates */
        public GridVector FindEmptyCell() {
            int xLength = grid.GetLength(0);
            int yLength = grid.GetLength(1);

            for (int x = 0; x < xLength; x++) {
                for (int y = 0; y < yLength; y++) {
                    if (grid[x, y] == null)
                        return new GridVector(x, y);
                }
            }

            return null;
        }

        /* [FindFreeStack] - Finds the first available open stack of Loot items, returns coordinates */
        public GridVector FindFreeStack(Loot loot) {
            int xLength = grid.GetLength(0);
            int yLength = grid.GetLength(1);

            for (int x = 0; x < xLength; x++) {
                for (int y = 0; y < yLength; y++) {
                    if (grid[x, y] != null && grid[x, y].ContainsKey(loot)) {
                        if (grid[x, y][loot] < stackLimit)
                            return new GridVector(x, y);
                    }

                }
            }

            return null;
        }

        /* [FindAnyStack] - Finds the first available stack of a given Loot item, returns coordinates */
        public GridVector FindAnyStack(Loot loot) {
            int xLength = grid.GetLength(0);
            int yLength = grid.GetLength(1);

            for (int x = 0; x < xLength; x++) {
                for (int y = 0; y < yLength; y++) {
                    if (grid[x, y] != null && grid[x, y].ContainsKey(loot)) {
                        return new GridVector(x, y);
                    }

                }
            }

            return null;
        }

        /* [GetStack] - gets stack (if there is one) at a specified GridVector */
        public Dictionary<Loot, int> GetStack(GridVector pos) {
            return grid[pos.x, pos.y];
        }

        /* [MoveStack] - Moves a stack from a given position to a destination position, returns that stack */
        public Dictionary<Loot, int> MoveStack(GridVector from, GridVector to) {
            Dictionary<Loot, int> originStack = grid[from.x, from.y];
            Dictionary<Loot, int> destinationStack;            

            if (!CellEmpty(to)) {
                destinationStack = grid[to.x, to.y];
                grid[from.x, from.y] = destinationStack;
            } else {
                ClearCell(from);
            }

            grid[to.x, to.y] = originStack;

            return originStack;
        }

        /* [ClearCell] - Clears the contents of a given cell, returns that cell */
        public GridVector ClearCell(GridVector cell) {
            grid[cell.x, cell.y] = null;

            return cell;
        }

        /* [IsCellEmpty] - Returns true or false if given cell is empty */
        public bool CellEmpty(GridVector gridVector) {
            return grid[gridVector.x, gridVector.y] == null;
        }

        /* [FreeCells] - returns how many Inventory cells are currently empty */
        public int FreeCells() {
            int xLength = grid.GetLength(0);
            int yLength = grid.GetLength(1);
            int acc = 0;

            for (int x = 0; x < xLength; x++) {
                for (int y = 0; y < yLength; y++) {
                    if (grid[x, y] != null) {
                        acc++;
                    }
                }
            }

            return inventorySize - acc;
        }


        /* [Total] - returns the total number of items in inventory */
        public int Total() {
            int xLength = grid.GetLength(0);
            int yLength = grid.GetLength(1);
            int acc = 0;

            for (int x = 0; x < xLength; x++) {
                for (int y = 0; y < yLength; y++) {
                    if (grid[x, y] != null) {
                        List<int> list = new List<int>(grid[x, y].Values);
                        for (int i = 0; i < list.Count; i++) {
                            acc += list[i];
                        }
                    }
                }
            }

            return acc;
        }

        /* [Quantity] - returns the quantity of a given item in inventory */
        public int Quantity(Loot loot) {
            int xLength = grid.GetLength(0);
            int yLength = grid.GetLength(1);
            int acc = 0;

            for (int x = 0; x < xLength; x++) {
                for (int y = 0; y < yLength; y++) {
                    if (grid[x, y] != null && grid[x, y].ContainsKey(loot)) {
                        acc += grid[x, y][loot];
                    }
                }
            }

            return acc;
        }

        public int Quantity(int id) {
            int xLength = grid.GetLength(0);
            int yLength = grid.GetLength(1);
            int acc = 0;
            Loot loot = GetLoot(id);

            if (loot == null)
                return acc;

            for (int x = 0; x < xLength; x++) {
                for (int y = 0; y < yLength; y++) {
                    if (grid[x, y] != null && grid[x, y].ContainsKey(loot)) {
                        acc += grid[x, y][loot];
                    }
                }
            }

            return acc;
        }

        public int Quantity(string name) {
            int xLength = grid.GetLength(0);
            int yLength = grid.GetLength(1);
            int acc = 0;
            Loot loot = GetLoot(name);

            if (loot == null)
                return acc;

            for (int x = 0; x < xLength; x++) {
                for (int y = 0; y < yLength; y++) {
                    if (grid[x, y] != null && grid[x, y].ContainsKey(loot)) {
                        acc += grid[x, y][loot];
                    }
                }
            }

            return acc;
        }

        /* [Contains] - return whether or not a piece of loot is in inventory */
        public bool Contains(Loot loot) {
            int xLength = grid.GetLength(0);
            int yLength = grid.GetLength(1);

            for (int x = 0; x < xLength; x++) {
                for (int y = 0; y < yLength; y++) {
                    if (grid[x, y] != null && grid[x, y].ContainsKey(loot)) {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool Contains(int id) {
            int xLength = grid.GetLength(0);
            int yLength = grid.GetLength(1);
            Loot loot = GetLoot(id);

            if (loot == null)
                return false;

            for (int x = 0; x < xLength; x++) {
                for (int y = 0; y < yLength; y++) {
                    if (grid[x, y] != null && grid[x, y].ContainsKey(loot))
                        return true;
                }
            }

            return false;
        }

        /* [GetLoot] - Returns Loot object by id or string */
        public Loot GetLoot(string name) {
            int xLength = grid.GetLength(0);
            int yLength = grid.GetLength(1);

            for (int x = 0; x < xLength; x++) {
                for (int y = 0; y < yLength; y++) {
                    if (grid[x, y] != null) {
                        List<Loot> list = new List<Loot>(grid[x, y].Keys);
                        int c = list.Count;

                        for (int i = 0; i < c; i++) {
                            if (list[i].Name == name)
                                return list[i];
                        }
                    }
                }
            }

            return null;
        }

        public Loot GetLoot(int id) {
            int xLength = grid.GetLength(0);
            int yLength = grid.GetLength(1);

            for (int x = 0; x < xLength; x++) {
                for (int y = 0; y < yLength; y++) {
                    if (grid[x, y] != null) {
                        List<Loot> list = new List<Loot>(grid[x, y].Keys);
                        int c = list.Count;
                        
                        for (int i = 0; i < c; i++) {
                            if (list[i].Id == id)
                                return list[i];
                        }
                    }
                }
            }

            return null;
        }

        public Dictionary<Loot, int>[,] Contents {
            get { return grid; }
        }
    }
}
