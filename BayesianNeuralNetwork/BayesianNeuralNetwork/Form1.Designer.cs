
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BayesianNeuralNetwork
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1600, 900);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
        }

        public void InitializePanels()
        { 
            panels = new List<CustomPanel>();
            for (int i = 0; i < graph.nodes.Count; ++i)
            {
                var panel = new CustomPanel(i);
                panel.nodeProbability = Math.Round(graph.nodes[i].True()*100,2);

                panel.Location = new Point(72, 80 * i + 20);
                panel.Size = new Size(170, 80);
                panel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
                panel.BackColor = Color.White;

                panel.title = new Label();
                panel.title.Text = graph.nodes[i].tag;
                panel.title.Location = new Point(0, 0);
                panel.title.Size = new Size(100, 20);
                panel.Controls.Add(panel.title);

                panel.trueLabel = new Label();
                panel.trueLabel.Text = "True";
                panel.trueLabel.Location = new Point(0, 20);
                panel.trueLabel.Size = new Size(30, 15);
                panel.Controls.Add(panel.trueLabel);

                panel.trueProgressBar = new ProgressBar();
                panel.trueProgressBar.Value = (int)Math.Round(panel.nodeProbability);
                panel.trueProgressBar.Location = new Point(50, 20);
                panel.trueProgressBar.Size = new Size(40, 15);
                panel.Controls.Add(panel.trueProgressBar);

                panel.trueProbabilityLabel = new Label();
                panel.trueProbabilityLabel.Text = $"{panel.nodeProbability}%";
                panel.trueProbabilityLabel.Location = new Point(95, 20);
                panel.trueProbabilityLabel.Size = new Size(45, 15);
                panel.Controls.Add(panel.trueProbabilityLabel);

                panel.trueCheckBox = new CheckBox();
                panel.trueCheckBox.Size = new Size(20, 20);
                panel.trueCheckBox.Location = new Point(140, 20);
                panel.Controls.Add(panel.trueCheckBox);

                panel.falseLabel = new Label();
                panel.falseLabel.Text = "False";
                panel.falseLabel.Location = new Point(0, 40);
                panel.falseLabel.Size = new Size(30, 15);
                panel.Controls.Add(panel.falseLabel);

                panel.falseProgressBar = new ProgressBar();
                panel.falseProgressBar.Value = 100 - (int)Math.Round(panel.nodeProbability);
                panel.falseProgressBar.Location = new Point(50, 40);
                panel.falseProgressBar.Size = new Size(40, 15);
                panel.Controls.Add(panel.falseProgressBar);

                panel.falseProbabilityLabel = new Label();
                panel.falseProbabilityLabel.Text = $"{100-panel.nodeProbability}%";
                panel.falseProbabilityLabel.Location = new Point(95, 40);
                panel.falseProbabilityLabel.Size = new Size(45, 15);
                panel.Controls.Add(panel.falseProbabilityLabel);

                panel.falseCheckBox = new CheckBox();
                panel.falseCheckBox.Size = new Size(20, 20);
                panel.falseCheckBox.Location = new Point(140, 40);
                panel.Controls.Add(panel.falseCheckBox);

                panel.trueCheckBox.CheckedChanged += (o, e) =>
                {
                    if (panel.trueCheckBox.Checked)
                    {
                        panel.falseCheckBox.Checked = false;
                        queries.Add(new Tuple<Node, int>(graph.nodes[panel.nodeId], 1));
                        panel.setNodeProbability(1);
                    }
                    else
                    {
                        var query = queries.FirstOrDefault(q => q.Item1 == graph.nodes[panel.nodeId] && q.Item2 == 1);
                        if (query != null)
                        {
                            queries.Remove(query);
                        }
                    }
                    Update(queries);
                };

                panel.falseCheckBox.CheckedChanged += (o, e) =>
                {
                    if (panel.falseCheckBox.Checked)
                    {
                        panel.trueCheckBox.Checked = false;
                        queries.Add(new Tuple<Node, int>(graph.nodes[panel.nodeId], 0));
                        panel.setNodeProbability(0);
                    }
                    else
                    {
                        var query = queries.FirstOrDefault(q => q.Item1 == graph.nodes[panel.nodeId] && q.Item2 == 0);
                        if (query != null)
                        {
                            queries.Remove(query);
                        }
                    }
                    Update(queries);
                };


                this.Controls.Add(panel);
                panels.Add(panel);
            }
            this.SuspendLayout();
        }

        private void Update(List<Tuple<Node, int>> queries)
        {
            for (int i = 0; i < panels.Count; ++i)
            {
                if (!NodeIsInQueries(graph.nodes[panels[i].nodeId]))
                {
                    panels[i].setNodeProbability(graph.queryProbability(graph.nodes[i], queries));
                }
            }
        }

        private bool NodeIsInQueries(Node node)
        {
            return queries.FirstOrDefault(q => q.Item1 == node) != null;
        }

        public List<CustomPanel> panels;
        #endregion
    }
}

