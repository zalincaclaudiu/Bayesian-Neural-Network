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
        public List<List<double>> probabilityTable;
        public string tag;
        public string nodeName;
        public int states;

        public Node(string name)
        {
            this.nodeName = name;
            parents = new List<Node>();
            sons = new List<Node>();
            probabilityTable = new List<List<double>>();
            states=2;
        }

        public void AddSon(Node node)
        {
            sons.Add(node);
            node.parents.Add(this);
        }

        public double True()
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

        public double True(List<Tuple<Node,int>> dependents)
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
                double p = getTrueProbability(dependents);
                foreach(Tuple<Node,int> tuple in dependents)
                {
                    double dP = tuple.Item2 == 0 ? tuple.Item1.False() : tuple.Item1.True();
                    p = p * dP;
                }
                return p;
            }
        }

        public double False()
        {
            return 1 - True();
        }

        public double getTrueProbability(List<Tuple<Node,int>> parentValues)
        {
            double index = 0;
            for (int i = 0; i < parents.Count; ++i)
            {
                double indP = parents.IndexOf(parentValues[i].Item1);
                index += Math.Pow(2, indP) * parentValues[i].Item2;
            }
            return probabilityTable[(int)index][1];
        }

        public double getFalseProbability(List<Tuple<Node, int>> parentValues)
        {
            return 1 - getTrueProbability(parentValues);
        }

        public void setTag(string tag)
        {
            this.tag = tag;
        }
    }
}
