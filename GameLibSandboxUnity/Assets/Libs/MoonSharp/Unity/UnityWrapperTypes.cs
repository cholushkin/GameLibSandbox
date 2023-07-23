using MoonSharp.Interpreter;
using MoonSharp.UnityWrapper;

public static class UnityWrapperTypes 
{
    public static void RegisterUnityWrapperTypes()
    {
        UserData.RegisterType<LuaVector3>();
    }
}
