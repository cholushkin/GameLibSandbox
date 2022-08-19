using GameLib.Random;
using NCalc;
using UnityEngine;
using UnityEngine.Assertions;

public class TestScenarioCustomFunctionCall : TestScenario
{
    public string Expression;
    public string ExpectedResult;
    public int RandomSeed;
    private IPseudoRandomNumberGenerator _rnd;

    public override void Execute()
    {
        _rnd = RandomHelper.CreateRandomNumberGenerator(RandomSeed, RandomHelper.PseudoRandomNumberGenerator.LinearCongruential);
        var e = new Expression(Expression);

        e.EvaluateFunction += delegate (string name, FunctionArgs args)
        {
            if (name == "RandomPrize")
                args.Result = RandomPrize(
                    (int) args.Parameters[0].Evaluate(),
                    (int) args.Parameters[1].Evaluate(),
                    (int) args.Parameters[2].Evaluate());

            if (name == "SpecificPrize")
                args.Result = SpecificPrize(args.Parameters);

            if (name == "GiveRandomIndex")
                args.Result = GiveRandomIndex();
        };

        var result = e.Evaluate();


        if (Log.Verbose())
        {
            Debug.LogFormat("Expression '{0}' result is: {1}", Expression, result);
            Debug.LogFormat("Prizes win: {0} {1}. Seed: {2} ", PrizeResult, PrizeResultIndexes, RandomSeed);
        }
        Assert.IsTrue(ExpectedResult == PrizeResultIndexes);
    }


    private string[] PrizeProvider = { "Coin", "Toy", "Teddy bear", "Candy" };
    private string PrizeResult;
    private string PrizeResultIndexes;


    // first param is how many prizes to give, second and third is from which range
    private string RandomPrize(int howManyPrizesToGive, int fromRange, int toRange)
    {
        var PrizeRangeIndexes = new Range(fromRange,toRange);

        
        for (int i = 0; i < howManyPrizesToGive; i++)
            GivePrize(_rnd.FromRangeIntInclusive(PrizeRangeIndexes));
        return PrizeResult;
    }

    // just give a specific prizes by indexes
    private string SpecificPrize(Expression[] indexes)
    {
        foreach (var expression in indexes)
        {
            GivePrize((int)expression.Evaluate());
        }
        
        return "0";
    }

    private int GiveRandomIndex()
    {
        var rndIndex = _rnd.FromRangeIntInclusive(0, PrizeProvider.Length - 1);
        return rndIndex;
    }

    private void GivePrize(int index)
    {
        var prize = string.Format("'{0}' ", PrizeProvider[index]);
        PrizeResult += prize;
        PrizeResultIndexes += index.ToString();
    }
}
