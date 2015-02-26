# LootTable.cs
 
Based on LootTable.js by John Watson (Copyright © 2015)
https://github.com/jotson/LootTable.js/blob/master/LootTable.js

LootTable is used to make a random choice among a weighted list of alternatives for item drops,
map generation, and many other processes. There's a good overview of loot tables on
[Lost Garden](http://www.lostgarden.com/2014/12/loot-drop-tables.html).

## Example

```cs
LootTable lt = new LootTable();
lt.add(new Loot("sword", 20));
lt.add(new Loot("shield", 5));
lt.add(new Loot("gold", 100));
 
Loot item = lt.Choose(); // most likely gold
```

Weights are arbitrary, not percentages, and don't need to add up to 100.
If one item has a weight of 2 and another has a weight of 1, the first item
is twice as likely to be chosen. If quantity is given, then calls to `choose()`
will only return that item while some are available. Each `choose()` that
selects that item will reduce its quantity by 1.

Items are built using the included Loot class and accept a string name, int quantity,
and float weight.