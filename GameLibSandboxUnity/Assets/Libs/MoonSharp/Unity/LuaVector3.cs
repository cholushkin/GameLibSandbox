using System.Text;
using MoonSharp.Interpreter;
using UnityEngine;

// To register a new type:
// 1. Create wrapper for your type(proxy class) like LuaVector3
// 2. Call YourType.RegisterWrapperType on your lua virtual machine (Script) 
// 3. Add call inside UnityWrapperTypes.RegisterUnityWrapperTypes() for your type
// 4. Add convertion to RegisterScriptToClrCustomConversion

namespace MoonSharp.UnityWrapper
{
    [MoonSharpUserData]
    public class LuaVector3
    {
        private Vector3 _vector;

        public LuaVector3(float x = 0f, float y = 0f, float z = 0f)
        {
            _vector = new Vector3(x, y, z);
        }

        public float x
        {
            get => _vector.x;
            set => _vector.x = value;
        }

        public float y
        {
            get => _vector.y;
            set => _vector.y = value;
        }

        public float z
        {
            get => _vector.z;
            set => _vector.z = value;
        }

        public float Magnitude()
        {
            return _vector.magnitude;
        }

        [MoonSharpHidden]
        public Vector3 ToVector3()
        {
            return _vector;
        }

        [MoonSharpHidden]
        public static LuaVector3 FromVector3(Vector3 v)
        {
            return new LuaVector3(v.x, v.y, v.z);
        }

        [MoonSharpHidden]
        public static void RegisterWrapperType(Script script)
        {
            LuaWrapperHelper.RegisterWrapperType<LuaVector3>(script, "Vector3", "LuaVector3");
        }
    }

    // todo: separate file
    public static class LuaWrapperHelper
    {
        public static void RegisterWrapperType<T>(Script script, string luaTypeName, string clrWrapperName)
        {
            // todo: check free name for type 
            script.Globals[clrWrapperName] = typeof(T);

            StringBuilder sb = new StringBuilder(512);

            sb.Append($"{luaTypeName} = ");
            sb.Append("{} local metaTableForWrapperTable = { __call = function(t, ...) ");
            sb.Append($"local instance = {clrWrapperName}.__new(...)");
            sb.Append("return instance end }");
            sb.Append($"setmetatable({luaTypeName}, metaTableForWrapperTable)");
            script.DoString(sb.ToString());
        }
    }
}
