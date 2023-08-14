using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;
using System;

//      +--------------+
//     /|             /|
//    *--+-----------* |
//    | | LUA        | |
//    | | BACKBONE   | |
//    | +------------|-+
//    |/             |/
//    *--------------*
//
// Singleton to manage the Lua database
//
// MOONSCRIPT SYNTAX:
// Read:
// DynValue XXXValue = luaData.Globals.Get("XXX");
// int VIT = VITValue.Number;
// Write:
// luaData.Globals.Set("XXX", DynValue.NewNumber(newValue));
//

public class LuaBackbone : MonoBehaviour
{
    public static LuaBackbone Instance { get; private set; }
    
    public Script luaData;

    private void Awake()
    {
        // Persist as Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void BeginAnew()
    {
        // Run when starting a New Game

        // Initialize the Lua Script
        luaData = new Script();
        luaData.DoString("require('PlayerData')");
        luaData.DoString("require('BUnit')");

	// Initialize LuaLog
	luaData.Globals["Log"] = (Action<string>)LuaLog;

        // Create an instance of PlayerData
        luaData.Globals["playerData"] = luaData.DoString("return PlayerData.new()");

        // Create Toria and his reference
        DynValue Toria = luaData.DoString("return BUnit.new('Toria', 12, 11, 10, 11, 7 )");
	luaData.Globals["ToriaReference"] = Toria;

        // Add Toria to the player's party
        luaData.DoString($"playerData:addUnit(ToriaReference)");

        // Initialize Determination, Toria's unique stat
	// Read base stats:
        int STR = (int)Toria.Table.Get("STR").Number;
        int RES = (int)Toria.Table.Get("RES").Number;
        int AGI = (int)Toria.Table.Get("AGI").Number;
        int DEX = (int)Toria.Table.Get("DEX").Number;
	// Calculate and add DET to table:
        int maxDET = (STR * 6 + RES * 6 + AGI * 6 + DEX * 6) / 4;
	int DET = maxDET;
        Toria.Table.Set("maxDET", DynValue.NewNumber(maxDET));
        Toria.Table.Set("DET", DynValue.NewNumber(DET));
    }

    public void LuaLog(string message)
    {
	Debug.Log("[Lua]: " + message);
    }
}
