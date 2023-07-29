using MoonSharp.Interpreter;
using Qonsole;
using UnityEngine.Assertions;

public class TestScenarioQonsoleGeneral : TestScenario
{
    public override void Execute()
    {
        TestAccessOfInternalMethods();
    }

    protected void TestAccessOfInternalMethods()
    {
        var lua = WidgetQonsoleController.Instance.Script;
        Assert.IsNotNull(lua, "WidgetQonsoleController.Instance.Script is always available");

        Assert.IsTrue(IsLuaFunctionAvailableGlobally("PrintTable"));
        Assert.IsFalse(IsLuaFunctionAvailableGlobally("PrintTable2"));
        Assert.IsTrue(IsLuaFunctionAvailableGlobally("SplitString"));
        Assert.IsFalse(IsLuaFunctionAvailableGlobally("splitstring"), "Lua is sensetive to register of the letters in names");
    }

    private bool IsLuaFunctionAvailableGlobally(string functionName)
    {
        var lua = WidgetQonsoleController.Instance.Script;
        Assert.IsNotNull(lua, "WidgetQonsoleController.Instance.Script is always available");

        DynValue dVal = lua.Globals.Get(functionName);
        Assert.IsNotNull(dVal, "Always has a DynValue");

        return dVal.Type == DataType.Function;
    }
}
