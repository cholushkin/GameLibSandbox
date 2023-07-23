using System.Drawing.Printing;
using MoonSharp.Interpreter;
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
            Script.GlobalOptions.CustomConverters.SetScriptToClrCustomConversion(DataType.Table, typeof(Color),
                v => new Color(1f, 0, 0, 0));

            Script.GlobalOptions.CustomConverters.SetScriptToClrCustomConversion(DataType.String, typeof(Color), dv => ColorLiteralToColor(dv));
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
            
            if(clrName == "red")
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
    