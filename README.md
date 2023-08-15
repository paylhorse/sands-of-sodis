![steam-header](https://github.com/paylhorse/sands-of-sodis/assets/74363924/994893e1-106c-4587-81ee-605e2a32972b)

**Source-Available**

**Engine:** Unity

**Homepage:** [sandsofsodis.com](https://www.sandsofsodis.com/)

## Console Commands

I've chosen to make public the robust Debug Console being used internally for balancing: it can be accessed from the Dev Room.

Commands used in the in-game console are structured as follows:

**action** *subjectType* (subject) value

### Running a Battle

**Use the** `help` **command for a list of enemy types.**

`help enemy`

**Start by using the** `create` **command, to add an instance of a certain enemy to the kernel.**

`create enemy ('enemyType', 'chosenEnemyName')`

Optionally:

`create enemy ('enemyType', 'chosenEnemyName', level, STR, RES, AGI, DEX, VAS)`

**Use the** `list` **command to query the current instance.**

`list enemy`

This should display the enemy you just created.

**Use the** `check` **command to print an instanced enemy's level, stats or equipped gear.**

`check chosenEnemyName level`
`check chosenEnemyName AGI`
`check chosenEnemyName gearset`


**Use the** `modify` **command to update an instanced enemy's level, stats or equipped gear.**

`modify chosenEnemyName level 10`

Setting a level imposes a default set of stats, for an enemy of that type and level.

`modify chosenEnemyName AGI 16`
`modify chosenEnemyName gearset (headName, bodyName, legsName, ringName, mainHandName, offHandName)`

The enemy's now registered in the kernel. We bring it into the field by **creating a battle**:

`create battle (mapName, chosenBattleName, normal, chosenEnemyName1, chosenEnemyName2...)`

The first parameter dictates starting positions: *normal*, *advantage* or *disadvantage*.

`list battle`

**Use the** `spawn` **command to instantiate the battle, in the form of the leading enemy.**

`spawn chosenBattleName`

The enemy will behave as it does in the main game, actively searching for the player.

### Special Commands

**Use** `freeze` **to stop all enemies in their tracks.**

`freeze on`
`freeze off`

This is useful if you'd like to spawn multiple battles, to simulate chain encounters.

**Use** `simulate` **to run a comparision of stats and simulate a battle, outputting it's result.**

`simulate battle battleName`

