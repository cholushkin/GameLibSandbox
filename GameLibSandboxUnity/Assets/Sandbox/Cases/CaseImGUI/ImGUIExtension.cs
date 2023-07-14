using System.Collections;
using System.Collections.Generic;
using ImGuiNET;
using UnityEngine;

static class ImGUIExtension
{
    //static private Vector4 _v4Tmp = Vector4.one;

    public static void HelpMarker(string description)
    {
        ImGui.TextDisabled("[?]");
        if (ImGui.IsItemHovered())
        {
            ImGui.BeginTooltip();
            ImGui.PushTextWrapPos(ImGui.GetFontSize()* 35.0f);
            ImGui.TextUnformatted(description);
            ImGui.PopTextWrapPos();
            ImGui.EndTooltip();
        }
    }


}
