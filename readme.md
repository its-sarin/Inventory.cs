# Inventory.cs System

The Inventory.cs System consists of three core classes:
- Loot.cs
- Inventory.cs
- LootTable.cs

(LootTable.cs is based on LootTable.js by John Watson (Copyright © 2015)
https://github.com/jotson/LootTable.js)

## Loot.cs

Loot.cs is a helper class that represents a unique Loot object in your game. It contains various properties for
defining a type of Loot.

### Example 

```c#
/* 
 A Loot item requires at least a string Name and an int Id.
 It can optionally be set with a Rarity rarity (default Rarity.common), int stackLimit (default 99),
 LootType lootType (default LootType.ingredient)

 Rarity represents the relative likelihood that it will be chosen when using LootTable.cs

 stackLimit represents how many of this item can be stacked in one InventorySlot

 LootType is an enum that contains what type of Loot this Loot object is (e.g. 'equipment')
 */

// Creates a new "Sword" Loot object with an Id of 1
Loot sword = new Loot("Sword", 1);
// Sets stackLimit to 1, this item will occupy one InventorySlot for every 1 item
sword.StackLimit = 1;
// Sets its LootType to 'equipment'
sword.Type = LootType.equipment;

// Creates a new "Potion" Loot object with an Id of 2, a rarity of Rarity.rare, 
// can stack 99 per InventorySlot, and of LootType.consumable
Loot potion = new Loot("Potion", 2, Rarity.rare, 99, LootType.consumable);

// Creates a new "Wood" Loot object with an Id of 3
Loot wood = new Loot("Wood", 3); // Will be both stackable and of LootType 'ingredient' by default

```


## Inventory.cs

Inventory.cs is used to store a list of Loot objects such as ones the player has found with `LootTable.Choose()`.
It stores items in Dictionaries (Loot, quantity) inside a grid-based two-dimensional array. The array size is customizable
and each cell represents one grid in a UI.

### Example

```c#
// Inventory constructor has two optional parameters: inventorySize (default 10) and rowLength (default 5)
// inventorySize should be evenly divisible by rowLength.

// Creates a new Inventory with default sizes. By default, this Inventory will be 2 rows of 5 cells (equaling inventorySize 10)
Inventory myInventory = new Inventory();

Loot sword = new Loot("Sword", 1);
sword.Stackable = false;
sword.Type = LootType.equipment;

// Adding one item
myInventory.Add(sword);
myInventory.Contains(sword); // returns true
myInventory.Quantity(sword); // returns 1

// Adding multiples of one item
myInventory.Add(sword, 4);
myInventory.Quantity(sword); // now returns 5

// Chaining Adding items
myInventory.Add(potion, 4).Add(potion);
myInventory.Quantity(potion); // returns 5

// Removing an item
myInventory.Remove(sword);
myInventory.Quantity(sword); // now returns 4

// Removing multiples of an item
myInventory.Remove(sword, 2);
myInventory.Quantity(sword); // now returns 2

// Getting Total of all items in the Inventory
myInventory.Total(); // returns 7 (2 swords, 5 potions)

```


## LootTable.cs
(based on LootTable.js by John Watson (Copyright © 2015) https://github.com/jotson/LootTable.js)

LootTable.cs is used to make a random choice among a weighted list of Loot objects. 

Weights are relative and don't need to add up to 100.
If one item has a weight of 2 and another has a weight of 1, the first item
is twice as likely to be chosen. If quantity is given, then calls to `Choose()`
will only return that item while some are available. Each `Choose()` that
selects that item will reduce its quantity by 1.

### Example

```c#

// Creates a new LootTable
LootTable myLootTable = new LootTable();

// Adds a new "sword" Loot object to the LootTable with a set quantity of 1
myLootTable.Add(new Loot("sword", 20), 1);
// Adds a new "shield" Loot object to the LootTable with a set quantity of 2
myLootTable.Add(new Loot("shield", 5), 2);
// Adds a new "gold" Loot object to the LootTable with no set quantity (quantity will be Infinity)
myLootTable.Add(new Loot("gold", 100));
 
Loot item = myLootTable.Choose(); // most likely gold

// Loot randomly chosen from the LootTable can be added directly to the Inventory
myInventory.Add(myLootTable.Choose()); // Will most likely add "gold" to the Inventory

Loot item = myLootTable.Choose();

// If the chosen Loot is gold..
if (item.Name == "gold") {
	myInventory.Add(item, 5); // Adds 5 gold to the Inventory
} else {
	myInventory.Add(item); // Adds just 1 item
}

```



