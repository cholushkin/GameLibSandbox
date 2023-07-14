using ImGuiNET;
using UImGui;
using UnityEngine;

public class ExWindow : MonoBehaviour
{
    public Color LabelColor;
    public int ID;
    public Vector2 InitialPosition;
    public bool SetInitialPosition;

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
        if(SetInitialPosition)
			ImGui.SetNextWindowPos(InitialPosition, ImGuiCond.Once);
        if (ImGui.Begin($"ExWindow##{ID}")) // Use "##" to pass a complement to the ID that won't be visible to the end-user https://github.com/ocornut/imgui/blob/master/docs/FAQ.md#q-about-the-id-stack-system
        {
            ImGui.Text("Hello world"); ImGui.SameLine(); ImGUIExtension.HelpMarker("Tooltip example text");
            ImGui.TextColored(LabelColor, "Hello");
            if (ImGui.Button("Print window info"))
            {
                Debug.Log($"My pos: {ImGui.GetWindowPos()}");
			}
            ImGui.End();
        }
    }
}

