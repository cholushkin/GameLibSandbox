using System.Runtime.InteropServices;
using UnityEngine;

public class RandomlyPrintMessages : MonoBehaviour
{
    public float MessageDelay;
    public int StopAfter = 200;
    private float _curDelay;
    private string[] _messages = {
        "Fast rendering and more responsive input using uimgui",
        "Lua scripting. You can register commands to the console. After that you can use them for direct call from the console or from the script file, you can use them in cycles and so on Variables registration",
        "Alias names for function and variables",
        "Advanced log filtering using LogManager"};


    void Update()
    {
        if (StopAfter <= 0)
            return;

        _curDelay += Time.deltaTime;
        if (_curDelay > MessageDelay)
        {
            var index = Random.Range(0, 5);
            if (index == 0)
                Debug.LogError(_messages[Random.Range(0, _messages.Length)]);
            else if (index == 1)
                Debug.LogAssertion(_messages[Random.Range(0, _messages.Length)]);
            else if (index == 2)
                Debug.LogWarning(_messages[Random.Range(0, _messages.Length)]);
            else if (index == 3)
                Debug.Log(_messages[Random.Range(0, _messages.Length)]);
            else if (index == 4)
                Debug.LogException(new ExternalException("test exception"));

            _curDelay = 0f;
            StopAfter--;
        }

    }
}
