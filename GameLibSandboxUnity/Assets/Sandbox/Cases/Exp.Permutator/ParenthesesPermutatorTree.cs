using System.Collections.Generic;
using UnityEngine;

public class ParenthesesPermutatorTree : MonoBehaviour
{
    public class Node
    {
        public class Branch
        {
            public List<Node> Nodes;

            public void Add(Node node)
            {
                if(Nodes==null)
                    Nodes = new List<Node>();
                Nodes.Add(node);
            }
        }

        public Node(int id)
        {
            Id = id;
        }

        public void Add(Branch branch)
        {
            if(_branches == null)
                _branches = new List<Branch>();
            _branches.Add(branch);
        }

        public int GetBranchesCount()
        {
            return _branches.Count;
        }

        private List<Branch> _branches;
        public Node Parent;
        public int Id;
        public int ParentBranchIndex =-1;
    }

    public int N;
    private int _permutations;
    private int uidCounter;

    void Start()
    {
        uidCounter = 0;
        var n = new Node(uidCounter++);
        var b = new Node.Branch();
        b.Add(n);
        n.Add(b);
        _permutations = 0;
        if (N > 0)
            Step(n, N);
        Debug.LogFormat("For N = {0} there is {1} permutations", N, _permutations);
    }


    void Step(Node node, int remainingNodes)
    {
        if (remainingNodes == 0)
        {
            Count();
            Print(node);
            return;
        }

        for (int b = 0; b < node.GetBranchesCount(); ++b) // for each branch of current node
        {
            for (int r = 1; r <= remainingNodes; ++r) // for each node in the branch
            {
                var newNode = new Node(uidCounter++);
                newNode.Parent = node;
                newNode.ParentBranchIndex = b;

                for (int bb = 0; bb < r; ++bb) // create branches 
                {
                    var branch = new Node.Branch();
                    newNode.Add(branch);
                }
                Step(newNode, remainingNodes - r);
            }
        }
    }

    private void Print(Node node)
    {
        var result = "";
        var pointer = node;
        var branchIndex = -1;

        while (pointer != null)
        {
            if(pointer.ParentBranchIndex == -1)
                break;
            var bcount = pointer.GetBranchesCount();
            var curNodeState = "";
            for (int i = 0; i < bcount; i++) // i-я пара должна включить предыдущий результат
            {
                if (i == branchIndex)
                {
                    curNodeState += string.Format("({0})", result);
                }
                else
                    curNodeState += "()";
            }
            //curNodeState = string.Format(curNodeState, result);
            result = curNodeState;
            branchIndex = pointer.ParentBranchIndex;
            pointer = pointer.Parent;
        }
        Debug.Log(result + "  " + node.Id);
    }

    void Count()
    {
        _permutations++;
    }
}
