using Alg;
using NaughtyAttributes;
using XLua;

public class LuaEnvSingleton : Singleton<LuaEnvSingleton>
{
    public LuaEnv LuaEnv { get; private set; }
#if UNITY_EDITOR
    public string TableName = "_G";
    public string String = "print(_G)";
#endif

    protected override void Awake()
    {
        base.Awake();
        LuaEnv = new LuaEnv();
    }

    void Update()
    {
        LuaEnv.Tick();
    }


    void OnDestroy()
    {
        LuaEnv.Dispose();
    }

#if UNITY_EDITOR
    [Button()]
    void PrintLuaTable()
    {
        LuaEnv.DoString($"for n in pairs({TableName}) do print(n) end");
    }

    [Button()]
    void DoString()
    {
        LuaEnv.DoString(String);
    }
#endif
}
