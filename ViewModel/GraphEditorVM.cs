using GraphTheory.Core;
using GraphTheoryInWPF.Components;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GraphTheoryInWPF.ViewModel {
    public class GraphEditorVM {
        private readonly Graph _graph;
        private readonly Canvas _canvas;

        public ObservableCollection<string> NodeNames { get => new ObservableCollection<string>(_graph.GetAllNodeNames()); }
        public ObservableCollection<NodeEditor> NodeEditors { get; set; }

        public GraphEditorVM(Graph graph, Canvas canvas) {
            this._graph = graph;
            this._canvas = canvas;

            this.InstantiateNodeEditors();
            this.OnGraphChanged();
        }

        private void InstantiateNodeEditors() {
            this.NodeEditors = new ObservableCollection<NodeEditor>();
            foreach (string name in this.NodeNames) {
                this.NodeEditors.Add(new NodeEditor(this._graph.GetNode(name), this, this._graph));
            }
        }

        public void RemoveNodeEditor(NodeEditor nodeEditor) {
            this.NodeEditors.Remove(nodeEditor);
            //foreach (var item in this.NodeEditors) {
                //item.UpdateConnectionData(nodeEditor.NodeName);
            //}
        }

        public void ButtonAddNode(string nodeName) {
            this._graph.AddNewNodeToGraph(nodeName);
            this.NodeEditors.Add(new NodeEditor(this._graph.GetNode(nodeName),this, this._graph));

            // Update all connection comboboxes
            foreach (var item in this.NodeEditors) {
                item.UpdateAllConnectionEditors();
            }


            this.OnGraphChanged();
        }


        public void OnGraphChanged() {
            this._canvas.Children.Clear();
            this.FillCanvasWithAllNodes();
        }


        #region change this into static methods because they are the exact same as in routeplanner
        private void FillCanvasWithAllNodes(/*int minDistance = 10*/) {
            // Gett All Names and Sizes
            List<string> allNodeNames =  this._graph.GetAllNodeNames().ToList();
            List<Point> sizes = new List<Point>();
            foreach (string name in allNodeNames) {
                sizes.Add(NodeEllipse.GetEllipseWidthAndHeightBasedOnText(name));
            }
            //allNodeNames.ForEach(x => Sizes.Add(NodeEllipse.GetEllipseWidthAndHeightBasedOnText(x)));

            // Actually Fill the Canvas
            for (int i = 0; i < allNodeNames.Count; i++) {
                Node n = this._graph.GetNode(allNodeNames[i]);
                this.AddNodeEllipse(n, new Point(n.Position.X, n.Position.Y));
            }
            for (int j = 0; j < this._canvas.Children.Count; j++) {
                if (this._canvas.Children[j] is NodeEllipse nodeEllipse) {
                    nodeEllipse.InstantiateConnectionLines();
                }
            }
        }

        private void AddNodeEllipse(Node n, Point p) {
            NodeEllipse nodeEllipse = new NodeEllipse(this._canvas, this._graph, n, p, Brushes.Magenta, Brushes.White);
            this._canvas.Children.Add(nodeEllipse);
        }
        #endregion
    }
}
