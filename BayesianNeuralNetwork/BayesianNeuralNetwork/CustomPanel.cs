using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BayesianNeuralNetwork
{
    public class CustomPanel : Panel
    {
        public int nodeId;
        public Label title;
        public Label trueLabel;
        public Label falseLabel;
        public CheckBox trueCheckBox;
        public CheckBox falseCheckBox;
        public ProgressBar trueProgressBar;
        public ProgressBar falseProgressBar;
        public Label trueProbabilityLabel;
        public Label falseProbabilityLabel;
        public double nodeProbability;

        public CustomPanel(int id) : base()
        {
            nodeId = id;
        }

        public void setNodeProbability(double p)
        {
            nodeProbability = Math.Round(p * 100, 2);
            trueProgressBar.Value = (int)Math.Round(nodeProbability);
            falseProgressBar.Value = 100 - (int)Math.Round(nodeProbability);
            trueProbabilityLabel.Text = $"{nodeProbability}%";
            falseProbabilityLabel.Text = $"{100-nodeProbability}%";
        }
    }
}
