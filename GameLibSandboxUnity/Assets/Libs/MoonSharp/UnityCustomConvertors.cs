using MoonSharp.Interpreter;
using UnityEngine;

namespace MoonSharp
{
    public static class UnityCustomConvertors
    {
        public static void RegisterUnityCustomConvertors()
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


 

 

///// <summary>
/////   <para>Magenta. RGBA is (1, 0, 1, 1).</para>
///// </summary>
//public static Color magenta
//{
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    get => new Color(1f, 0.0f, 1f, 1f);
//}

///// <summary>
/////   <para>Gray. RGBA is (0.5, 0.5, 0.5, 1).</para>
///// </summary>
//public static Color gray
//{
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    get => new Color(0.5f, 0.5f, 0.5f, 1f);
//}

///// <summary>
/////   <para>English spelling for gray. RGBA is the same (0.5, 0.5, 0.5, 1).</para>
///// </summary>
//public static Color grey
//{
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    get => new Color(0.5f, 0.5f, 0.5f, 1f);
//}

///// <summary>
/////   <para>Completely transparent. RGBA is (0, 0, 0, 0).</para>
///// </summary>
//public static Color clear
//{
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    get => new Color(0.0f, 0.0f, 0.0f, 0.0f);
//}

///// <summary>
/////   <para>The grayscale value of the color. (Read Only)</para>
///// </summary>
//public float grayscale