//using UnityEngine.Assertions;

//public class DbgCaseCaption : Pane
//{
//    public CasesManager CaseManager;
//    private int _curCaseIndex;
//    public override void InitializeState()
//    {
//        base.InitializeState();
//        Assert.IsNotNull(CaseManager);
//        DisableButton();
//        UpdateText();
//    }

//    private void UpdateText()
//    {
//        SetText($"<b>[{CaseManager.GetCurrentCaseName()}]</b>\n{CaseManager.CurrentCaseIndex} of {CaseManager.GetCasesCount()-1}");
//    }

//    void Update()
//    {
//        if (CaseManager.CurrentCaseIndex != _curCaseIndex)
//        {
//            _curCaseIndex = CaseManager.CurrentCaseIndex;
//            UpdateText();
//        }
//    }
//}
