//using UnityEngine.Assertions;

//public class DbgNextCase : Pane
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
//        CaseManager.IncIndex();
//        CaseManager.RunCase();
//    }

//    private void UpdateText()
//    {
//        SetText($"Next\n<b>[{CaseManager.GetNextCaseName()}]</b>");
//    }
//}
