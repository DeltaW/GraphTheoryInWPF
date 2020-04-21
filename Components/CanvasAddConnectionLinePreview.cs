using GraphTheory.Core;
using GraphTheoryInWPF.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphTheoryInWPF.Components {
    public class CanvasAddConnectionLinePreview {
        private NodeEllipse _fromNodeEllipse;
        private NodeEllipse _toNodeEllipse;
        private readonly Line _line;
        private readonly List<Line> _directionalLines;
        private readonly Canvas _canvas;
        private readonly bool _isTwoWayConnection;
        private readonly MouseButtonEventHandler _rightButtonDown;
        private readonly MouseButtonEventHandler _leftButtonUp;
        private readonly MouseEventHandler _mouseMove;
        private readonly UserControl _parent;
        private readonly Graph _graph;

        public CanvasAddConnectionLinePreview(bool isTwoWayConnection, UserControl userControl, NodeEllipse nodeEllipse, Canvas canvas, Graph graph) {
            this._fromNodeEllipse = nodeEllipse;
            this._canvas = canvas;
            this._isTwoWayConnection = isTwoWayConnection;
            this._parent = userControl;
            this._graph = graph;

            SolidColorBrush solidColorBrush = new SolidColorBrush(new System.Windows.Media.Color {
                A = ((System.Drawing.Color) Properties.Settings.Default["CanvasNodeConnectionPreviewBrushColour"]).A,
                R = ((System.Drawing.Color) Properties.Settings.Default["CanvasNodeConnectionPreviewBrushColour"]).R,
                G = ((System.Drawing.Color) Properties.Settings.Default["CanvasNodeConnectionPreviewBrushColour"]).G,
                B = ((System.Drawing.Color) Properties.Settings.Default["CanvasNodeConnectionPreviewBrushColour"]).B,
            });

            this._line = new Line {
                //X1 = nodeEllipse.Center.X,
                //Y1 = nodeEllipse.Center.Y,
                Stroke = solidColorBrush,
                StrokeThickness = (int) Properties.Settings.Default["ConnectionStrokeThickness"],
                //X2 = nodeEllipse.Center.X,
                //Y2 = nodeEllipse.Center.Y,
            };

            this._directionalLines = new List<Line>();
            for (int i = 0; i < ((this._isTwoWayConnection) ? 4 : 2); i++) {
                this._directionalLines.Add(new Line {
                    //X1 = nodeEllipse.Center.X,
                    //Y1 = nodeEllipse.Center.Y,
                    Stroke = solidColorBrush,
                    StrokeThickness = (int) Properties.Settings.Default["ConnectionStrokeThickness"],
                    //X2 = nodeEllipse.Center.X,
                    //Y2 = nodeEllipse.Center.Y,
                });
                this._canvas.Children.Add(this._directionalLines[i]);
            }

            Canvas.SetZIndex(this._line, -1);
            this._canvas.Children.Add(this._line);

            this._rightButtonDown = new MouseButtonEventHandler(this.Control_MouseRightButtonDown);
            this._leftButtonUp = new MouseButtonEventHandler(this.Control_MouseLeftButtonUp);
            this._mouseMove = new MouseEventHandler(this.Control_MouseMove);

            this._canvas.MouseRightButtonDown += this._rightButtonDown;
            this._canvas.MouseLeftButtonUp += this._leftButtonUp;
            this._canvas.MouseMove += this._mouseMove;
        }

        #region Arrow stuff
        private void UpdateArrowCoordinate(double degrees = 45) {
            Point p = this.GetOneWayDirectionArrowPoint();
            Point toPoint = new Point((int) Math.Round(this._line.X2), (int) Math.Round(this._line.Y2));
            Point rotatedPoint1 = this.RotatePointAroundAnotherPointByDegrees(p,toPoint,  degrees);
            Point rotatedPoint2 = this.RotatePointAroundAnotherPointByDegrees(p,toPoint, -degrees);

            this._directionalLines[0].X1 = toPoint.X;
            this._directionalLines[0].Y1 = toPoint.Y;
            this._directionalLines[0].X2 = rotatedPoint1.X;
            this._directionalLines[0].Y2 = rotatedPoint1.Y;

            this._directionalLines[1].X1 = toPoint.X;
            this._directionalLines[1].Y1 = toPoint.Y;
            this._directionalLines[1].X2 = rotatedPoint2.X;
            this._directionalLines[1].Y2 = rotatedPoint2.Y;

            if (this._isTwoWayConnection) {
                p = this.GetTwoWayDirectionArrowPoint();
                toPoint = new Point((int) Math.Round(this._line.X1), (int) Math.Round(this._line.Y1));
                rotatedPoint1 = this.RotatePointAroundAnotherPointByDegrees(p, toPoint, degrees);
                rotatedPoint2 = this.RotatePointAroundAnotherPointByDegrees(p, toPoint, -degrees);

                this._directionalLines[2].X1 = toPoint.X;
                this._directionalLines[2].Y1 = toPoint.Y;
                this._directionalLines[2].X2 = rotatedPoint1.X;
                this._directionalLines[2].Y2 = rotatedPoint1.Y;

                this._directionalLines[3].X1 = toPoint.X;
                this._directionalLines[3].Y1 = toPoint.Y;
                this._directionalLines[3].X2 = rotatedPoint2.X;
                this._directionalLines[3].Y2 = rotatedPoint2.Y;
            }
        }

        private Point GetOneWayDirectionArrowPoint() {
            System.Windows.Vector vector = new System.Windows.Vector(this._line.X2 - this._line.X1,
                                                                     this._line.Y2 - this._line.Y1);
            System.Windows.Vector desiredVector = vector;
            desiredVector.Normalize();
            double desiredLength = (int) Properties.Settings.Default["ConnectionArrowHeadLength"];
            vector -= (desiredVector * desiredLength);

            return new Point((int) Math.Round(this._line.X1 + vector.X),
                             (int) Math.Round(this._line.Y1 + vector.Y));
        }
        private Point GetTwoWayDirectionArrowPoint() {
            System.Windows.Vector vector = new System.Windows.Vector(this._line.X1 - this._line.X2,
                                                                     this._line.Y1 - this._line.Y2);
            System.Windows.Vector desiredVector = vector;
            desiredVector.Normalize();
            double desiredLength = (int) Properties.Settings.Default["ConnectionArrowHeadLength"];
            vector -= (desiredVector * desiredLength);

            return new Point((int) Math.Round(this._line.X2 + vector.X),
                             (int) Math.Round(this._line.Y2 + vector.Y));
        }

        private Point RotatePointAroundAnotherPointByRadians(Point p, Point o, double radians) {
            return new Point((int) Math.Round(Math.Cos(radians) * (p.X - o.X) - Math.Sin(radians) * (p.Y - o.Y) + o.X),
                             (int) Math.Round(Math.Sin(radians) * (p.X - o.X) + Math.Cos(radians) * (p.Y - o.Y) + o.Y));
        }

        private Point RotatePointAroundAnotherPointByDegrees(Point p, Point o, double degrees) {
            return this.RotatePointAroundAnotherPointByRadians(p, o, degrees * (Math.PI / 180)); ;
        }
        #endregion

        private void Control_MouseRightButtonDown(object sender, MouseEventArgs e) {
            // remove every CanvasAddConnectionLinePreview from canvas and also remove all mouse eventhandlers
            this._canvas.Children.Remove(this._line);
            this._directionalLines.ForEach(x => this._canvas.Children.Remove(x));
            this._canvas.MouseRightButtonDown -= this._rightButtonDown;
            this._canvas.MouseLeftButtonUp -= this._leftButtonUp;
            this._canvas.MouseMove -= this._mouseMove;
            this._canvas.ReleaseMouseCapture();
        }

        private void Control_MouseLeftButtonUp(object sender, MouseEventArgs e) {
            // Connect the nodes if possible
            if (this._toNodeEllipse != null) {
                try {
                    if (this._isTwoWayConnection)
                        this._graph.AddTwoWayNodeConnetionToGraph(this._fromNodeEllipse.GetNode().Name,
                                                                  this._toNodeEllipse.GetNode().Name);
                    else
                        this._graph.AddOneWayNodeConnetionToGraph(this._fromNodeEllipse.GetNode().Name,
                                                                  this._toNodeEllipse.GetNode().Name);

                    // Update the rest of the view in the parent

                    if (this._parent is SettingsEditor settingsEditor) {
                        this._canvas.Children.Clear();
                        NodeEllipse.FillCanvasWithAllNodes(this._canvas, this._graph, this._parent);
                    } else if (this._parent is GraphEditor graphEditor) {
                        this._canvas.Children.Clear();
                        NodeEllipse.FillCanvasWithAllNodes(this._canvas, this._graph, this._parent);
                        graphEditor.GEVM.Update();
                        // Preview does not display properly if this is the case
                    } else if (this._parent is RoutePlanner routePlanner) {
                        this._canvas.Children.Clear();
                        NodeEllipse.FillCanvasWithAllNodes(this._canvas, this._graph, this._parent);
                        routePlanner.RPVM.Update();
                    } else {
                        throw new NotImplementedException();
                    }

                } catch (GraphException ge) {
                    MessageBox.Show(ge.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }


            // "Destroy" Every connection of the canvas to this
            this.Control_MouseRightButtonDown(null, null);

            // if "Ctrl" is held down allow more connections
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) {
                foreach (var item in this._canvas.Children) {
                    if (item is NodeEllipse nodeEllipse) {
                        if (nodeEllipse.GetNode().Name == this._fromNodeEllipse.GetNode().Name) {
                            if (this._isTwoWayConnection)
                                nodeEllipse.MenuItem_Click_AddTwoWayConnection(null, null);
                            else
                                nodeEllipse.MenuItem_Click_AddOneWayConnection(null, null);
                            break;
                        }
                    }
                }
            }
        }

        private void SetCoordinates(Point p) {
            Point fromNodeBorderPoint = this._fromNodeEllipse.GetClosestBorderPoint(p);
            this._line.X1 = fromNodeBorderPoint.X;
            this._line.Y1 = fromNodeBorderPoint.Y;

            if (this._toNodeEllipse == null) {
                this._line.X2 = p.X;
                this._line.Y2 = p.Y;
            } else {
                Point toNodeBorderPoint = this._toNodeEllipse.GetClosestBorderPoint(this._fromNodeEllipse.Center);
                this._line.X2 = toNodeBorderPoint.X;
                this._line.Y2 = toNodeBorderPoint.Y;
            }

            this.UpdateArrowCoordinate();
        }


        private static bool IsPointWithinEllipse(Point p, Rect r) {
            // Reference: "https://stackoverflow.com/a/13285453"

            System.Drawing.Drawing2D.GraphicsPath graphicsPath = new System.Drawing.Drawing2D.GraphicsPath();
            graphicsPath.AddEllipse(new System.Drawing.RectangleF((float) r.X, (float) r.Y, (float) r.Width, (float) r.Height));
            return graphicsPath.IsVisible((float) p.X, (float) p.Y);

            #region Old point within rect calculation
            /*
            // too far left
            if (p.X < r.X)
                return false;
            // too far right
            if (p.X > r.X + r.Width)
                return false;
            // too far up
            if (p.Y < r.Y)
                return false;
            // too far down
            if (p.Y > r.Y + r.Height)
                return false;

            // inside
            return true;
            */
            #endregion
        }

        private void Control_MouseMove(object sender, MouseEventArgs e) {
            Point mousePosition = e.GetPosition(this._canvas);
            // set the _toNodeEllipse if can
            bool foundNode = false;
            foreach (var item in this._canvas.Children) {
                if (item is NodeEllipse nodeEllipse) {
                    if (CanvasAddConnectionLinePreview.IsPointWithinEllipse(mousePosition, nodeEllipse.Measurements)) {
                        if (nodeEllipse == this._fromNodeEllipse)
                            continue;
                        foundNode = true;
                        this._toNodeEllipse = nodeEllipse;
                        break;
                    }
                }
            }
            if (!foundNode)
                this._toNodeEllipse = null;

            this.SetCoordinates(new Point(mousePosition.X,
                                          mousePosition.Y));
        }

    }
}
