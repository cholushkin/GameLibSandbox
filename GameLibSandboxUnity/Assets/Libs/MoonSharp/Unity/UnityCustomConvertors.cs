using System.Drawing.Printing;
using MoonSharp.Interpreter;
using MoonSharp.UnityWrapper;
using UnityEngine;

namespace MoonSharp
{
    public static class UnityCustomConvertors
    {
        public static void RegisterCustomConvertors()
        {
            RegisterScriptToClrCustomConversion();
        }

        public static void RegisterScriptToClrCustomConversion()
        {
            // All conversions are from Lua to CS

            Script.GlobalOptions.CustomConverters.SetScriptToClrCustomConversion(DataType.Table, typeof(Color),
                v => new Color(1f, 0, 0, 0));

            // table -> Vector3
            Script.GlobalOptions.CustomConverters.SetScriptToClrCustomConversion(DataType.Table, typeof(Vector3), dv => TableToVector3(dv));

            // LuaVector3 -> Vector3
            Script.GlobalOptions.CustomConverters.SetScriptToClrCustomConversion(DataType.UserData, typeof(Vector3), dv => UserDataToVector3(dv));

            // string -> Color
            Script.GlobalOptions.CustomConverters.SetScriptToClrCustomConversion(DataType.String, typeof(Color), dv => ColorLiteralToColor(dv));
        }

        private static Vector3 TableToVector3(DynValue v3Table)
        {
            if (v3Table.Type != DataType.Table)
            {
                Debug.LogError($"Expecting table");
                return Vector3.zero;
            }
            var table = v3Table.Table;
            if (table.Length == 3 && table.Get(1).Type == DataType.Number
                                  && table.Get(2).Type == DataType.Number
                                  && table.Get(3).Type == DataType.Number)
                return new Vector3((float)table.Get(1).Number, (float)table.Get(2).Number, (float)table.Get(2).Number);
            if (table.Length == 2 && table.Get(1).Type == DataType.Number
                                  && table.Get(2).Type == DataType.Number)
                return new Vector3((float)table.Get(1).Number, (float)table.Get(2).Number, 0f);
            if (table.Length == 1 && table.Get(1).Type == DataType.Number)
                return new Vector3((float)table.Get(1).Number, 0f, 0f);
            if (table.Length == 0)
                return new Vector3(0f, 0f, 0f);
            Debug.LogError($"Wrong table format for representing Vector3 : {v3Table.Table}");
            return Vector3.zero;
        }

        private static Vector3 UserDataToVector3(DynValue usrData)
        {
            var ud = usrData.UserData;
            LuaVector3 lv3 = (LuaVector3) ud.Object;
            return lv3.ToVector3();
        }


        private static Color ColorLiteralToColor(DynValue dVal)
        {
            var clrName = dVal.String;
            if (string.IsNullOrEmpty(clrName))
            {
                Debug.LogError("Error converting color from string literal. Color value is empty");
                return Color.white;
            }
            clrName = clrName.Trim().ToLower();
            
            if (clrName == "red")
                return Color.red;
            if (clrName == "green")
                return Color.green;
            if (clrName == "blue")
                return Color.blue;
            if (clrName == "white")
                return Color.white;
            if (clrName == "black")
                return Color.black;
            if (clrName == "yellow")
                return Color.yellow;
            if (clrName == "cyan")
                return Color.cyan;
            if (clrName == "magenta")
                return Color.magenta;
            if (clrName == "gray" || clrName == "grey")
                return Color.gray;


            Debug.LogError($"Can't find color literal '{clrName}'");
            return Color.white;
        }
    }
}
    