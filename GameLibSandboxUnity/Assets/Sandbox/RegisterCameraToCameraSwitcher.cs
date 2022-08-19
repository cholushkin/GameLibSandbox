using UnityEngine;

public class RegisterCameraToCameraSwitcher: MonoBehaviour
{
    public bool MarkAsMain;
    public int Priority;

    void Start()
    {
        DbgNextCamera.GetStaticInstance().RegisterCamera(GetComponent<Camera>(), MarkAsMain);
    }
}
