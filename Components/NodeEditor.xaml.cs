using GraphTheory.Core;
using GraphTheoryInWPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraphTheoryInWPF.Components {
    /// <summary>
    /// Interaction logic for NodeEditor.xaml
    /// </summary>
    public partial class NodeEditor: UserControl {

        private readonly GraphEditorVM _gevm;
        private readonly Node _node;
        private readonly Graph _graph;

        public ObservableCollection<NodeConnectionEditor> NodeConnectionEditors { get; set; }

        public string NodeName {
            get => this._node.Name;
        }

        public NodeEditor() {
            this.InitializeComponent();
        }

        public NodeEditor(Node node, GraphEditorVM gevm, Graph graph) {
            this._node = node;
            this._gevm = gevm;
            this._graph = graph;

            this.InstantiateConnections();

            this.InitializeComponent();
            this.DataContext = this;
        }

        private void InstantiateConnections() {
            this.NodeConnectionEditors = new ObservableCollection<NodeConnectionEditor>();

            // this is uesed when an already existing graph got passed on
            // the node might already have connections so it needs to be filled
            foreach (Connection connection in this._node.Connections) {
                this.NodeConnectionEditors.Add(new NodeConnectionEditor(this._gevm, this, this._node, this._graph, connection.ToNode));
            }
        }

        public void UpdateAllConnectionEditors() {
            // Hexenwerk
            var allNodeNames = this._graph.GetAllNodeNames();

            // Loop through each NodeConnectionEditor
            for (int i = this.NodeConnectionEditors.Count - 1; i >= 0; i--) {
                var nodeConnectionEditor = this.NodeConnectionEditors[i];
                // Remove the nodeConnectionEditor if the node it references is no longer part of the graph
                if (nodeConnectionEditor.ConnectedNode == null || !allNodeNames.Contains(nodeConnectionEditor.ConnectedNode.Name)) {
                    this.NodeConnectionEditors.Remove(nodeConnectionEditor);
                }
            }

            // Update the ComboBoxItems for each remaining NodeConnectionEditor
            foreach (NodeConnectionEditor nodeConnectionEditor in this.NodeConnectionEditors) {
                nodeConnectionEditor.SetConnectionChoices();
            }
        }

        private void Button_Click_DeleteNode(object sender, RoutedEventArgs e) {
            this._graph.RemoveNodeFromGraph(this.NodeName);
            this._gevm.RemoveNodeEditor(this);
            this._gevm.OnGraphChanged();

            // Hexenwerk
            // Upon deletion of a node:
            //      Update EVERY NodeEditor's connections
            foreach (var item in this._gevm.NodeEditors) {
                item.UpdateAllConnectionEditors();
            }
            //this._gevm.UpdateNodeConnectionComboBoxes(this.NodeName);
        }

        private void Button_Click_AddConnection(object sender, RoutedEventArgs e) {
            this.NodeConnectionEditors.Add(new NodeConnectionEditor(this._gevm, this, this._node, this._graph));
            this.CollapsableContainer.IsExpanded = true;
        }

        private void Button_Click_ResetPosition(object sender, RoutedEventArgs e) {
            this._node.Position = new System.Drawing.Point(0, 0);
            this._gevm.OnGraphChanged();
        }
    }
}
