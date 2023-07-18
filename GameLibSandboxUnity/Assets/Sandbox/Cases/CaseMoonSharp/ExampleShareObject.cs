using MoonSharp.Interpreter;
using NaughtyAttributes;
using UnityEngine;




public class ExampleShareObject : MonoBehaviour
{
    private Script _script;
    [ResizableTextArea]
    public string LuaScript = @"    
		zombie1.SayHi()
        zombie2.SayHi()

        print('zombie 1 '..zombie1.Health)
        zombie1.Health = 999
        print('zombie 1 '..zombie1.Health)
        print('zombie 1 '..zombie1.Health)
        print(type(ExampleShareObject.Zombie))
	";

    [MoonSharpUserData]
    class Zombie
    {
        public float Health;
        public string Name;
        public static int ZombieCounter;

        public Zombie(string name = "nameless", float health = 10f)
        {
            Name = name;
            Health = health;
            ZombieCounter++;
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

        Debug.Log("Registered types:");
        var registeredTypes = UserData.GetRegisteredTypes();
        foreach (var registeredType in registeredTypes)
        {
            Debug.Log(registeredType);
        }

    }

    [Button]
    void RunScript()
    {
        _script.DoString(LuaScript);
    }
}
