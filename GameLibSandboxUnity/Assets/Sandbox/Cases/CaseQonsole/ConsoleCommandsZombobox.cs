#define TEST_REGISTRATION_ERRORS_disabled

using UnityEngine;



namespace Qonsole
{

    public static class ConsoleVariablesZombobox
    {
        //[ConsoleVariable("Zombobox.MaxZombies", "maxz", "Maximum amount of zombies possible")]
        //public static int MaxZombies { get; set; } = 10;

        //[ConsoleVariable("Zombobox.MinZombies", "minz", "Minimum amount of zombies possible")]
        //public static int MinZombies { get; set; } = 1;

    }

    public static class ConsoleCommandsZombobox
    {
        public enum Weapon
        {
            Gun,
            Shotgun,
            Rifle
        }

        
        // Example of: no description
        // Example of: passing enum (in lua: Weapon.Gun )
        [ConsoleMethod("Zombobox.PrintWeapon", "weapon", null), UnityEngine.Scripting.Preserve]
        public static void PrintWeapon(Weapon w)
        {
            if(w == Weapon.Gun)
                Debug.Log("gun");
            else if (w == Weapon.Shotgun)
                Debug.Log("shotgun");
            else if (w == Weapon.Rifle)
                Debug.Log("rifle");
        }


        [ConsoleMethod("Zombobox.Hit", "hit", "Hit the zombie")]
        // Example of: default value
        public static void HitZombie(string name, float hitPoint = 1f)
        {
            Debug.Log($"Hit zombie {name} by {hitPoint} hit points");
            var zombie = FindZombie(name);
            if (zombie == null)
            {
                return;
            }
            zombie.Health -= hitPoint;
        }


        [ConsoleMethod("Zombobox.CreateZombie", "cz", "Create zombie")]
        // Example of: default value
        public static void CreateZombie(string name, Vector3 pos, float health = 10f)
        {
            var gObj = new GameObject(name);
            gObj.transform.position = pos;
            var zombie = gObj.AddComponent<Zombie>();
            zombie.Health = health;
        }


        [ConsoleMethod("Zombobox.SetColor", "setcolor", "Set color to zombie")]
        public static void SetColor(string name, Color clr)
        {

        }

        [ConsoleMethod("Zombobox.PrintState", "printz", "Print the state of zombobox")]
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
