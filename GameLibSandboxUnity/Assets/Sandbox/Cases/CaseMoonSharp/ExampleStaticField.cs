using System;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Interop;
using MoonSharp.Interpreter.Interop.BasicDescriptors;
using NaughtyAttributes;
using UnityEngine;




public class ExampleStaticField : MonoBehaviour
{
    private Script _script;
    [Button]
    void RunScript()
    {
        _script.DoString(LuaScript);
    }

    [ResizableTextArea]
    public string LuaScript = @"    
Core = {Zombies={Count=Variable}}
print(Core.Zombies.Count.Var)
Core.Zombies.Count.Var = 333
print(Core.Zombies.Count.Var)
	";


    //[MoonSharpUserData]
    static partial class ConsoleVariables
    {
        //public static int Var = 42;
        public static int ZProp1 { get; set; } = 42;

        //public static int ZProp2 { get; set; } = 42;
    }

    //static partial class ConsoleVariables
    //{
    //    public static int Var2 = 2;
    //}


    void Start()
    {
        _script = new Script();


        var registered = UserData.RegisterType(typeof(ConsoleVariables)); // create StandardGenericsUserDataDescriptor and register it in lua registry. the descriptor contains getters and setters for a type
        var dsc = registered as StandardUserDataDescriptor;
        foreach (var keyValuePair in dsc.Members)
        {
            //print(keyValuePair.Key);
            if (keyValuePair.Key == "get_ZProp1")
            {
                print($"name = {keyValuePair.Value.Name} isStatic={keyValuePair.Value.IsStatic}");
                //_script.Globals["Core"] = new Table(_script, keyValuePair.Value.GetValue(_script, null));
                _script.Globals["getter"] = keyValuePair.Value.GetValue(_script, null); // dynval as function for getter
            }

            if (keyValuePair.Key == "set_ZProp1")
            {
                print($"name = {keyValuePair.Value.Name} isStatic={keyValuePair.Value.IsStatic}");
                //_script.Globals["Core2"] = new Table(_script, keyValuePair.Value.GetValue(_script, null));
                _script.Globals["setter"] = keyValuePair.Value.GetValue(_script, null);
            }
            if (keyValuePair.Key == "ZProp1")
            {
                //print($"name = {keyValuePair.Value.Name} isStatic={keyValuePair.Value.IsStatic}");
                //_script.Globals["Core"] = new Table(_script, keyValuePair.Value.s);
            }
            
        }

        _script.DoString(
@"
local MyClass = {}
MyClass.__index = MyClass

setmetatable(MyClass, {
  __call = function (cls, ...)
    return cls.new(...)
  end,
})

function MyClass.new(init)
  local self = setmetatable({}, MyClass)
  self.value = init
  return self
end

-- the : syntax here causes a 'self' arg to be implicitly added before any other args
function MyClass: set_value(newval)
setter(newval)
end

function MyClass: get_value()
    return getter()
end







instance = MyClass(5)
print(instance:get_value())
-- do stuff with instance...
"
        );
        
        //_script.Globals["Variable"] = typeof(ConsoleVariables);

    }

    [Button]
    void PrintCSValue()
    {
        //print("Var = " + ConsoleVariables.Var);
        //print("Var2 = " + ConsoleVariables.Var2);
        print("ZProp1 = " + ConsoleVariables.ZProp1);
    }



    // Create type descriptor for only one property
     


}
