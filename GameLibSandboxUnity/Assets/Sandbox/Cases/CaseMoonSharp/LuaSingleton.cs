using GameLib.Alg;
using MoonSharp.Interpreter;
using NaughtyAttributes;
using UnityEngine;

public class LuaSingleton : Singleton<LuaSingleton>
{
    public Script Script { get; private set; }
#if UNITY_EDITOR
    public string TableName = "_G";
    [ResizableTextArea]
    public string String = "print(_G)";
#endif

    protected override void Awake()
    {
        base.Awake();

        // Redefine print to print using Unity Debug.Log
        Script.DefaultOptions.DebugPrint = s => Debug.Log(s);

        // Note: this call should be after defining  Script.DefaultOptions
        Script = new Script();
    }

    public void PrintLuaTable(string tableName)
    {
        Script.DoString($"for n in pairs({tableName}) do print(n) end");
    }

    public void PrintLuaTable(Table table)
    {
        foreach (var tablePair in table.Pairs)
        {
            Debug.Log($"{tablePair.Key} = {tablePair.Value}");
        }
    }

#if UNITY_EDITOR
    [Button()]
    void PrintLuaTable()
    {
        PrintLuaTable(TableName);
    }

    [Button()]
    void DoString()
    {
        Script.DoString(String);
    }
#endif
}
