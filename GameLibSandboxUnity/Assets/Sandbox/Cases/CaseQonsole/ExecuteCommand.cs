using NaughtyAttributes;
using Qonsole;
using UnityEngine;

public class ExecuteCommand : MonoBehaviour
{
    [ResizableTextArea]
    public string Code;

    [Button]
    void Run()
    {
        WidgetQonsoleController.Instance.ExecuteString(Code);
    }
}
