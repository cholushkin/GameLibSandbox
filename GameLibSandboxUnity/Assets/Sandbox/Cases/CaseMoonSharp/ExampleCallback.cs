using System;
using MoonSharp.Interpreter;
using UnityEngine;

public class ExampleCallback : MonoBehaviour
{
 
    private static void Print(string text)
    {
        Debug.Log(text);
    }


    private static int Mul(int a, int b)
    {
        return a * b;
    }

    private static double CallbackTest()
    {
        string scriptCode = @"    
        -- defines a factorial function
        function fact (n)
            if (n == 0) then
                return 1
            else
                return Mul(n, fact(n - 1));
            end
        end";

        Script script = new Script();

        script.Globals["Mul"] = (Func<int, int, int>)Mul;
        script.Globals["Print"] = (Action<string>)Print;
        Script.GlobalOptions.CustomConverters.SetScriptToClrCustomConversion(DataType.Table, typeof(Color),
            v => new Color(1,1,1));

        script.DoString(scriptCode);

        DynValue res = script.Call(script.Globals["fact"], 4);
        script.Call(script.Globals["Print"], "hi");

        return res.Number;
    }

    void Awake()
    {
        var result = CallbackTest();
        Debug.Log(result);
    }

}