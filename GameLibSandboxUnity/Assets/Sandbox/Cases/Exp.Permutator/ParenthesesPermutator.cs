using System.Collections.Generic;
using UnityEngine;

public class ParenthesesPermutator : MonoBehaviour
{
    public struct Node
    {
        public void Add(Node n)
        {
            if (Children == null)
                Children = new List<Node>();
            Children.Add(n);
        }
        public List<Node> Children;
    }

    public int N;
    private int _permutations;

    void Start()
    {
        var n = new Node();
        n.Add(new Node());
        _permutations = 0;
        if (N > 0)
            Step(n, N);
        Debug.LogFormat("For N = {0} there is {1} permutations", N, _permutations);
    }


    void Step(Node parent, int remainingNodes)
    {
        if (remainingNodes == 0)
        {
            Count();
            return;
        }

        for (int i = 0; i < parent.Children.Count; ++i)
        {
            var newParent = parent.Children[i];
            for (int j = 1; j <= remainingNodes; ++j)
            {
                var newNode = new Node();
                newParent.Add(newNode);
                Step(newParent, remainingNodes - j);
            }
        }
    }

    void Count()
    {
        _permutations++;
    }
}
