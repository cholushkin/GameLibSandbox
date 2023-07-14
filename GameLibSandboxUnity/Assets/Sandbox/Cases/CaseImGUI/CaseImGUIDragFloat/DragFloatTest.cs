using ImGuiNET;
using UImGui;
using UnityEngine;

public class DragFloatTest : MonoBehaviour
{
    public Vector2 Position;
    public float a;
    public float b;
    public Vector2 f2;
    public Vector4 f4;

    public Vector2Int f2i;

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
        ImGui.SetNextWindowSize(Vector2.one * 600, ImGuiCond.Once);
        ImGui.SetNextWindowPos(Position, ImGuiCond.Once);
        if (ImGui.Begin("ExWindow"))
        {
            ImGui.DragFloat("float a", ref a, 0.1f, 0f, 10f);
            ImGui.DragFloat("float b", ref b);

            ImGui.DragFloat2("f2", ref f2);

            //ImGui.DragInt2("i2 slider", ref f2i);



            ImGui.End();
        }
    }
}
