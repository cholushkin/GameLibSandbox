using NaughtyAttributes;
using UnityEngine;


 

public class ExampleConsole : MonoBehaviour
{




    [Button]
    void SetScene()
    {
        //Qonsole.Qonsole.Instance.DoCommand("SpawnZombie('Bob', {0,0,0})");
        //Qonsole.Qonsole.Instance.DoCommand("SpawnZombie('Bill', {1,1,1}, 12)");
    }



}



namespace Qonsole
{
    public static partial class QonsoleCommands
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
    }
}
