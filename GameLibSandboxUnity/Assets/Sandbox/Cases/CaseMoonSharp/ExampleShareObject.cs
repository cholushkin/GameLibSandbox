using System.Collections;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using NaughtyAttributes;
using NaughtyAttributes.Test;
using UnityEngine;




public class ExampleShareObject : MonoBehaviour
{
    private Script _script;
    [ResizableTextArea]
    public string LuaScript = @"    
		zombie1.SayHi()
        zombie2.SayHi()
	";

    [MoonSharpUserData]
    class Zombie
    {
        public float Health;
        public string Name;

        public Zombie(string name = "nameless", float health = 10f)
        {
            Name = name;
            Health = health;
        }

        public void ReceiveDamage(float damage)
        {
            Health -= damage;
        }

        public void SayHi()
        {
            Debug.Log($"Zombie {Name}: Hi!");
        }
    }


    void Start()
    {
        // Automatically register all MoonSharpUserData types
        UserData.RegisterAssembly();
        
        _script = new Script();

        // Pass an instance of the class to the script in a global
        _script.Globals["zombie1"] = new Zombie("Bill");
        _script.Globals["zombie2"] = new Zombie("Bob");

    }

    [Button]
    void RunScript()
    {
        _script.DoString(LuaScript);
    }
}
