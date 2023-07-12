using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using XLua.LuaDLL;

public class ExampleRegisterCSFunction : MonoBehaviour
{
    // Example method
    static int MyHi(IntPtr L)
    {
        Debug.Log("Hi");
        return 0; // number of results
    }

    static int MySin(IntPtr L)
    {
        double d = Lua.lua_tonumber(L, 1);  /* get argument */
        Lua.lua_pushnumber(L, Mathf.Sin((float)d));  /* push result */
        return 1;  /* number of results */
    }



    void Start()
    {
        var luaEnv = LuaEnvSingleton.Instance.LuaEnv;
        Lua.lua_pushstdcallcfunction(luaEnv.L, MyHi);
        Lua.xlua_setglobal(luaEnv.L, "MyHi");

        Lua.lua_pushstdcallcfunction(luaEnv.L, MySin);
        Lua.xlua_setglobal(luaEnv.L, "MySin");

        luaEnv.DoString("MyHi()");
        luaEnv.DoString("print( MySin() )");
        luaEnv.DoString(@"print(CS.UnityEngine.Color.red)");
    }
}
