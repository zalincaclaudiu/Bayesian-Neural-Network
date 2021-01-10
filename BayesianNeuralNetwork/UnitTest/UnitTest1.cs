using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using BayesianNeuralNetwork;
using System.Collections.Generic;
using System.Linq;

//acest unit test va analiza rezulatele unei retele bayesiene pe setul de date Asia care analizeaza cancerul de plamani si tuberculoza
namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        //Rezultatele din program sunt verificate cu cele de pe aplicatia https://www.bayesserver.com/examples/networks/asia

        //testarea probabilitatii generale a nodului interior "Either" care reprezinta probabilitatea ca o persoana sa aiba fie cancer, fie tuberculoza
        [TestMethod]
        public void TestMethod1()
        {
            var graph = new Graph("cancer.txt");
            var p = graph.nodes.Where(n => n.nodeName == "either").First().True();
            Assert.AreEqual(6.48, Math.Round(p * 100,2));
        }

        //probabilitatea ca o persoana sa aiba Dyspnea daca este fumator si daca nu sufera de cancer sau tuberculoza
        [TestMethod]
        public void TestMethod2()
        {
            var graph = new Graph("cancer.txt");
            var queries = new List<Tuple<Node, int>>();
            queries.Add(new Tuple<Node, int>(graph.nodes[2], 1)); //nodul fumator setat pe true
            queries.Add(new Tuple<Node, int>(graph.nodes[5], 0)); //nodul cancer sau tuberculoza setat pe false
            var p = graph.queryProbability(graph.nodes[6], queries); //calcularea probabilitatii Dispneei in functie de queries
            Assert.AreEqual(52.0, Math.Round(p * 100, 2));
        }

        //probabilitatea ca o persoana sa fie fumatoare daca nu are cancer, dar are bronsita
        [TestMethod]
        public void TestMethod3()
        {
            var graph = new Graph("cancer.txt");
            var queries = new List<Tuple<Node, int>>();
            queries.Add(new Tuple<Node, int>(graph.nodes[3], 0)); //nodul cancer setat pe false
            queries.Add(new Tuple<Node, int>(graph.nodes[4], 1)); //nodul bronsita setat pe true
            var p = graph.queryProbability(graph.nodes[2], queries); //calcularea probabilitatii fumator in functie de queries
            Assert.AreEqual(64.52, Math.Round(p * 100, 2));
        }

        //probabilitatea ca o persoana sa aiba radiografii anormale daca a vizitat Asia si este fumator
        [TestMethod]
        public void TestMethod4()
        {
            var graph = new Graph("cancer.txt");
            var queries = new List<Tuple<Node, int>>();
            queries.Add(new Tuple<Node, int>(graph.nodes[0], 1)); //nodul asia setat pe true
            queries.Add(new Tuple<Node, int>(graph.nodes[2], 1)); //nodul fumator setat pe true
            var p = graph.queryProbability(graph.nodes[7], queries); //calcularea probabilitatii radiografiei anormale in functie de queries
            Assert.AreEqual(18.49, Math.Round(p * 100, 2));
        }

        //probabilitatea ca o persoana sa nu aiba tuberculoza daca are radiografie anormala si sufera de Dispnee
        [TestMethod]
        public void TestMethod5()
        {
            var graph = new Graph("cancer.txt");
            var queries = new List<Tuple<Node, int>>();
            queries.Add(new Tuple<Node, int>(graph.nodes[7], 1)); //nodul radiografie anormala setat pe true
            queries.Add(new Tuple<Node, int>(graph.nodes[6], 1)); //nodul Dispnee setat pe true
            var p = graph.queryProbability(graph.nodes[1], queries); //calcularea probabilitatii tuberculozii in functie de queries
            Assert.AreEqual(100 - 11.39, 100 - Math.Round(p * 100, 2));
        }
    }
}
