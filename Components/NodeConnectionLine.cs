using GraphTheory.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Controls;
using Brushes = System.Windows.Media.Brushes;

namespace GraphTheoryInWPF.Components {
    public class NodeConnectionLineOLD {

        public NodeEllipse ToNodeEllipse, FromNodeEllipse;
        private readonly Line _line;
        private readonly Canvas _canvas;

        private readonly List<Line> _directionLines;

        public bool IsPartOfPath = false;

        public void UpdateColour() {
            if (this.IsPartOfPath) {
                this._line.Stroke = new SolidColorBrush(
                    new System.Windows.Media.Color() {
                        A = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionPathBrushColour"]).A,
                        R = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionPathBrushColour"]).R,
                        G = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionPathBrushColour"]).G,
                        B = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionPathBrushColour"]).B,
                    });
                Canvas.SetZIndex(this._line, 0);

                this._directionLines[0].Stroke = new SolidColorBrush(
                    new System.Windows.Media.Color() {
                        A = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionPathBrushColour"]).A,
                        R = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionPathBrushColour"]).R,
                        G = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionPathBrushColour"]).G,
                        B = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionPathBrushColour"]).B,
                    });
                this._directionLines[1].Stroke = new SolidColorBrush(
                    new System.Windows.Media.Color() {
                        A = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionPathBrushColour"]).A,
                        R = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionPathBrushColour"]).R,
                        G = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionPathBrushColour"]).G,
                        B = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionPathBrushColour"]).B,
                    });
                //Canvas.SetZIndex(this._directionLines[0], 0);
                //Canvas.SetZIndex(this._directionLines[1], 0);
            } else {
                this._line.Stroke = new SolidColorBrush(
                    new System.Windows.Media.Color() {
                        A = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionNormalBrushColour"]).A,
                        R = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionNormalBrushColour"]).R,
                        G = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionNormalBrushColour"]).G,
                        B = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionNormalBrushColour"]).B,
                    });
                Canvas.SetZIndex(this._line, -1);

                this._directionLines[0].Stroke = new SolidColorBrush(
                    new System.Windows.Media.Color() {
                        A = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionNormalBrushColour"]).A,
                        R = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionNormalBrushColour"]).R,
                        G = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionNormalBrushColour"]).G,
                        B = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionNormalBrushColour"]).B,
                    });
                this._directionLines[1].Stroke = new SolidColorBrush(
                    new System.Windows.Media.Color() {
                        A = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionNormalBrushColour"]).A,
                        R = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionNormalBrushColour"]).R,
                        G = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionNormalBrushColour"]).G,
                        B = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionNormalBrushColour"]).B,
                    });
                //Canvas.SetZIndex(this._directionLines[0], 0);
                //Canvas.SetZIndex(this._directionLines[1], 0);
            }
        }

        private void UpdateArrowCoordinate(double degrees = 45) {
            Point p = this.GetDirectionArrowPoint();
            Point toPoint = new Point((int) this._line.X2, (int) this._line.Y2);
            Point rotatedPoint1 = this.RotatePointAroundAnotherPointByDegrees(p,toPoint,  degrees);
            Point rotatedPoint2 = this.RotatePointAroundAnotherPointByDegrees(p,toPoint, -degrees);

            this._directionLines[0].X1 = toPoint.X;
            this._directionLines[0].Y1 = toPoint.Y;
            this._directionLines[0].X2 = rotatedPoint1.X;
            this._directionLines[0].Y2 = rotatedPoint1.Y;

            this._directionLines[1].X1 = toPoint.X;
            this._directionLines[1].Y1 = toPoint.Y;
            this._directionLines[1].X2 = rotatedPoint2.X;
            this._directionLines[1].Y2 = rotatedPoint2.Y;
        }

        private Point GetDirectionArrowPoint() {
            System.Windows.Vector vector = new System.Windows.Vector(this._line.X2 - this._line.X1,
                                                                     this._line.Y2 - this._line.Y1);
            System.Windows.Vector desiredVector = vector;
            desiredVector.Normalize();
            double desiredLength = 10/*(vector - desiredVector).Length * .1*/;
            vector -= (desiredVector * desiredLength);

            return new Point((int) (this._line.X1 + vector.X),
                             (int) (this._line.Y1 + vector.Y));
        }

        private Point RotatePointAroundAnotherPointByRadians(Point p, Point o, double radians) {
            return new Point((int) (Math.Cos(radians) * (p.X - o.X) - Math.Sin(radians) * (p.Y - o.Y) + o.X),
                             (int) (Math.Sin(radians) * (p.X - o.X) + Math.Cos(radians) * (p.Y - o.Y) + o.Y));
        }

        private Point RotatePointAroundAnotherPointByDegrees(Point p, Point o, double degrees) {
            return this.RotatePointAroundAnotherPointByRadians(p, o, degrees * (Math.PI / 180)); ;
        }

        public void UpdatePosition() {
            System.Windows.Point fromPoint = this.FromNodeEllipse.GetClosestBorderPoint(this.ToNodeEllipse.Center);
            System.Windows.Point toPoint = this.ToNodeEllipse.GetClosestBorderPoint(this.FromNodeEllipse.Center);

            this._line.X1 = fromPoint.X;
            this._line.Y1 = fromPoint.Y;
            this._line.X2 = toPoint.X;
            this._line.Y2 = toPoint.Y;

            this.UpdateArrowCoordinate();
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

        public NodeConnectionLineOLD(NodeEllipse fromNodeEllipse, NodeEllipse toNodeEllipse, Canvas c) {
            this.FromNodeEllipse = fromNodeEllipse;
            this.ToNodeEllipse = toNodeEllipse;
            this._canvas = c;

            this._line = new Line() {
                Stroke = new SolidColorBrush(new System.Windows.Media.Color {
                    A = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionNormalBrushColour"]).A,
                    R = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionNormalBrushColour"]).R,
                    G = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionNormalBrushColour"]).G,
                    B = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionNormalBrushColour"]).B,
                }),
                StrokeThickness = 3,
            };

            Canvas.SetZIndex(this._line, -1);
            this._canvas.Children.Add(this._line);

            this._directionLines = new List<Line>() {
                new Line() {
                    Stroke = new SolidColorBrush(new System.Windows.Media.Color {
                        A = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionNormalBrushColour"]).A,
                        R = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionNormalBrushColour"]).R,
                        G = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionNormalBrushColour"]).G,
                        B = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionNormalBrushColour"]).B,
                    }),
                    StrokeThickness = 3,
                },
                new Line() {
                    Stroke = new SolidColorBrush(new System.Windows.Media.Color {
                        A = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionNormalBrushColour"]).A,
                        R = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionNormalBrushColour"]).R,
                        G = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionNormalBrushColour"]).G,
                        B = ((System.Drawing.Color) Properties.Settings.Default["NodeConnectionNormalBrushColour"]).B,
                    }),
                    StrokeThickness = 3,
                }
            };
            Canvas.SetZIndex(this._directionLines[0], 2);
            Canvas.SetZIndex(this._directionLines[1], 2);
            this._canvas.Children.Add(this._directionLines[0]);
            this._canvas.Children.Add(this._directionLines[1]);

            this.UpdatePosition();

        }
    }
}
