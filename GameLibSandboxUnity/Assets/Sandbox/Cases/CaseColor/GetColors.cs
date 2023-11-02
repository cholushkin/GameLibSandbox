using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using GameLib;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Assertions;

public class GetColors : MonoBehaviour
{
    public bool PrintInfo;
    public bool PrintHSL;
    public bool PrintBrights;
    public bool PrintDarks;
    public bool PrintSaturations;
    public bool PrintDesaturations;
    public bool PrintAnalogues;


    void Start()
    {
        PrintAllColors();
        PrintAnaloguesForColor(Color.red);
    }

    [Button()]
    void PrintAllColors()
    {
        var colorProperties = typeof(ColorUtils).GetProperties(BindingFlags.Public | BindingFlags.Static);

        int index = 0;
        foreach (var property in colorProperties)
        {
            if (property.PropertyType != typeof(Color)) 
                continue;
            Color color = (Color)property.GetValue(null, null);
            var info = $"BRT_N: {color.GetBrightnessNorm():F2}, LUM: {color.GetLuminance():F2}, Dark: {color.IsDark()}, ";
            var brights = $"Brighten: <color=#{color.Brighten().ToHexString()}>[0.1]</color><color=#{color.Brighten(0.2f).ToHexString()}>[0.2]</color><color=#{color.Brighten(0.4f).ToHexString()}>[0.4]</color><color=#{color.Brighten(0.75f).ToHexString()}>[0.75]</color><color=#{color.Brighten(1f).ToHexString()}>[1.0]</color>";
            var darks = $"Darken: <color=#{color.Darken().ToHexString()}>[0.1]</color><color=#{color.Darken(0.2f).ToHexString()}>[0.2]</color><color=#{color.Darken(0.4f).ToHexString()}>[0.4]</color><color=#{color.Darken(0.75f).ToHexString()}>[0.75]</color><color=#{color.Darken(1f).ToHexString()}>[1.0]</color>";
            var sats = $"Saturations: <color=#{color.Saturate().ToHexString()}>[0.1]</color><color=#{color.Saturate(0.2f).ToHexString()}>[0.2]</color><color=#{color.Saturate(0.4f).ToHexString()}>[0.4]</color><color=#{color.Saturate(0.75f).ToHexString()}>[0.75]</color><color=#{color.Saturate(1f).ToHexString()}>[1.0]</color>";
            var desats = $"Desaturations: <color=#{color.Desaturate().ToHexString()}>[0.1]</color><color=#{color.Desaturate(0.2f).ToHexString()}>[0.2]</color><color=#{color.Desaturate(0.4f).ToHexString()}>[0.4]</color><color=#{color.Desaturate(0.75f).ToHexString()}>[0.75]</color><color=#{color.Desaturate(1f).ToHexString()}>[1.0]</color>";

            var analoguesList = color.Analogous();
            var analogs = $"Analogues: <color=#{analoguesList[0].ToHexString()}>[0]</color><color=#{analoguesList[1].ToHexString()}>[1]</color><color=#{analoguesList[2].ToHexString()}>[2]</color><color=#{analoguesList[3].ToHexString()}>[3]</color><color=#{analoguesList[4].ToHexString()}>[4]</color><color=#{analoguesList[5].ToHexString()}>[5]</color>";


            Debug.Log($"{++index}. <color=#{color.ToHexString()}> {property.Name} </color>(Hex:#{color.ToHexString()}) " 
                      + (PrintHSL ? $"HSL:{color.ToHSL()}" : "")
                      + (PrintInfo ? info : "")
                      + (PrintSaturations ? sats : "")
                      + (PrintDesaturations ? desats : "")
                      + (PrintAnalogues ? analogs : "")
                      + (PrintBrights ? brights : "")
                      + (PrintDarks ? darks : ""));
        }
    }

    void PrintAnaloguesForColor(Color clr)
    {
        var analogous = clr.Analogous();
        foreach (var analog in analogous)
        {
            Debug.Log($"{analog.ToHexString()}");
        }


    }

    [Button()]
    void TestHSLToRgb()
    {
        var clr = new Color(0.628f, 0.643f, 0.142f, 1f);
        Assert.IsTrue(clr.ToHexString(true) == "A0A424FF", $"should be A0A424FF {clr.ToHexString(true)}");

        //hsv: { h: "61.8", s: "0.779", v: "0.643" },
        //hsl: { h: "61.8", s: "0.638", l: "0.393" },

        {
            var color = Color.red;
            var anl = color.Analogous();
            print($"{anl[0].ToHexString()} {anl[1].ToHexString()} {anl[2].ToHexString()} {anl[3].ToHexString()} {anl[4].ToHexString()} {anl[5].ToHexString()}");
        }
        print(clr.ToHSL());
        print(ColorUtils.RGBtoHSL(clr));

    }
}
