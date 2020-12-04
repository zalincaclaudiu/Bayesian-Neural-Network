using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BayesianNeuralNetwork
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        /*Graph graph = new Graph("cancer.txt");
            /for (int i = 0; i < graph.nodes.Count; ++i)
            {
                Console.WriteLine($"{graph.nodes[i].tag} -> {graph.nodes[i].True() * 100}%");
            }
        var dependents = new List<Tuple<Node, int>>();
        dependents.Add(new Tuple<Node, int>(graph.nodes[3], 1));
            dependents.Add(new Tuple<Node, int>(graph.nodes[4], 0));
            for (int i = 0; i<graph.nodes.Count; ++i)
            {
                Console.WriteLine($"{graph.nodes[i].tag} -> {graph.queryProbability(graph.nodes[i], dependents) * 100}%");
            }
        public static List<Node> LoadData()
        {
            List<Node> graph = new List<Node>();
            Node Asia = new Node("Asia");
            Asia.probabilityTable.Add(new List<double> { 0.99f, 0.01f });
            Node Tub = new Node("Has Tuberculosis");
            Asia.AddSon(Tub);
            Tub.probabilityTable.Add(new List<double> { 0.99f, 0.01f });
            Tub.probabilityTable.Add(new List<double> { 0.95f, 0.05f });
            Node Smoker = new Node("Smoker");
            Smoker.probabilityTable.Add(new List<double> { 0.5f, 0.5f });
            Node Lung = new Node("Has Lung Cancer");
            Smoker.AddSon(Lung);
            Lung.probabilityTable.Add(new List<double> { 0.99f, 0.01f });
            Lung.probabilityTable.Add(new List<double> { 0.9f, 0.1f });
            Node Bronchite = new Node("Has Bronchitis");
            Smoker.AddSon(Bronchite);
            Bronchite.probabilityTable.Add(new List<double> { 0.7f, 0.3f });
            Bronchite.probabilityTable.Add(new List<double> { 0.4f, 0.6f });
            Node Either = new Node("Tuberculosis or Cancer");

            Tub.AddSon(Either);
            Lung.AddSon(Either);
            
            Either.probabilityTable.Add(new List<double> { 1.0f, 0.0f });
            Either.probabilityTable.Add(new List<double> { 0.0f, 1.0f });
            Either.probabilityTable.Add(new List<double> { 0.0f, 1.0f });
            Either.probabilityTable.Add(new List<double> { 0.0f, 1.0f });
            Node Dyspnea = new Node("Dyspnea");
            Either.AddSon(Dyspnea);
            Bronchite.AddSon(Dyspnea);
           
            Dyspnea.probabilityTable.Add(new List<double> { 0.9f, 0.1f });
            Dyspnea.probabilityTable.Add(new List<double> { 0.3f, 0.7f });
            Dyspnea.probabilityTable.Add(new List<double> { 0.2f, 0.8f });
            Dyspnea.probabilityTable.Add(new List<double> { 0.1f, 0.9f });
            Node Xray = new Node("XRay Result");
            Either.AddSon(Xray);
            Xray.probabilityTable.Add(new List<double> { 0.95f, 0.05f });
            Xray.probabilityTable.Add(new List<double> { 0.02f, 0.98f });
            graph.Add(Asia);
            graph.Add(Tub);
            graph.Add(Smoker);
            graph.Add(Lung);
            graph.Add(Bronchite);
            
            graph.Add(Either);
            graph.Add(Dyspnea);
            graph.Add(Xray);

            for(int i = 0; i < graph.Count; ++i)
            {
                Console.WriteLine($"{graph[i].tag} -> {graph[i].True()*100}%");
            }

            

            var dependents = new List<Tuple<Node, int>>();
            dependents.Add(new Tuple<Node, int>(Lung,1));
            

            for (int i = 0; i < graph.Count; ++i)
            {
                Console.WriteLine($"{graph[i].nodeName} -> {queryProbability(graph[i],dependents,graph) * 100}%");
            }

            return graph;
        }

        public static double queryProbability(Node node, List<Tuple<Node, int>> dependents, List<Node> graph)
        {
            var depF = new List<Tuple<Node, int>>();
            var depT = new List<Tuple<Node, int>>();
            depF.AddRange(dependents);
            depT.AddRange(dependents);
            depT.Add(new Tuple<Node, int>(node, 1));
            depF.Add(new Tuple<Node, int>(node, 0));
            var t = recursive(depT, graph, 0);
            var f = recursive(depF, graph, 0);
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


        public static double recursive(List<Tuple<Node,int>> dependents, List<Node> graph, int ind)
        {
            if (ind<graph.Count) {
                var depF = new List<Tuple<Node, int>>();
                var depT = new List<Tuple<Node, int>>();
                depF.AddRange(dependents);
                depT.AddRange(dependents);
                depF.Add(new Tuple<Node, int>(graph[ind], 0));
                depT.Add(new Tuple<Node, int>(graph[ind], 1));
                if (dependents.FirstOrDefault(d => d.Item1 == graph[ind]) != null)
                {
                    if (dependents.First(d => d.Item1 == graph[ind]).Item2 == 0)
                    {
                        return graph[ind].getFalseProbability(getParentsValues(dependents, graph[ind].parents)) * recursive(depF, graph, ind + 1);
                        
                    }
                    else
                    {
                        return graph[ind].getTrueProbability(getParentsValues(dependents, graph[ind].parents)) * recursive(depT, graph, ind + 1);
                    }
                }
                else
                {
                    return graph[ind].getFalseProbability(getParentsValues(dependents, graph[ind].parents)) * recursive(depF, graph, ind + 1) +
                        graph[ind].getTrueProbability(getParentsValues(dependents, graph[ind].parents)) * recursive(depT, graph, ind + 1);
                }
            }
            else
            {
                return 1;
            }
        }

        public static bool checkParentsAreInDependents(List<Tuple<Node,int>> dependents, List<Node> parents)
        {
            foreach(Node node in parents)
            {
                if (dependents.Select(i => i.Item1).FirstOrDefault(n => n == node) == null)
                {
                    return false;
                }
            }
            return true;
        }

        public static List<Tuple<Node, int>> getParentsValues(List<Tuple<Node, int>> dependents, List<Node> parents)
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

        public static string afisare(List<Tuple<Node, int>> dependents, List<Node> graph, int ind)
        {
            if (ind < graph.Count)
            {
                var depF = new List<Tuple<Node, int>>();
                var depT = new List<Tuple<Node, int>>();
                depF.AddRange(dependents);
                depT.AddRange(dependents);
                depF.Add(new Tuple<Node, int>(graph[ind], 0));
                depT.Add(new Tuple<Node, int>(graph[ind], 1));
                if (dependents.FirstOrDefault(d => d.Item1 == graph[ind]) != null)
                {
                    if (dependents.First(d => d.Item1 == graph[ind]).Item2 == 0)
                    {
                        return $"PN({graph[ind].tag})*{afisare(depF, graph, ind + 1)}";

                    }
                    else
                    {
                        return $"PD({graph[ind].tag})*{afisare(depT, graph, ind + 1)}";
                    }
                }
                else
                {
                    return $"Sum({graph[ind].tag})";
                }
            }
            else
            {
                return "";
            }
        }*/
    }
}
