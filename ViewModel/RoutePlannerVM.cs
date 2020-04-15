using GraphTheory.Core;
using GraphTheoryInWPF.Components;
using GraphTheoryInWPF.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GraphTheoryInWPF.ViewModel {
    public class RoutePlannerVM: INotifyPropertyChanged {

        public int GoalCounter = 0;
        public string GoalName { get => "Goal " + GoalCounter; }
        public ObservableCollection<string> NodeNames { get => new ObservableCollection<string>(_graph.GetAllNodeNames()); }
        public ObservableCollection<NodeSelector> NodeSelectors { get; set; } = new ObservableCollection<NodeSelector>();

        private readonly Graph _graph;
        private readonly TextBlock _textblock;
        private readonly RadioButton _rbAllRoutes;
        private readonly RadioButton _rbShortestRoute;
        private readonly Canvas _shortestRouteCanvas;

        public RoutePlannerVM(Graph graph, TextBlock textBlock, RadioButton rbAllRoutes, RadioButton rbShortestRoute, Canvas shortestRouteCanvas) {
            this._graph = graph;
            this._textblock = textBlock;
            this.ButtonPlus();
            this.ButtonPlus();
            this._rbAllRoutes = rbAllRoutes;
            this._rbShortestRoute = rbShortestRoute;
            this._shortestRouteCanvas = shortestRouteCanvas;

            this.FillCanvasWithAllNodes();



        }

        private void FillCanvasWithAllNodes(/*int minDistance = 10*/) {
            // Gett All Names and Sizes
            List<string> allNodeNames =  this._graph.GetAllNodeNames().ToList();
            List<Point> sizes = new List<Point>();
            allNodeNames.ForEach(x => sizes.Add(NodeEllipse.GetEllipseWidthAndHeightBasedOnText(x,
                (int) Properties.Settings.Default["MinNodeEllipsePadding"],
                this._graph.GetNode(x))));

            // Actually Fill the Canvas
            for (int i = 0; i < allNodeNames.Count; i++) {
                Node n = this._graph.GetNode(allNodeNames[i]);
                this.AddNodeEllipse(n, new Point(n.Position.X, n.Position.Y));
            }

            for (int j = 0; j < this._shortestRouteCanvas.Children.Count; j++) {
                if (this._shortestRouteCanvas.Children[j] is NodeEllipse nodeEllipse) {
                    nodeEllipse.InstantiateConnectionLines();
                }
            }
        }

        private void AddNodeEllipse(Node n, Point p) {
            NodeEllipse nodeEllipse = new NodeEllipse(this._shortestRouteCanvas, this._graph, n, p, Brushes.Magenta, Brushes.White,
                                                      (int) Properties.Settings.Default["MinNodeEllipsePadding"]);
            this._shortestRouteCanvas.Children.Add(nodeEllipse);
        }

        public void OnNodeSelectorChanged() {
            if (this._rbShortestRoute.IsChecked == true) {
                this._textblock.Text = this.GetShortestRouteAsString();

            } else if (this._rbAllRoutes.IsChecked == true) {
                this._textblock.Text = this.GetAllRoute();
            }
            for (int i = 0; i < this._shortestRouteCanvas.Children.Count; i++) {
                if (this._shortestRouteCanvas.Children[i] is NodeEllipse nodeEllipse) {
                    if (this._rbShortestRoute.IsChecked == true)
                        nodeEllipse.UpdateConnectionColours(this.GetShortestRoute());
                    else
                        nodeEllipse.SetConnectionIsNotPath();
                }
            }
        }

        public string GetAllRoute() {
            try {
                if (this.NodeSelectors.Count < 2) {
                    // TOO FEW ROUTES
                    throw new GraphException("TOO FEW ROUTES!");
                }
                string startNodeName = this.NodeSelectors[0].GetContent();
                string output = "";
                for (int i = 1; i < this.NodeSelectors.Count; i++) {
                    string destinationNodeName = this.NodeSelectors[i].GetContent();
                    List<Route> lr = this._graph.GetAllRoutesFromNodeAToNodeB(startNodeName, destinationNodeName);
                    int j = 0;
                    foreach (Route r in lr) {
                        output += $"{i}.{j}: {r.ToString(" → ")}\n";
                        j++;
                    }
                    startNodeName = destinationNodeName;
                }
                return output;
            } catch (GraphException e) {
                return e.Message;
            } catch (Exception) {
                return "ALL NODES NEED VALID VALUES";
            }
        }

        public Route GetShortestRoute() {
            Route outputRoute = new Route();
            try {

                string startNodeName = this.NodeSelectors[0].GetContent();

                for (int i = 1; i < this.NodeSelectors.Count; i++) {
                    string destinationNodeName = this.NodeSelectors[i].GetContent();
                    Route r = this._graph.GetShortestRouteFromNodeAToNodeB(startNodeName, destinationNodeName);
                    foreach (Node node in r.Nodes) {
                        outputRoute.Nodes.Add(node);
                    }
                    startNodeName = destinationNodeName;
                }

                return outputRoute;
            } catch (Exception) {
                return new Route();
                //throw;
            }
        }

        public string GetShortestRouteAsString() {
            try {
                if (this.NodeSelectors.Count < 2) {
                    // TOO FEW ROUTES
                    throw new GraphException("TOO FEW ROUTES!");
                }
                string startNodeName = this.NodeSelectors[0].GetContent();
                string output = "";
                for (int i = 1; i < this.NodeSelectors.Count; i++) {
                    string destinationNodeName = this.NodeSelectors[i].GetContent();
                    Route r = this._graph.GetShortestRouteFromNodeAToNodeB(startNodeName, destinationNodeName);
                    output += $"{i}: {r.ToString(" → ")}\n";
                    startNodeName = destinationNodeName;
                }
                return output;
            } catch (GraphException e) {
                return e.Message;
            } catch (Exception) {
                return "ALL NODES NEED VALID VALUES";
            }
        }

        public void ButtonPlus() => NodeSelectors.Add(new NodeSelector(GoalCounter++, new ObservableCollection<string>(_graph.GetAllNodeNames()), this));

        public void UpdateOrders() {
            for (int i = 0; i < this.NodeSelectors.Count; i++) {
                this.NodeSelectors[i].OrderNumber = i;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string name = "")
               => this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(name));

    }
}