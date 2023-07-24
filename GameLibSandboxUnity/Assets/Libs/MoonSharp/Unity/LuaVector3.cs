using System.Text;
using MoonSharp.Interpreter;
using UnityEngine;

// To register a new type:
// 1. Create wrapper for your type(proxy class) like LuaVector3
// 2. Call YourType.RegisterWrapperType on your lua virtual machine (Script) 
// 3. Add call inside UnityWrapperTypes.RegisterUnityWrapperTypes() for your type
// 4. Add conversion to RegisterScriptToClrCustomConversion

namespace MoonSharp.UnityWrapper
{
    [MoonSharpUserData]
    public class LuaVector3
    {
        private Vector3 _vector;

        public static readonly LuaVector3 One = new LuaVector3(1f,1f,1f);

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

            // Register Vector3 constants
            script.DoString("Vector3.One = Vector3(1.0, 1.0, 1.0)");
            script.DoString("Vector3.Zero = Vector3(0.0, 0.0, 0.0)");
            script.DoString("Vector3.NegativeInfinity = Vector3(-math.huge,-math.huge,-math.huge)");
            script.DoString("Vector3.PositiveInfinity = Vector3(math.huge,math.huge,math.huge)");
            script.DoString("Vector3.Down = Vector3(0.0, -1.0, 0.0)");
            script.DoString("Vector3.Forward = Vector3(0.0, 0.0, 1.0)");
            script.DoString("Vector3.Left = Vector3(-1.0, 0.0, 0.0)");
            script.DoString("Vector3.Right = Vector3(1.0, 0.0, 0.0)");
            script.DoString("Vector3.Up = Vector3(0.0, 1.0, 0.0)");
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
