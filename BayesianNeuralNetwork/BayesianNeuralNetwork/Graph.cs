using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BayesianNeuralNetwork
{
    public class Graph
    {
        public List<Node> nodes;

        public Graph(string fileName)
        {
            StreamReader sr = new StreamReader(fileName);
            nodes = new List<Node>();
            while (sr.Peek() != -1)
            {
                string line = sr.ReadLine().Trim();
                if (line == "")
                    continue;

                if (line.Split(' ').Length == 1)
                {
                    var parts = line.Split('|');
                    Node newNode = new Node(parts[0]);
                    int nrOfParents = 0;
                    if (parts.Length > 1)
                    {
                        var parents = parts[1].Split(',');
                        nrOfParents = parents.Length;
                        for (int i = 0; i < nrOfParents; ++i)
                        {
                            nodes.First(n => n.nodeName.Equals(parents[i])).AddSon(newNode);
                        }
                    }
                    line = sr.ReadLine().Trim();
                    newNode.setTag(line);
                    for (int i = 0; i < Math.Pow(2, nrOfParents); ++i)
                    {
                        line = sr.ReadLine().Trim();
                        var probabilites = line.Split(' ');
                        double firstP = double.Parse(probabilites[nrOfParents]);
                        double secondP = double.Parse(probabilites[nrOfParents+1]);
                        newNode.probabilityTable.Add(new List<double> { firstP, secondP });
                    }
                    nodes.Add(newNode);
                }
            }
            sr.Close();
        }

        public double queryProbability(Node node, List<Tuple<Node, int>> dependents)
        {
            var depF = new List<Tuple<Node, int>>();
            var depT = new List<Tuple<Node, int>>();
            depF.AddRange(dependents);
            depT.AddRange(dependents);
            depT.Add(new Tuple<Node, int>(node, 1));
            depF.Add(new Tuple<Node, int>(node, 0));
            var t = recursive(depT, 0);
            var f = recursive(depF, 0);
            if (t + f < 1)
            {
                var alpha = 1.0f / (t + f);
                return alpha * t;
            }
            else
            {
                return t;
            }
        }


        public double recursive(List<Tuple<Node, int>> dependents, int ind)
        {
            if (ind < nodes.Count)
            {
                var depF = new List<Tuple<Node, int>>();
                var depT = new List<Tuple<Node, int>>();
                depF.AddRange(dependents);
                depT.AddRange(dependents);
                depF.Add(new Tuple<Node, int>(nodes[ind], 0));
                depT.Add(new Tuple<Node, int>(nodes[ind], 1));
                if (dependents.FirstOrDefault(d => d.Item1 == nodes[ind]) != null)
                {
                    if (dependents.First(d => d.Item1 == nodes[ind]).Item2 == 0)
                    {
                        return nodes[ind].getFalseProbability(getParentsValues(dependents, nodes[ind].parents)) * recursive(depF, ind + 1);

                    }
                    else
                    {
                        return nodes[ind].getTrueProbability(getParentsValues(dependents, nodes[ind].parents)) * recursive(depT, ind + 1);
                    }
                }
                else
                {
                    return nodes[ind].getFalseProbability(getParentsValues(dependents, nodes[ind].parents)) * recursive(depF, ind + 1) +
                        nodes[ind].getTrueProbability(getParentsValues(dependents, nodes[ind].parents)) * recursive(depT, ind + 1);
                }
            }
            else
            {
                return 1;
            }
        }

        public bool checkParentsAreInDependents(List<Tuple<Node, int>> dependents, List<Node> parents)
        {
            foreach (Node node in parents)
            {
                if (dependents.Select(i => i.Item1).FirstOrDefault(n => n == node) == null)
                {
                    return false;
                }
            }
            return true;
        }

        public List<Tuple<Node, int>> getParentsValues(List<Tuple<Node, int>> dependents, List<Node> parents)
        {
            var result = new List<Tuple<Node, int>>();
            foreach (Node node in parents)
            {
                var x = dependents.FirstOrDefault(n => n.Item1 == node);
                if (x != null)
                {
                    result.Add(x);
                }
            }
            return result;
        }
    }
}
