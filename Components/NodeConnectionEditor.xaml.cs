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
    /// Interaction logic for ConnectedNodesEditor.xaml
    /// </summary>
    public partial class NodeConnectionEditor: UserControl {

        private readonly Graph _graph;
        private readonly Node _node;
        private readonly GraphEditorVM _gevm;
        private readonly NodeEditor _nodeEditor;

        public Node ConnectedNode { set; get; }

        public ObservableCollection<string> ConnectionChoices { set; get; } = new ObservableCollection<string>();

        public void SetConnectionChoices() {
            // Seems to be working perfectly. But it does not currently update. only set correctly at start

            List<string> output = this._graph.GetAllNodeNames().ToList();
            // Remove self
            output.RemoveAll(x => x == this._node.Name);
            // Remove already existing connections
            foreach (string name in this._node.Connections.Select(x => x.ToNode.Name)) {
                output.RemoveAll(x => x == name);
            }
            // Add the currently selected node to the choices. otherwise it is not showing up
            if (this.ConnectedNode != null)
                output.Add(this.ConnectedNode.Name);

            // Set the ConnectionChoices Property
            //this.ConnectionChoices = new ObservableCollection<string>(output);
            this.ConnectionChoices.Clear();
            foreach (var item in output) {
                this.ConnectionChoices.Add(item);
            }
        }

        public NodeConnectionEditor() {
            this.InitializeComponent();
        }

        public NodeConnectionEditor(GraphEditorVM gevm, NodeEditor nodeEditor, Node node, Graph graph) {
            this._gevm = gevm;
            this._graph = graph;
            this._node = node;
            this._nodeEditor = nodeEditor;

            this.SetConnectionChoices();

            this.InitializeComponent();
            this.DataContext = this;
        }

        public NodeConnectionEditor(GraphEditorVM gevm, NodeEditor nodeEditor, Node node, Graph graph, Node connectedNode) {
            this._gevm = gevm;
            this._graph = graph;
            this._node = node;
            this._nodeEditor = nodeEditor;


            this.ConnectedNode = connectedNode;

            this.SetConnectionChoices();

            this.InitializeComponent();
            this.DataContext = this;

            this.ConnectionSelectorComboBox.SelectedIndex = ConnectionChoices.IndexOf(connectedNode.Name);
        }

        private void ConnectionSelectorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            string selectedItem = this.ConnectionSelectorComboBox.SelectedItem?.ToString();

            if (!string.IsNullOrEmpty(selectedItem)) {


                // Remove the connection that was here before and add the new one if a valid node has been selected
                if (this.ConnectedNode != null) {
                    this._graph.RemoveOneWayConnections(this._node.Name, this.ConnectedNode.Name);
                    //this._node.RemoveConnectionToNode(this._connectedNode);
                    this.ConnectedNode = this._graph.GetNode(selectedItem);
                    this._graph.AddOneWayNodeConnetionToGraph(this._node.Name, this.ConnectedNode.Name);
                } else {
                    this.ConnectedNode = this._graph.GetNode(selectedItem);
                    this._graph.AddOneWayNodeConnetionToGraph(this._node.Name, this.ConnectedNode.Name);
                }

                // Hexenwerk
                this._nodeEditor.UpdateAllConnectionEditors();

                // Update the canvas
                this._gevm.OnGraphChanged();
            }
        }

        private void Button_Click_RemoveConnection(object sender, RoutedEventArgs e) {
            // Remove Connection
            if (this.ConnectedNode != null) {
                this._graph.RemoveOneWayConnections(this._node.Name, this.ConnectedNode.Name);
            }
            this._nodeEditor.NodeConnectionEditors.Remove(this);
            // Need to update all remaining connectionEditors of this editor's parents
            this._nodeEditor.UpdateAllConnectionEditors();
            this._gevm.OnGraphChanged();
        }
    }
}
