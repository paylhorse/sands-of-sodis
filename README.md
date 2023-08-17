![steam-header](https://github.com/paylhorse/sands-of-sodis/assets/74363924/994893e1-106c-4587-81ee-605e2a32972b)

**Source-Available**

**Homepage:** [sandsofsodis.com](https://www.sandsofsodis.com/)

## Technical Overview

SANDS OF SODIS is built atop an advanced Unity framework, inspired by the code structure of PSX/Dreamcast-era JRPGs. We employ [DarkMoonSharp](https://github.com/paylhorse/darkmoonsharp) to harness the power of LuaJIT, creating a flexible and high-performance "kernel" of sorts around the core game logic. The C# codebase within employs ECS modules, which are optimized with Burst compilation at runtime (DOTS). Our performance and stability is benchmarked against hand-crafted C++ engines akin to Valve's Source, and nothing less.

## Console Commands

Below is a detailed look at our robust Quantum Console-based interface, which we use internally for balancing. You can access it from the Dev Room. Additionally, we'll soon be integrating a Lua-based modding interface, allowing you to create your own weapons, units or gamemodes!

### Syntax

Commands used in the in-game console are structured as follows:

**command** _argument1_ _argument2_ _argument3_...

### Running a Battle

**Use the** `help` **command for a list of enemy types.**

`help enemy`

**Start by using the** `create` **command, to add an instance of a certain enemy to the kernel.**

`create enemy ['enemyType', 'chosenEnemyName']`

Optionally:

`create enemy ['enemyType', 'chosenEnemyName', level, STR, RES, AGI, DEX, VAS]`

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

`modify chosenEnemyName gearset [headName, bodyName, legsName, ringName, mainHandName, offHandName]`

The enemy's now registered in the kernel. We bring it into the field by **creating a battle**:

`create battle [mapName, chosenBattleName, normal, chosenEnemyName1, chosenEnemyName2...]`

The first parameter dictates starting positions: _normal_, _advantage_ or _disadvantage_.

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
