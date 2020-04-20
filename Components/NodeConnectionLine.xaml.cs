using GraphTheory.Core;
using GraphTheoryInWPF.View;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for NodeConnectionLine.xaml
    /// </summary>
    public partial class NodeConnectionLine: UserControl {

        public NodeEllipse ToNodeEllipse, FromNodeEllipse;
        private readonly Canvas _canvas;
        private readonly Graph _graph;
        private readonly UserControl _parent;

        public bool IsPartOfPath = false;


        public NodeConnectionLine(NodeEllipse fromNodeEllipse, NodeEllipse toNodeEllipse, Graph graph, Canvas c, UserControl userControl) {
            this.InitializeComponent();
            this.FromNodeEllipse = fromNodeEllipse;
            this.ToNodeEllipse = toNodeEllipse;
            this._canvas = c;
            this._graph = graph;
            this._parent = userControl;

            SolidColorBrush solidColorBrush = new SolidColorBrush(new System.Windows.Media.Color {
                A = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionNormalBrushColour"]).A,
                R = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionNormalBrushColour"]).R,
                G = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionNormalBrushColour"]).G,
                B = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionNormalBrushColour"]).B,
            });

            this.MainLine.Stroke = solidColorBrush;
            this.MainLine.StrokeThickness = 3;

            this.DirectionalLine1.Stroke = solidColorBrush;
            this.DirectionalLine1.StrokeThickness = 3;

            this.DirectionalLine2.Stroke = solidColorBrush;
            this.DirectionalLine2.StrokeThickness = 3;

            Canvas.SetZIndex(this.MainLine, -1);
            Canvas.SetZIndex(this.DirectionalLine1, 2);
            Canvas.SetZIndex(this.DirectionalLine2, 2);

            c.Children.Add(this);

            this.UpdatePosition();
        }

        public void UpdatePosition() {
            System.Windows.Point fromPoint = this.FromNodeEllipse.GetClosestBorderPoint(this.ToNodeEllipse.Center);
            System.Windows.Point toPoint = this.ToNodeEllipse.GetClosestBorderPoint(this.FromNodeEllipse.Center);

            this.MainLine.X1 = fromPoint.X;
            this.MainLine.Y1 = fromPoint.Y;
            this.MainLine.X2 = toPoint.X;
            this.MainLine.Y2 = toPoint.Y;

            this.UpdateArrowCoordinate();
        }

        private void UpdateArrowCoordinate(double degrees = 45) {
            Point p = this.GetDirectionArrowPoint();
            Point toPoint = new Point((int) this.MainLine.X2, (int) this.MainLine.Y2);
            Point rotatedPoint1 = this.RotatePointAroundAnotherPointByDegrees(p,toPoint,  degrees);
            Point rotatedPoint2 = this.RotatePointAroundAnotherPointByDegrees(p,toPoint, -degrees);

            this.DirectionalLine1.X1 = toPoint.X;
            this.DirectionalLine1.Y1 = toPoint.Y;
            this.DirectionalLine1.X2 = rotatedPoint1.X;
            this.DirectionalLine1.Y2 = rotatedPoint1.Y;

            this.DirectionalLine2.X1 = toPoint.X;
            this.DirectionalLine2.Y1 = toPoint.Y;
            this.DirectionalLine2.X2 = rotatedPoint2.X;
            this.DirectionalLine2.Y2 = rotatedPoint2.Y;
        }

        private Point GetDirectionArrowPoint() {
            System.Windows.Vector vector = new System.Windows.Vector(this.MainLine.X2 - this.MainLine.X1,
                                                                     this.MainLine.Y2 - this.MainLine.Y1);
            System.Windows.Vector desiredVector = vector;
            desiredVector.Normalize();
            double desiredLength = 10;
            vector -= (desiredVector * desiredLength);

            return new Point((int) (this.MainLine.X1 + vector.X),
                             (int) (this.MainLine.Y1 + vector.Y));
        }

        private Point RotatePointAroundAnotherPointByRadians(Point p, Point o, double radians) {
            return new Point((int) (Math.Cos(radians) * (p.X - o.X) - Math.Sin(radians) * (p.Y - o.Y) + o.X),
                             (int) (Math.Sin(radians) * (p.X - o.X) + Math.Cos(radians) * (p.Y - o.Y) + o.Y));
        }

        private Point RotatePointAroundAnotherPointByDegrees(Point p, Point o, double degrees) {
            return this.RotatePointAroundAnotherPointByRadians(p, o, degrees * (Math.PI / 180)); ;
        }

        internal void UpdateIsPath(Route r) {
            try {
                string fromNodeName = this.FromNodeEllipse.GetNode().Name;
                string toNodeName = this.ToNodeEllipse.GetNode().Name;
                if (r.Nodes.Count == 2) {
                    this.IsPartOfPath = (r.Nodes[0].Name == fromNodeName && r.Nodes[1].Name == toNodeName);
                } else {
                    this.IsPartOfPath = false;
                    for (int i = 0; i < r.Nodes.Count - 1; i++) {
                        if (r.Nodes[i].Name == fromNodeName && r.Nodes[i + 1].Name == toNodeName) {
                            this.IsPartOfPath = true;
                            break;
                        }
                    }
                }
            } catch (Exception) {
                this.IsPartOfPath = false;
                throw; // um zu gucken ob man hier überhaupt landen kann
            }

        }



        private void MenuItem_Click(object sender, RoutedEventArgs e) {
            //TODO
        }

        private void Grid_ContextMenuOpening(object sender, ContextMenuEventArgs e) {
            // Set content of the context menu
            bool isTwoWay = this.ToNodeEllipse.GetNode().IsDirectlyConnectedToNode(this.FromNodeEllipse.GetNode());

            this.ConnectionContextMenu.Items.Clear();

            #region Create Context MenuItems
            // Delete OneWayConnection from FromNode to ToNode
            MenuItem menuItem1 = new MenuItem {
                Icon = new Image {
                    Source = new BitmapImage(new Uri("/Icons/DependencyWarning_16x.png", UriKind.Relative))
                },
                Header = $"Delete One Way Connection from \"{this.FromNodeEllipse.GetNode().Name}\" to \"{this.ToNodeEllipse.GetNode().Name}\"",
            };
            menuItem1.Click += MenuItem1_Click;
            this.ConnectionContextMenu.Items.Add(menuItem1);

            if (isTwoWay) {
                // Delete OneWayConnection from ToNode to FromNode
                MenuItem menuItem2 = new MenuItem {
                    Icon = new Image {
                        Source = new BitmapImage(new Uri("/Icons/DependencyWarning_16x.png", UriKind.Relative))
                    },
                    Header = $"Delete One Way Connection from \"{this.ToNodeEllipse.GetNode().Name}\" to \"{this.FromNodeEllipse.GetNode().Name}\"",
                };
                menuItem2.Click += MenuItem2_Click;
                this.ConnectionContextMenu.Items.Add(menuItem2);
                // Delete TwoWayConnection
                MenuItem menuItem3 = new MenuItem {
                    Icon = new Image {
                        Source = new BitmapImage(new Uri("/Icons/DependencyWarning_16x.png", UriKind.Relative))
                    },
                    Header = $"Delete Two Way Connection",
                };
                menuItem3.Click += MenuItem3_Click;
                this.ConnectionContextMenu.Items.Add(menuItem3);
            } else {
                // Turn into TwoWayConnection
                MenuItem menuItem4 = new MenuItem {
                    Icon = new Image {
                        Source = new BitmapImage(new Uri("/Icons/SyncArrow_16x.png", UriKind.Relative))
                    },
                    Header = $"Turn into Two Way Connection",
                };
                menuItem4.Click += MenuItem4_Click;
                this.ConnectionContextMenu.Items.Add(menuItem4);
            }
            #endregion
        }

        private void MenuItem4_Click(object sender, RoutedEventArgs e) {
            this.ToNodeEllipse.GetNode().AddConnectionToNode(this.FromNodeEllipse.GetNode());
            this.UpdateShownView();
        }

        private void UpdateShownView() {
            // Update ShownView
            if (this._parent is SettingsEditor settingsEditor) {
                // Only need to redraw the canvas
                this._canvas.Children.Clear();
                NodeEllipse.FillCanvasWithAllNodes(this._canvas, this._graph, this._parent);
            } else if (this._parent is RoutePlanner routePlanner) {
                this._canvas.Children.Clear();
                NodeEllipse.FillCanvasWithAllNodes(this._canvas, this._graph, this._parent);
                routePlanner.RPVM.Update();
            } else if (this._parent is GraphEditor graphEditor) {
                this._canvas.Children.Clear();
                NodeEllipse.FillCanvasWithAllNodes(this._canvas, this._graph, this._parent);
                graphEditor.GEVM.Update();
            } else {
                throw new NotImplementedException();
            }
        }

        private void MenuItem1_Click(object sender, RoutedEventArgs e) {
            // Delete OneWayConnection from FromNode to ToNode
            this.FromNodeEllipse.GetNode().RemoveAllConnectionsToNode(this.ToNodeEllipse.GetNode());
            this.UpdateShownView();
        }



        private void MenuItem2_Click(object sender, RoutedEventArgs e) {
            // Delete OneWayConnection from  ToNode toFromNode
            this.ToNodeEllipse.GetNode().RemoveAllConnectionsToNode(this.FromNodeEllipse.GetNode());
            this.UpdateShownView();

        }

        private void MenuItem3_Click(object sender, RoutedEventArgs e) {
            this.FromNodeEllipse.GetNode().RemoveAllConnectionsToNode(this.ToNodeEllipse.GetNode());
            this.ToNodeEllipse.GetNode().RemoveAllConnectionsToNode(this.FromNodeEllipse.GetNode());
            // Delete TwoWayConnection
            this.UpdateShownView();

        }

        public void UpdateColour() {
            if (this.IsPartOfPath) {
                this.MainLine.Stroke = new SolidColorBrush(
                    new System.Windows.Media.Color() {
                        A = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionPathBrushColour"]).A,
                        R = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionPathBrushColour"]).R,
                        G = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionPathBrushColour"]).G,
                        B = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionPathBrushColour"]).B,
                    });
                Canvas.SetZIndex(this, 0);

                this.DirectionalLine1.Stroke = new SolidColorBrush(
                    new System.Windows.Media.Color() {
                        A = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionPathBrushColour"]).A,
                        R = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionPathBrushColour"]).R,
                        G = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionPathBrushColour"]).G,
                        B = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionPathBrushColour"]).B,
                    });
                this.DirectionalLine2.Stroke = new SolidColorBrush(
                    new System.Windows.Media.Color() {
                        A = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionPathBrushColour"]).A,
                        R = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionPathBrushColour"]).R,
                        G = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionPathBrushColour"]).G,
                        B = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionPathBrushColour"]).B,
                    });
            } else {
                this.MainLine.Stroke = new SolidColorBrush(
                    new System.Windows.Media.Color() {
                        A = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionNormalBrushColour"]).A,
                        R = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionNormalBrushColour"]).R,
                        G = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionNormalBrushColour"]).G,
                        B = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionNormalBrushColour"]).B,
                    });
                Canvas.SetZIndex(this, -1);

                this.DirectionalLine1.Stroke = new SolidColorBrush(
                    new System.Windows.Media.Color() {
                        A = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionNormalBrushColour"]).A,
                        R = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionNormalBrushColour"]).R,
                        G = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionNormalBrushColour"]).G,
                        B = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionNormalBrushColour"]).B,
                    });
                this.DirectionalLine2.Stroke = new SolidColorBrush(
                    new System.Windows.Media.Color() {
                        A = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionNormalBrushColour"]).A,
                        R = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionNormalBrushColour"]).R,
                        G = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionNormalBrushColour"]).G,
                        B = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionNormalBrushColour"]).B,
                    });
            }
        }

    }
}
