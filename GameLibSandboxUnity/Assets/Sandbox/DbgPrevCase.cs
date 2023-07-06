//using UnityEngine.Assertions;

//public class DbgPrevCase : Pane
//{
//    public CasesManager CaseManager;
//    public override void InitializeState()
//    {
//        base.InitializeState();
//        Assert.IsNotNull(CaseManager);
//        UpdateText();
//    }

//    public override void OnClick()
//    {
//        CaseManager.DecIndex();
//        CaseManager.RunCase();
//    }

//    private void UpdateText()
//    {
//        SetText($"Prev\n<b>[{CaseManager.GetPrevCaseName()}]</b>");
//    }
//}