using System.Collections;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using NaughtyAttributes;
using UnityEngine;

public class ExampleRegisterClassExplicitly : MonoBehaviour
{
    [ResizableTextArea]
    public string LuaScript = @"    
        zombie1.SayHi()        
        zombie2.SayHi()
	";

    public static class LuaToColor
    {
        public static Color Get(Table color)
        {
            if (color.Length == 4)
            {
                return new Color(
                    (float)color.Get(1).Number,
                    (float)color.Get(2).Number,
                    (float)color.Get(3).Number,
                    (float)color.Get(4).Number);
            }
            else if (color.Length == 3)
            {
                return new Color(
                    (float)color.Get(1).Number,
                    (float)color.Get(2).Number,
                    (float)color.Get(3).Number,
                    1f);
            }
            else if (color.Length == 1)
            {
                return Get(color.Get(1).String);
            }
            else
            {
                Debug.LogError($"Color table has not supported format");
            }
            return Color.white;
        }

        public static Color Get(string colorName)
        {
            colorName = colorName.ToLower();
            if (colorName == "red")
                return Color.red;
            if (colorName == "green")
                return Color.green;
            return Color.white;
        }
    }

    // Note: no [MoonSharpUserData] needed anymore
    class Zombie
    {
        public enum ZType
        {
            Smelly,
            Rotten
        }
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

        // Note: you can use default values in Lua
        public void SayHi(string customText = null)
        {
            if(!string.IsNullOrEmpty(customText))
                Debug.Log($"Zombie {Name}: {customText}");
            else
                Debug.Log($"Zombie {Name}: Hi!");
        }

        public void Color(Table clr)
        {
            Debug.Log( LuaToColor.Get(clr).ToString());
        }

        public void Color(string clr)
        {
            Debug.Log(LuaToColor.Get(clr).ToString());
        }

        public void SetZType(ZType zType)
        {
            Debug.Log(zType);
        }
    }




    void Start()
    {
        // Register just Zombie, explicitly.
        UserData.RegisterType<Zombie>();
        UserData.RegisterType<Zombie.ZType>();

        LuaSingleton.Instance.Script.Globals["ZType"] = typeof(Zombie.ZType);

        // Pass an instance of the class to the script in a global
        LuaSingleton.Instance.Script.Globals["zombie1"] = new Zombie("Bill");

        // Create a userdata, explicitly.
        DynValue obj = UserData.Create(new Zombie("Bob"));
        LuaSingleton.Instance.Script.Globals.Set("zombie2", obj);

        
    }

    [Button]
    void RunScript()
    {
        LuaSingleton.Instance.Script.DoString(LuaScript);
    }
}
