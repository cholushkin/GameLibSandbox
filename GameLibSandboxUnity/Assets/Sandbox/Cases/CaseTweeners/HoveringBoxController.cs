using GameLib;
using UnityEngine;
using UnityEngine.InputSystem;

public class HoveringBoxController : MonoBehaviour
{
    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            var push = gameObject.AddComponent<Push>();
            push.PunchDirection = RandomUnitVector().ToVector3(0f);
        }
    }

    public Vector2 RandomUnitVector()
    {
        float random = Random.Range(0f, 260f);
        return new Vector2(Mathf.Cos(random), Mathf.Sin(random));
    }
}
