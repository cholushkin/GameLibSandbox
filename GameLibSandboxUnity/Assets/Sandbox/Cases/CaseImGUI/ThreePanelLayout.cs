using System;
using ImGuiNET;
using UImGui;
using UnityEngine;
using UnityEngine.Assertions;

public class ThreePanelLayout : MonoBehaviour
{
    public enum State
    {
        Prompt,
        Images
    }

    [Serializable]
    public class StateNode
    {
        public enum Mode
        {
            FourTextures,
            Loading,
            Prompt
        }
        public string Caption;
        public Texture2D[] SubquadTextures;
        public int Selector;
        public Mode CurrentMode;
    }

    [Serializable]
    public class GeneratorButton
    {
        public string Name;
        public bool Enabled;
    }

    [Serializable]
    public class SubquadState
    {
        public Texture2D Texture;
        public bool Loading;
        public string Model;
        public int SecondsGenerated;
        public int Seed;
        public bool Error;
        public string ErrorText;
    }


    ImGuiWindowFlags MainFrameFlags = ImGuiWindowFlags.NoDecoration | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoSavedSettings;

    [Range(0f, 1f)] public float Panel1Div;
    [Range(0f, 1f)] public float Panel2Div;
    [Range(0f, 1f)] public float Panel3Div;

    [Header("Left panel")]
    public StateNode[] StateNodes;
    public int Selection;
    public Color SelectionColor;

    [Header("Middle panel")]
    public SubquadState[] SubquadStates;
    public Texture2D RefreshButtonTexture;
    public string PromptText = "";
    public State CurrentState;

    [Header("Right panel")]
    public bool DebugInfo;

    public Color GeneratorEnabledButtonColor;
    public Color GeneratorDisabledButtonColor;

    public GeneratorButton[] Txt2ImgGenerators;
    public GeneratorButton[] Img2ImgGenerators;

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
        Show3Panels();
    }

    void Show3Panels()
    {
        var viewport = ImGui.GetMainViewport();
        ImGui.SetNextWindowPos(viewport.Pos);
        ImGui.SetNextWindowSize(viewport.Size);

        bool open = true;
        if (ImGui.Begin("Fullscreen window", ref open, MainFrameFlags))
        {
            var totalWidth = ImGui.GetWindowSize().x;
            var totalHeight = ImGui.GetWindowSize().y;

            // Child 1
            {
                ImGuiWindowFlags window_flags = ImGuiWindowFlags.HorizontalScrollbar;
                ImGui.BeginChild("ChildL", new Vector2(totalWidth * Panel1Div, totalHeight), true, window_flags);
                LeftPanel();
                ImGui.EndChild();
            }
            ImGui.SameLine();

            // Child 2
            {

                ImGuiWindowFlags window_flags = ImGuiWindowFlags.None | ImGuiWindowFlags.NoScrollbar;
                ImGui.PushStyleVar(ImGuiStyleVar.ChildRounding, 5.0f);
                ImGui.BeginChild("ChildM", new Vector2(totalWidth * Panel2Div, totalHeight), false, window_flags);
                if (CurrentState == State.Images)
                    MiddlePanelImages();
                else if (CurrentState == State.Prompt)
                    MiddlePanelPrompt();
                ImGui.EndChild();
                ImGui.PopStyleVar();
            }
            ImGui.SameLine();

            // Child 3
            {
                ImGuiWindowFlags window_flags = ImGuiWindowFlags.None;
                ImGui.BeginChild("ChildR", new Vector2(totalWidth * Panel3Div, totalHeight), true, window_flags);
                RightPanel();
                ImGui.EndChild();
            }
            ImGui.End();
        }
    }

    void LeftPanel()
    {
        void DrawStateNode(StateNode stateNode)
        {
            Assert.IsNotNull(stateNode);
            if (stateNode.CurrentMode == StateNode.Mode.FourTextures)
            {
                for (int j = 0; j < stateNode.SubquadTextures.Length; ++j)
                {
                    System.IntPtr texID = UImGuiUtility.GetTextureId(stateNode.SubquadTextures[j]);
                    var selScale = stateNode.Selector == j ? 1.35f : 1.0f;
                    ImGui.Image(texID, Vector2.one * 64 * selScale);
                    ImGui.SameLine();
                }
            }
            else if (stateNode.CurrentMode == StateNode.Mode.Prompt)
            {
                ImGui.Button("[Prompt]");
            }
            else if (stateNode.CurrentMode == StateNode.Mode.Loading)
            {
                ImGui.Text(CookLoadingString("Generating{0}"));
            }
        }
        var reqionWidth = ImGui.GetContentRegionAvail().x;
        for (int i = 0; i < StateNodes.Length; ++i)
        {
            var stateNode = StateNodes[i];
            Assert.IsNotNull(stateNode);
            if (stateNode != null)
            {
                if (Selection == i)
                    ImGui.PushStyleColor(ImGuiCol.ChildBg, SelectionColor);
                ImGui.BeginChild($"xxx{i}", new Vector2(reqionWidth, 136), true);
                if (ImGui.IsWindowHovered() && ImGui.IsMouseClicked(ImGuiMouseButton.Left))
                {
                    print($"{i} clicked");
                }
                ImGui.Indent();
                ImGui.Text(stateNode.Caption);
                ImGui.Unindent();
                DrawStateNode(stateNode);
                ImGui.EndChild();
                if (Selection == i)
                    ImGui.PopStyleColor();
                ImGui.NewLine();
            }
        }
    }



    void MiddlePanelPrompt()
    {
        var totalWidth = ImGui.GetContentRegionAvail().x;
        ImGui.BeginChild("ChildMM", new Vector2(totalWidth, totalWidth), false);
        ImGui.InputTextMultiline("", ref PromptText, 1024, Vector2.one * (totalWidth));
        ImGui.EndChild();

        ImGui.NewLine();

        ImGui.SetCursorPosX((totalWidth - 220) * 0.5f);
        ImGui.Button("Imagine!", new Vector2(240, 60));
        ImGui.SameLine();

        ImGui.SetCursorPosX((totalWidth - 60) - 15);
        System.IntPtr texID = UImGuiUtility.GetTextureId(RefreshButtonTexture);
        ImGui.ImageButton(texID, new Vector2(60, 60));
    }

    private float[] tmpf = { 0, 0, 0, 0f };
    private int[] tmpi = { 0, 0, 0, 0 };
    void MiddlePanelImages()
    {
        bool Popup(int subQuadIndex)
        {
            var subQuadState = SubquadStates[subQuadIndex];
            if (subQuadState == null)
                return false;

            if (subQuadState.Loading)
                return false;

            if (ImGui.BeginPopupContextItem($"sub-quad {subQuadIndex} popup"))
            {
                ImGui.Indent();
                ImGui.TextDisabled($"Model: {subQuadState.Model}");
                ImGui.TextDisabled($"Generated in {subQuadState.SecondsGenerated} seconds");
                if(subQuadState.Error)
                    ImGui.TextColored(Color.red, $"Error: {subQuadState.ErrorText}");
                if (ImGui.Selectable($"Regenerate Image {subQuadIndex}"))
                {

                }
                ImGui.SameLine();
                ImGUIExtension.HelpMarker($"Regenerate Image {subQuadIndex} with new randomized parameters");

                if (ImGui.Selectable($"Save Image {subQuadIndex}"))
                {

                }
                ImGui.SameLine();
                ImGUIExtension.HelpMarker("Saves the image in format Gen.xx image.yy and add meta to output.csv of the current session"); // todo: provide a real name here

                if (ImGui.Selectable($"Save Image {subQuadIndex} as..."))
                {

                }
                ImGui.SameLine();
                ImGUIExtension.HelpMarker("Saves image with provided name (output.csv is ignored)");

                if (ImGui.Selectable($"Change prompt for Image {subQuadIndex}"))
                {

                }
                ImGui.SameLine();
                ImGUIExtension.HelpMarker($"Modify prompt for sub-quad {subQuadIndex}");

                // Parameters
                ImGui.TextDisabled("Noise:");
                ImGui.SameLine();
                ImGui.DragFloat($"##{subQuadIndex}", ref tmpf[subQuadIndex], 0.01f, 0.0f, 1.0f);
                ImGui.SameLine();
                ImGUIExtension.HelpMarker("Denoising parameter: 0 - no changes, 1 - completely new image");


                ImGui.TextDisabled("Seed:");
                ImGui.SameLine();
                ImGui.DragInt($"###{subQuadIndex}", ref tmpi[subQuadIndex], 1, 0, 0);

                ImGui.Unindent();
                ImGui.EndPopup();
                return true;
            }
            return false;
        }
        // Draw caption
        // todo: move to helper centered text
        var captionText = "Generation 0";
        var windowWidth = ImGui.GetContentRegionAvail().x;
        var textWidth = ImGui.CalcTextSize(captionText).x;
        ImGui.SetCursorPosX((windowWidth - textWidth) * 0.5f);
        ImGui.TextColored(Color.blue, captionText);

        // Draw images
        var reqionWidth = ImGui.GetContentRegionAvail().x;

        for (int i = 0; i < SubquadStates.Length; i++)
        {
            var subQuadState = SubquadStates[i];
            Assert.IsNotNull(subQuadState);
            if (subQuadState.Loading)
            {
                if (ImGui.BeginChild($"subquadChild{i}", Vector2.one * reqionWidth * 0.5f))
                {
                    var childSize = ImGui.GetContentRegionAvail();
                    var textSize = ImGui.CalcTextSize("Generating");
                    ImGui.SetCursorPos((childSize - textSize) * 0.5f);
                    ImGui.Text( CookLoadingString("Generating{0}"));
                    ImGui.EndChild();
                }
            }
            else if (subQuadState.Texture)
            {
                System.IntPtr texID = UImGuiUtility.GetTextureId(subQuadState.Texture);
                ImGui.Image(texID, Vector2.one * reqionWidth * 0.5f);
            }

            if (Popup(i)) { }
            else if (ImGui.IsItemClicked())
            {
                print("clicked " + i);
            }
            if (i % 2 == 0)
                ImGui.SameLine();

        }
    }


    private float tmpf1, tmpf2;
    private Vector2 tmpv2;
    private int imin, imax;
    void RightPanel()
    {
        void DrawGeneratorsSection(string caption, GeneratorButton[] buttons)
        {
            Vector2 buttonSize = new Vector2(120, 80);

            ImGui.TextColored(Color.gray, caption);
            int buttons_count = buttons.Length;
            var style = ImGui.GetStyle();
            float window_visible_x2 = ImGui.GetWindowPos().x + ImGui.GetWindowContentRegionMax().x;
            for (int i = 0; i < buttons_count; i++)
            {
                var buttonDescriptor = buttons[i];
                ImGui.PushID(i);

                ImGui.PushStyleColor(ImGuiCol.Button, buttonDescriptor.Enabled ? GeneratorEnabledButtonColor : GeneratorDisabledButtonColor);
                ImGui.PushStyleColor(ImGuiCol.Text, buttonDescriptor.Enabled ? Color.white : Color.gray);

                ImGui.Button(buttonDescriptor.Name, buttonSize);
                float last_button_x2 = ImGui.GetItemRectMax().x;
                float next_button_x2 = last_button_x2 + style.ItemSpacing.x + buttonSize.x; // Expected position if next button was on same line
                if (i + 1 < buttons_count && next_button_x2 < window_visible_x2 && i != buttons_count - 1)
                    ImGui.SameLine();

                ImGui.PopStyleColor(2);
                ImGui.PopID();
            }
        }

        void DrawSubQuadSettings(int id)
        {
            var width = ImGui.GetContentRegionAvail().x;
            if (ImGui.TreeNode($"Sub-quad {id}"))
            {
                ImGui.TextDisabled($"Denoising randomize range: [{tmpf1}..{tmpf2}]");
                ImGui.SameLine();
                ImGUIExtension.HelpMarker("sadasdsa");
                ImGui.DragFloatRange2("", ref tmpf1, ref tmpf2, 1f);
                
                ImGui.TextDisabled($"Seed randomize range: [{imin}..{imax}]");
                ImGui.SameLine();
                ImGUIExtension.HelpMarker("sadasdsa");
                ImGui.DragIntRange2("",  ref imin, ref imax, 1);

                ImGui.TextDisabled($"Steps randomize range: [{imin}..{imax}]");
                ImGui.SameLine();
                ImGUIExtension.HelpMarker("sadasdsa");
                ImGui.DragIntRange2("", ref imin, ref imax, 1);

                ImGui.TreePop();
            }
        }

        if (DebugInfo && ImGui.TreeNode("Dev info:"))
        {

            ImGui.TextDisabled("Session #: ");
            ImGui.TextDisabled("Textures generated: 00/00 "); ImGui.SameLine(); ImGUIExtension.HelpMarker("Textures generated this session/overall");
            ImGui.TreePop();
            ImGui.NewLine();
        }

        DrawGeneratorsSection("Txt2Img generators:", Txt2ImgGenerators);
        ImGui.NewLine();
        DrawGeneratorsSection("Img2Img generators:", Img2ImgGenerators);
        ImGui.NewLine();

        ImGui.TextColored(Color.gray, "Commands:");
        ImGui.Button("Save Tree");
        ImGui.SameLine();
        ImGUIExtension.HelpMarker("save");
        ImGui.Button("Show info");
        ImGui.SameLine();
        ImGUIExtension.HelpMarker("show info");
        ImGui.Button("Restart session");
        ImGui.SameLine();
        ImGUIExtension.HelpMarker("restart");

        ImGui.NewLine();
        ImGui.TextColored(Color.gray, "Settings:");
        DrawSubQuadSettings(0);
        DrawSubQuadSettings(1);
        DrawSubQuadSettings(2);
        DrawSubQuadSettings(3);
    }

    private readonly string[] _dots = {"", ".", "..", "..."};
    string CookLoadingString(string formatString)
    {
        return string.Format(formatString, _dots[(int) (Time.time * 4.0f % 4)]);
    }
}
