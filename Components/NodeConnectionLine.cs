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
    public class NodeConnectionLine {

        public NodeEllipse ToNodeEllipse, FromNodeEllipse;
        //private double _centerX;
        //private double _centerY;
        private readonly Line _line;
        private readonly Canvas _canvas;

        private readonly List<Line> _directionLines;

        public bool IsPartOfPath = false;

        public void UpdateColour() {
            if (this.IsPartOfPath) {
                this._line.Stroke = Brushes.Green;
                Canvas.SetZIndex(this._line, 0);

                this._directionLines[0].Stroke = Brushes.Green;
                this._directionLines[1].Stroke = Brushes.Green;
                Canvas.SetZIndex(this._directionLines[0], 0);
                Canvas.SetZIndex(this._directionLines[1], 0);
            } else {
                this._line.Stroke = Brushes.White;
                Canvas.SetZIndex(this._line, -1);

                this._directionLines[0].Stroke = Brushes.White;
                this._directionLines[1].Stroke = Brushes.White;
                Canvas.SetZIndex(this._directionLines[0], -1);
                Canvas.SetZIndex(this._directionLines[1], -1);
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
            //this._line.X1 = this.FromNodeEllipse.Center.X;
            //this._line.Y1 = this.FromNodeEllipse.Center.Y;
            //this._line.X2 = this.ToNodeEllipse.Center.X;
            //this._line.Y2 = this.ToNodeEllipse.Center.Y;



            //this._centerX = (this._line.X1 + this._line.X2) / 2;
            //this._centerY = (this._line.Y1 + this._line.Y2) / 2;
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

        public NodeConnectionLine(NodeEllipse fromNodeEllipse, NodeEllipse toNodeEllipse, Canvas c) {
            this.FromNodeEllipse = fromNodeEllipse;
            this.ToNodeEllipse = toNodeEllipse;
            this._canvas = c;

            this._line = new Line() {
                Stroke = System.Windows.Media.Brushes.White,
                StrokeThickness = 5,
            };

            Canvas.SetZIndex(this._line, -1);
            this._canvas.Children.Add(this._line);

            this._directionLines = new List<Line>() {
                new Line() {
                    Stroke = System.Windows.Media.Brushes.White,
                    StrokeThickness = 2,
                },
                new Line() {
                    Stroke = System.Windows.Media.Brushes.White,
                    StrokeThickness = 2,
                }
            };
            Canvas.SetZIndex(this._directionLines[0], -1);
            Canvas.SetZIndex(this._directionLines[1], -1);
            this._canvas.Children.Add(this._directionLines[0]);
            this._canvas.Children.Add(this._directionLines[1]);

            this.UpdatePosition();

        }
    }
}
