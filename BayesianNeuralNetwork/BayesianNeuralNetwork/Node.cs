using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BayesianNeuralNetwork
{
    public class Node
    {
        public List<Node> parents;
        public List<Node> sons;
        public List<List<float>> probabilityTable;
        public float trueProbabilityNow;
        public string tag;
        public int states;

        public Node(string tag)
        {
            this.tag = tag;
            parents = new List<Node>();
            sons = new List<Node>();
            probabilityTable = new List<List<float>>();
            states=2;
        }

        public void AddSon(Node node)
        {
            sons.Add(node);
            node.parents.Add(this);
        }

        public float True()
        {
            if (parents.Count != 0)
            {
                return True(new List<Tuple<Node, int>>());
            }
            else
            {
                return probabilityTable[0][1];
            }
        }

        public float True(List<Tuple<Node,int>> dependents)
        {
            if (dependents.Count < this.parents.Count)
            {
                int lastIndex = dependents.Count();
                var falseDependent = new List<Tuple<Node, int>>();
                falseDependent.AddRange(dependents);
                var trueDependent = new List<Tuple<Node, int>>();
                trueDependent.AddRange(dependents);
                var falseNewParent = new Tuple<Node, int>(parents[lastIndex], 0);
                var trueNewParent = new Tuple<Node, int>(parents[lastIndex], 1);
                trueDependent.Add(trueNewParent);
                falseDependent.Add(falseNewParent);
                return True(trueDependent) + True(falseDependent);
            }
            else
            {
                float p = getTrueProbability(dependents);
                foreach(Tuple<Node,int> tuple in dependents)
                {
                    float dP = tuple.Item2 == 0 ? tuple.Item1.False() : tuple.Item1.True();
                    p = p * dP;
                }
                return p;
            }
        }

        public float False()
        {
            return 1 - True();
        }

        public float getTrueProbability(List<Tuple<Node,int>> parentValues)
        {
            double index = 0;
            for (int i = 0; i < parents.Count; ++i)
            {
                double indP = parents.IndexOf(parentValues[i].Item1);
                index += Math.Pow(2, indP) * parentValues[i].Item2;
            }
            return probabilityTable[(int)index][1];
        }

        public float getFalseProbability(List<Tuple<Node, int>> parentValues)
        {
            return 1 - getTrueProbability(parentValues);
        }

    }
}
