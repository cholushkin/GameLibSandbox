#define TEST_REGISTRATION_ERRORS_disabled

using Qonsole;
using UnityEngine;

namespace Zombotron.Qonsole
{
    // Represents real game domain 
    public static class Zombotron
    {
        public enum Weapon
        {
            Gun,
            Shotgun,
            Rifle,
            Sword,
            Knife
        }

        // Zombie class is also part of domain

    }

    // Console Variables
    public static class ConsoleVariables
    {
        //[ConsoleVariable("Zombotron.MaxZombies", "maxz", "Maximum amount of zombies possible")]
        //public static int MaxZombies { get; set; } = 10;

        //[ConsoleVariable("Zombotron.MinZombies", "minz", "Minimum amount of zombies possible")]
        //public static int MinZombies { get; set; } = 1;

    }

    // Console commands
    public static class ConsoleCommands
    {
        // Example of: no description
        // Example of: passing enum (in lua: Weapon.Gun )
        [ConsoleMethod("Zombotron.PrintWeapon", "weapon", null), UnityEngine.Scripting.Preserve]
        public static void PrintWeapon(Zombotron.Weapon w)
        {
            if (w == Zombotron.Weapon.Gun)
                Debug.Log("gun");
            else if (w == Zombotron.Weapon.Shotgun)
                Debug.Log("shotgun");
            else if (w == Zombotron.Weapon.Rifle)
                Debug.Log("rifle");
        }


        [ConsoleMethod("Zombotron.Hit", "hit", "Hit the zombie")]
        // Example of: default value
        public static void HitZombie(string name, float hitPoint = 1f)
        {
            Debug.Log($"Hit zombie {name} by {hitPoint} hit points");
            var zombie = FindZombie(name);
            if (zombie == null)
            {
                Debug.LogError($"Can't find zombie with the name '{name}'");
                return;
            }
            zombie.Health -= hitPoint;
        }


        [ConsoleMethod("Zombotron.CreateZombie", "cz", "Create zombie")]
        // Example of: default value
        public static void CreateZombie(string name, Vector3 pos, float health = 10f)
        {
            var gObj = new GameObject(name);
            gObj.transform.position = pos;
            var zombie = gObj.AddComponent<Zombie>();
            zombie.Health = health;
        }


        [ConsoleMethod("Zombotron.SetColor", "setcolor", "Set color to zombie")]
        public static void SetColor(string name, Color clr)
        {

        }

        [ConsoleMethod("Zombotron.PrintState", "printz", "Print the state of zombobox")]
        public static void PrintZomboboxState()
        {
            //Debug.Log($"MaxZombies={ConsoleVariablesZombobox.MaxZombies}");
            //Debug.Log($"MinZombies={ConsoleVariablesZombobox.MinZombies}");
        }

        private static Zombie FindZombie(string name)
        {
            var gObj = GameObject.Find(name);
            if (gObj == null)
                return null;
            return gObj.GetComponent<Zombie>();
        }


#if TEST_REGISTRATION_ERRORS
        [ConsoleMethod("Zombobox.CreateZombie", "cz", "Create zombie")]
        public static void CreateZombieE1()
        {
        }

        [ConsoleMethod("Zombobox.CreateZombie", "cz2", "Create zombie")]
        public static void CreateZombieE2()
        {
        }

        [ConsoleMethod("Zombobox.CreateZombie2", "cz", "Create zombie")]
        public static void CreateZombieE3()
        {
        }
#endif
    }
}
