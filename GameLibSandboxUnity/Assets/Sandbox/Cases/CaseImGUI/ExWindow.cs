using ImGuiNET;
using UImGui;
using UnityEngine;

public class ExWindow : MonoBehaviour
{
    public Color LabelColor;
    public int ID;
    public Vector2 Position;

    private void OnEnable()
    {
        UImGuiUtility.Layout += OnLayout;
    }

    private void OnDisable()
    {
        UImGuiUtility.Layout -= OnLayout;
    }

    private void OnLayout(UImGui.UImGui uImGui)
    {
        ImGui.SetNextWindowSize(Vector2.one * 300, ImGuiCond.Once);
        ImGui.SetNextWindowPos(Position,ImGuiCond.Once);
        if (ImGui.Begin($"ExWindow##{ID}")) // Use "##" to pass a complement to the ID that won't be visible to the end-user https://github.com/ocornut/imgui/blob/master/docs/FAQ.md#q-about-the-id-stack-system
        {
            ImGui.Text("Hello world"); ImGui.SameLine(); ImGUIExtension.HelpMarker("Tooltip example text");
            ImGui.TextColored(LabelColor, "Hello");
            ImGui.End();
        }
    }
}

