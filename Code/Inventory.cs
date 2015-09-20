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
 * inv.Quantity(item); // Returns 1;
 * 
 */

using System;
using Mathf = UnityEngine.Mathf;

namespace LootSystem {

    [Serializable]
    public class GridVector {
        public int x; public int y;

        public GridVector(int x, int y) {
            this.x = x;
            this.y = y;
        }
    }

    [Serializable]
    public class InventorySlot {
        public Loot loot;
        public int amount;

        public InventorySlot(Loot loot, int amount) {
            this.loot = loot;
            this.amount = amount;
        }
    }

    [Serializable]
    public class Inventory {

        // The inventory is a two dimensional array of InventorySlots.
        // This allows for multiple stacks of the same Loot item inside of one Inventory.
        public InventorySlot[,] grid;

        // The total number of grid cells in the inventory
        private int inventorySize;

        // Inventory size should be a multiple of rowLength
        public Inventory(int inventorySize = 10, int rowLength = 5) {
            this.inventorySize = inventorySize;

            grid = new InventorySlot[rowLength, inventorySize / rowLength];            
        }        

        // [Clear] - Clears the inventory
        public void Clear() {
            int xLength = grid.GetLength(0);
            int yLength = grid.GetLength(1);

            for (int x = 0; x < xLength; x++) {
                for (int y = 0; y < yLength; y++) {
                    grid[x, y] = null;
                }
            }
        }

        // [Add] - Add a Loot item to inventory with an optional quantity. Returns the added Loot object.
        public Inventory Add(Loot loot, int quantity = 1) {
            GridVector vStack = FindFreeStack(loot);
            GridVector vEmpty = FindEmptyCell();
            int stackLimit = loot.StackLimit;

            if ((vEmpty == null && vStack == null) || (Quantity(loot) >= loot.InventoryLimit))
                return this;

            if (vStack != null) {
                int x = vStack.x;
                int y = vStack.y;                

                if ((grid[x, y].amount + quantity) <= stackLimit) {
                    grid[x, y].amount += quantity;

                    return this;
                } else {
                    int added = (stackLimit - grid[x, y].amount);
                    grid[x, y].amount += added;
                    quantity -= added;
                    
                    return Add(loot, quantity);
                }
            } else if (vEmpty != null) {
                int x = vEmpty.x;
                int y = vEmpty.y;

                if (quantity <= stackLimit) {
                    grid[x, y] = new InventorySlot(loot, quantity);

                    return this;
                } else {
                    int added = stackLimit;
                    grid[x, y]= new InventorySlot(loot, added);
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

                if ((grid[x, y].amount - quantity) > 0) {
                    grid[x, y].amount -= quantity;

                    return this;
                } else {
                    int removed = grid[x, y].amount;
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

        /* [CanAdd] - returns whether or not there is space for a given Loot object */
        public bool CanAdd(Loot loot) {
            if (Quantity(loot) >= loot.InventoryLimit)
                return false;

            if (HasFreeCell())
                return true;

            int xLength = grid.GetLength(0);
            int yLength = grid.GetLength(1);

            for (int x = 0; x < xLength; x++) {
                for (int y = 0; y < yLength; y++) {
                    if (grid[x, y] != null && grid[x, y].loot.Equals(loot)) {
                        if (grid[x, y].amount < loot.StackLimit)
                            return true;
                    }
                }
            }

            return false;
        }

        /* [FindEmptyStack] - Finds the next available open grid cell, returns coordinates */
        public GridVector FindEmptyCell() {
            int xLength = grid.GetLength(0);
            int yLength = grid.GetLength(1);

            for (int y = 0; y < yLength; y++) {
                for (int x = 0; x < xLength; x++) {
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

            for (int y = 0; y < yLength; y++) {
                for (int x = 0; x < xLength; x++) {
                    if (grid[x, y] != null && grid[x, y].loot.Equals(loot)) {
                        if (grid[x, y].amount < loot.StackLimit)
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

            for (int y = 0; y < yLength; y++) {
                for (int x = 0; x < xLength; x++) {
                    if (grid[x, y] != null && grid[x, y].loot.Equals(loot)) {
                        return new GridVector(x, y);
                    }

                }
            }

            return null;
        }

        /* [GetStack] - gets stack (if there is one) at a specified GridVector */
        public InventorySlot GetStack(GridVector pos) {
            return grid[pos.x, pos.y];
        }

        /* [MoveStack] - Moves a stack from a given position to a destination position, returns that stack */
        public InventorySlot MoveStack(GridVector from, GridVector to) {
            InventorySlot originStack = GetStack(from);
            InventorySlot destinationStack;            

            if (!CellEmpty(to)) {
                destinationStack = GetStack(to);
                grid[from.x, from.y] = destinationStack;
            } else {
                ClearCell(from);
            }

            grid[to.x, to.y] = originStack;

            return originStack;
        }

        public InventorySlot MoveStack(GridVector from, Inventory toInventory, GridVector to) {
            InventorySlot originStack = GetStack(from);
            InventorySlot destinationStack;

            if (!toInventory.CellEmpty(to)) {
                destinationStack = toInventory.GetStack(to);
                grid[from.x, from.y] = destinationStack;
            } else {
                ClearCell(from);
            }

            toInventory.Contents[to.x, to.y] = originStack;

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

            for (int y = 0; y< yLength; y++) {
                for (int x = 0; x < xLength; x++) {
                    if (grid[x, y] != null) {
                        acc++;
                    }
                }
            }

            return inventorySize - acc;
        }

        /* [HasFreeCell] - returns whether or not Inventory has any empty cells */
        public bool HasFreeCell() {
            int xLength = grid.GetLength(0);
            int yLength = grid.GetLength(1);

            for (int y = 0; y < yLength; y++) {
                for (int x = 0; x < xLength; x++) {
                    if (grid[x, y] == null) {
                        return true;
                    }
                }
            }

            return false;
        }


        /* [Total] - returns the total number of items in inventory */
        public int Total() {
            int xLength = grid.GetLength(0);
            int yLength = grid.GetLength(1);
            int acc = 0;

            for (int y = 0; y < yLength; y++) {
                for (int x = 0; x < xLength; x++) {
                    if (grid[x, y] != null) {
                        acc += grid[x, y].amount;                        
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

            for (int y = 0; y < yLength; y++) {
                for (int x = 0; x < xLength; x++) {
                    if (grid[x, y] != null && grid[x, y].loot.Equals(loot)) {
                        acc += grid[x, y].amount;
                    }
                }
            }

            return acc;
        }

        public int Quantity(int id) {
            return Quantity(GetLoot(id));
        }

        public int Quantity(string name) {
            return Quantity(GetLoot(name));
        }

        /* [Contains] - return whether or not a piece of loot is in inventory */
        public bool Contains(Loot loot) {
            int xLength = grid.GetLength(0);
            int yLength = grid.GetLength(1);

            for (int y = 0; y < yLength; y++) {
                for (int x = 0; x < xLength; x++) {
                    if (grid[x, y] != null && grid[x, y].loot.Equals(loot)) {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool Contains(int id) {
            return Contains(GetLoot(id));
        }

        public bool Contains(string name) {
            return Contains(GetLoot(name));
        }

        /* [GetLoot] - Returns Loot object by id or string */
        public Loot GetLoot(string name) {
            int xLength = grid.GetLength(0);
            int yLength = grid.GetLength(1);

            for (int y = 0; y < yLength; y++) {
                for (int x = 0; x < xLength; x++) {
                    if (grid[x, y] != null && grid[x, y].loot.Name == name) {
                        return grid[x, y].loot;
                    }
                }
            }

            return null;
        }

        public Loot GetLoot(int id) {
            int xLength = grid.GetLength(0);
            int yLength = grid.GetLength(1);

            for (int y = 0; y < yLength; y++) {
                for (int x = 0; x < xLength; x++) {
                    if (grid[x, y] != null && grid[x, y].loot.Id == id) {
                        return grid[x, y].loot;
                    }
                }
            }

            return null;
        }

        public Loot GetLoot(GridVector gridVector) {
            if (CellEmpty(gridVector))
                return null;            

            return grid[gridVector.x, gridVector.y].loot;
        }

        public InventorySlot[,] Contents {
            get { return grid; }
        }

        public int Size { get { return inventorySize; } }
    }    
}
