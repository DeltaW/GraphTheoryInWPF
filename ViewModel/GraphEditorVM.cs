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
            NodeEllipse.FillCanvasWithAllNodes(this._canvas, this._graph);
        }
    }
}
