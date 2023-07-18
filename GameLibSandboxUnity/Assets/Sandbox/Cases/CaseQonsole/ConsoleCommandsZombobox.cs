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
        [ConsoleMethod("Zombobox.CreateZombie", "sz", "Create zombie")]
        public static void CreateZombie(string name, Vector3 pos, float health = 10f)
        {
            var gObj = new GameObject(name);
            gObj.transform.position = pos;
            var zombie = gObj.AddComponent<Zombie>();
            zombie.Health = health;
        }

        [ConsoleMethod("Zombobox.SelectZombie", "selzb", "Select zombie by name", "Zombie name")]
        public static void SelectZombie(string name)
        {
        }

        [ConsoleMethod("Zombobox.PrintState", "printz", "Print the state of zombobox")]
        public static void PrintZomboboxState()
        {
            //Debug.Log($"MaxZombies={ConsoleVariablesZombobox.MaxZombies}");
            //Debug.Log($"MinZombies={ConsoleVariablesZombobox.MinZombies}");
        }


#if TEST_REGISTRATION_ERRORS
        [ConsoleMethod("Zombobox.CreateZombie", "sz", "Create zombie")]
        public static void CreateZombieE1()
        {
        }

        [ConsoleMethod("Zombobox.CreateZombie", "sz2", "Create zombie")]
        public static void CreateZombieE2()
        {
        }

        [ConsoleMethod("Zombobox.CreateZombie2", "sz", "Create zombie")]
        public static void CreateZombieE3()
        {
        }
#endif
    }
}
