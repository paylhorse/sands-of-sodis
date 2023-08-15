![steam-header](https://github.com/paylhorse/sands-of-sodis/assets/74363924/994893e1-106c-4587-81ee-605e2a32972b)

**Source-Available**

**Engine:** Unity

**Homepage:** [sandsofsodis.com](https://www.sandsofsodis.com/)

## Console Commands

Commands used in the in-game console are structured as follows:

**action** *subjectType* (subject) value

### Running a Battle

**Start by using the** `create` **command, to add an instance of a certain enemy to the kernel.**

`create enemy ('enemyName', 'chosenName', level)`

Optionally:

`create enemy ('enemyName', 'chosenName', level, STR, RES, AGI, DEX, VAS)`

**Use the** `list` **command to query the current instance.**

`list enemy`

This should display the enemy you just created.

**Use the** `check` **command to print an instanced enemy's level, stats or equipped gear.**

`check chosenName level`
`check chosenName AGI`

**Use the** `modify` **command to update an instanced enemy's level, stats or equipped gear.**

`modify chosenName level 10`

Setting a level imposes a default set of stats, for an enemy of that type and level.

`modify chosenName AGI 16`

The enemy's now registered in the kernel. We bring it into the field by **creating a battle**:

`create battle (battleName, normal, chosenName1, chosenName2...)`

The first parameter dictates starting positions: *normal*, *advantage* or *disadvantage*.

`list battle`

**Use the** `spawn` **command to instantiate the battle, in the form of the leading enemy.**

`spawn battleName`

The enemy will behave as it does in the main game, actively searching for the player.

### Special Commands

**Use** `freeze` **to stop all enemies in their tracks.*

`freeze on`
`freeze off`

This is useful if you'd like to spawn multiple battles, to simulate chain encounters.

**Use** `simulate` **to run a comparision of stats and simulate a battle, outputting it's result.**

`simulate battle battleName`

