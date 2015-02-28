# Loot.cs System

The Loot.cs System consists of three core classes:
- Loot.cs
- LootTable.cs
- Inventory.cs

(LootTable.cs is based on LootTable.js by John Watson (Copyright © 2015)
https://github.com/jotson/LootTable.js)

## LootTable.cs

LootTable.cs is used to make a random choice among a weighted list of alternatives for item drops,
map generation, and many other processes. There's a good overview of loot tables on
[Lost Garden](http://www.lostgarden.com/2014/12/loot-drop-tables.html).

### Example

```c#
LootTable lt = new LootTable();
lt.Add(new Loot("sword", 20));
lt.Add(new Loot("shield", 5));
lt.Add(new Loot("gold", 100));
 
Loot item = lt.Choose(); // most likely gold
```

Weights are arbitrary, not percentages, and don't need to add up to 100.
If one item has a weight of 2 and another has a weight of 1, the first item
is twice as likely to be chosen. If quantity is given, then calls to `Choose()`
will only return that item while some are available. Each `Choose()` that
selects that item will reduce its quantity by 1.

Items are built using the included Loot class and accept a string name, int quantity,
and float weight.

## Inventory.cs

Inventory.cs is used to store a list of Loot objects the player has found with `LootTable.Choose()`

### Example

```c#
Inventory inv = new Inventory();

Loot item = lt.Choose();

// Adding one item
inv.Add(item);
inv.Contains(item); // returns true
inv.Count(item); // returns 1

// Adding multiples of one item
inv.Add(item, 4);
inv.Count(item); // now returns 5

// Adding item directly from a LootTable.Choose() method
inv.Add(lt.Choose());
// Inventory.Add() returns the Loot object added so you can perform
// another method on it as you add it
inv.Add(inv.Count(lt.Choose()));

inv.Remove(item);
inv.Count(item); // now returns 4
```
