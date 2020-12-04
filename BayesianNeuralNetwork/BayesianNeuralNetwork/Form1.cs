using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BayesianNeuralNetwork
{
    public partial class Form1 : Form
    {
        public Graph graph;
        public List<Tuple<Node, int>> queries;
        public Form1()
        {
            graph = new Graph("cancer.txt");
            queries = new List<Tuple<Node, int>>();
            InitializePanels();
            InitializeComponent();
        }
    }
}
