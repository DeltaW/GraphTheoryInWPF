using GraphTheory.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for NodeEllipse.xaml
    /// </summary>
    public partial class NodeEllipse: UserControl, INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged([CallerMemberName] string property = "")
               => PropertyChanged(this, new PropertyChangedEventArgs(property));

        private readonly Canvas _canvas;
        private Ellipse _ellipse;
        private TextBlock _textBlock;
        private Rect _measurements;
        private readonly Node _node;
        private readonly Graph _graph;
        private readonly List<NodeConnectionLine> _lines = new List<NodeConnectionLine>();

        public List<Point> GetBorderPoints() {
            return new List<Point>() { 
                // oben links
                new Point(this.Measurements.X + this.Measurements.Width / 4,
                          this.Measurements.Y /*+ this.Measurements.Height / 4*/),

                // oben mitte
                new Point(this.Measurements.X + this.Measurements.Width / 2,
                          this.Measurements.Y),
                
                // oben rechts
                new Point(this.Measurements.X + this.Measurements.Width * .75,
                          this.Measurements.Y /*+ this.Measurements.Height / 4*/),
                
                // rechts mitte
                new Point(this.Measurements.X + this.Measurements.Width,
                          this.Measurements.Y + this.Measurements.Height / 2),

                // rechts unten
                new Point(this.Measurements.X + this.Measurements.Width * .75,
                          this.Measurements.Y + this.Measurements.Height/* * .75*/),

                // unten mitte
                new Point(this.Measurements.X + this.Measurements.Width / 2,
                          this.Measurements.Y + this.Measurements.Height),

                // unten links
                new Point(this.Measurements.X + this.Measurements.Width / 4,
                          this.Measurements.Y + this.Measurements.Height/* * .75*/),

                // links mitte
                new Point(this.Measurements.X,
                          this.Measurements.Y + this.Measurements.Height / 2),
            };
        }

        public Node GetNode() => this._node;

        public Point GetClosestBorderPointNotWorking(Point point) {

            #region Schnittpunkte
            double xs1, xs2, ys1, ys2;
            #endregion;

            #region Ellipsen-Parameter
            // Halbachse
            double a = this.Measurements.Width / 2;
            double b = this.Measurements.Height / 2;

            // Mittelpunkt
            double m1 = this.Center.X;
            double m2 = this.Center.Y;
            #endregion

            #region Geraden-Parameter
            // Steigung
            double m0;
            // Y-Achsenabschnitt
            double d;

            // Zwei Punkte auf der Geraden
            double x1 = point.X;
            double y1 = point.Y;
            double x2 = m1;
            double y2 = m2;

            // Differenzen zur Berechnung der Geradensteigung
            double dx = x2 - x1;
            double dy = y2 - y1;
            #endregion

            //double dxa = Math.Abs(dx);
            double eps = 0.0000001;
            if (Math.Abs(dx) < eps) { // Senkrechte Gerade um Division durch 0 zu vermeiden
                // Schnittpunkt 1
                xs1 = x1;
                ys1 = m2 - b;
                // Schnittpunkt 2
                xs2 = x2;
                ys2 = m2 + b;
            } else {
                // Steigung
                m0 = dy / dx;
                // Y-Achsenabschnitt
                d = y2 - m0 * x2;

                // Hilfswerte:
                double g = d - m2;
                double r = a * b;
                double m0q = m0 * m0;
                double m1q = m1 * m1;
                double rq = r * r;
                double aq = a * a;
                double bq = b * b;
                double gq = g * g;
                double f = aq * m0q + bq;

                // Parameter für Quadratische pq-Formel
                double p = 2 * (aq * m0 * g - bq * m1) / f;
                double q = (aq * gq + gq * m1q - rq) / f;
                double mp2 = -(p / 2);
                double w = Math.Sqrt(mp2 * mp2 - q);
                
                if (double.IsNaN(w)) {
                    ;
                }

                // Schnittpunkt 1
                xs1 = mp2 + w;
                ys1 = m0 * xs1 + d;
                // Schnittpunkt 2
                xs2 = mp2 - w;
                ys2 = m0 * xs2 + d;
            }

            Point s1 = new Point(xs1, ys1);
            Point s2 = new Point(xs2, ys2);

            if (double.IsNaN(s1.X) || double.IsNaN(s2.X) || double.IsNaN(s1.Y) || double.IsNaN(s2.Y)) {
                //return new Point(this.Center.X, this.Center.Y);
                throw (new Exception());
                //this.NodeEllipseCanvas.Background = Brushes.MediumVioletRed;
                //return this.GetClosestBorderPoint(point);
            }

            double d1 = new Vector(Math.Abs(point.X - s1.X), Math.Abs(point.Y - s1.Y)).Length;
            double d2 = new Vector(Math.Abs(point.X - s2.X), Math.Abs(point.Y - s2.Y)).Length;

            this.NodeEllipseCanvas.Background = Brushes.Transparent;
            return (d1 < d2) ? s1 : s2;
        }

        public Point GetClosestBorderPoint(Point p) {
            int closestBorderPointIndex = 0;
            double smallestDistance = double.PositiveInfinity;
            List<Point> borderPoints = this.GetBorderPoints();
            for (int i = 0; i < borderPoints.Count; i++) {
                double distance = new Vector(Math.Abs(p.X - borderPoints[i].X),
                                             Math.Abs(p.Y - borderPoints[i].Y)).Length;
                if (distance < smallestDistance) {
                    smallestDistance = distance;
                    closestBorderPointIndex = i;
                }
            }
            return borderPoints[closestBorderPointIndex];
        }

        public Point GetClosestBorderPoint(int x, int y) {
            return this.GetClosestBorderPoint(new Point(x, y));
        }


        public void UpdateConnectionColours(Route route) {
            // Update the colour
            foreach (NodeConnectionLine line in this._lines) {
                line.UpdateIsPath(route);
                line.UpdateColour();
            }
        }

        public void SetConnectionIsNotPath() {
            // Update the colour
            foreach (NodeConnectionLine line in this._lines) {
                line.IsPartOfPath = false;
                line.UpdateColour();
            }
        }

        public Point Center {
            get {
                return new Point(Canvas.GetLeft(this) + this.Measurements.Width / 2,
                                 Canvas.GetTop(this) + this.Measurements.Height / 2);
            }
        }

        public Rect Measurements {
            get { return this._measurements; }
            set {
                this._measurements = value;
                this.SetCoordinates(new Point(this._measurements.X, this._measurements.Y));
                this.RaisePropertyChanged();
            }
        }

        public static NodeEllipse GetNodeEllipseByName(Canvas c, string name) {
            foreach (var item in c.Children) {
                if (item is NodeEllipse nodeEllipse) {
                    if (nodeEllipse._node.Name == name)
                        return nodeEllipse;
                }
            }
            return null;
        }

        private void AddConnectionLine(Node toNode) {
            NodeConnectionLine connectionLine = new NodeConnectionLine(this,
                NodeEllipse.GetNodeEllipseByName(this._canvas, toNode.Name), this._canvas);
            this._lines.Add(connectionLine);
        }

        public void InstantiateConnectionLines() {
            foreach (Connection c in this._node.Connections) {
                this.AddConnectionLine(c.ToNode);
            }
        }

        public static Point GetEllipseWidthAndHeightBasedOnText(string text, int minDistanceToText = 10) {
            /* Calculates the width and heigth of a Textblock and returns them as a Point
             * Where Point.X represents the width and Point.Y the height
             * "text" will be the text that is to be displayed and it is used to measure the size of the shapes
             * "minDistanceToText" is the minimum Distance between the TextBlock's Rect and the Ellipse's Rect
             */

            // Create a TextBlock
            TextBlock textBlock = new TextBlock() {
                Text = text
            };

            // Apply the TextBlocks Size based on it's text
            textBlock.Measure(new Size(Double.PositiveInfinity, double.PositiveInfinity));
            textBlock.Arrange(new Rect(textBlock.DesiredSize));

            // Calculate and return the width and height as a Point while also considering the minimum distance
            return new Point(textBlock.ActualWidth + 2 * minDistanceToText,
                             textBlock.ActualHeight + 2 * minDistanceToText);
        }

        private void InstantiateContent(Point p, Brush strokeBrush, Brush textBrush, int minDistanceToText = 10, int zIndex = 3) {
            Point Size = NodeEllipse.GetEllipseWidthAndHeightBasedOnText(this._node.Name, minDistanceToText);

            this._measurements = new Rect() {
                X = p.X,
                Y = p.Y,
                Width = Size.X,
                Height = Size.Y
            };

            // Creating the Ellipse
            this._ellipse = new Ellipse() {
                Fill = this._canvas.Background,
                Stroke = strokeBrush,
                Width = this._measurements.Width,
                Height = this._measurements.Height
            };

            // Setting the Ellipse's coordinates
            Canvas.SetLeft(this._ellipse, 0);
            Canvas.SetTop(this._ellipse, 0);
            Canvas.SetZIndex(this._ellipse, zIndex);

            // Creating the TextBlock
            this._textBlock = new TextBlock() {
                Text = this._node.Name,
                TextAlignment = TextAlignment.Center,
                Foreground = textBrush,
                Width = Size.X - 2 * minDistanceToText,
                Height = Size.Y - 2 * minDistanceToText
            };

            //// Applying the appropriate width and height of the TextBlock
            //this._textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            //this._textBlock.Arrange(new Rect(this._textBlock.DesiredSize));


            // Setting the Textblock's coordinates
            Canvas.SetLeft(this._textBlock, 0 + minDistanceToText);
            Canvas.SetTop(this._textBlock, 0 + minDistanceToText);
            Canvas.SetZIndex(this._textBlock, zIndex + 1);

            // Drawing
            this.NodeEllipseCanvas.Children.Add(this._ellipse);
            this.NodeEllipseCanvas.Children.Add(this._textBlock);
        }

        public NodeEllipse(Canvas c, Graph graph, Node n, Point p, Brush strokeBrush, Brush textBrush, int minDistanceToText = 5, int zIndex = 3) {
            this.InitializeComponent();
            this.DataContext = this;

            this._canvas = c;
            this._graph = graph;
            this._node = n;

            //this.SetCoordinates(p);

            Canvas.SetLeft(this, p.X);
            Canvas.SetTop(this, p.Y);

            this.UpdateConnectionCoordinates();
            for (int i = 0; i < this._lines.Count; i++) {
                if (this._lines[i].ToNodeEllipse.GetNode().IsDirectlyConnectedToNode(this._node)) {
                    this._lines[i].ToNodeEllipse.UpdateConnectionCoordinates();
                }
            }

            Canvas.SetZIndex(this, 1);
            this.InstantiateContent(p, strokeBrush, textBrush, minDistanceToText, zIndex);

            this.MouseLeftButtonDown += new MouseButtonEventHandler(this.Control_MouseLeftButtonDown);
            this.MouseLeftButtonUp += new MouseButtonEventHandler(this.Control_MouseLeftButtonUp);
            this.MouseMove += new MouseEventHandler(this.Control_MouseMove);
        }

        private double Clamp(double value, double min, double max) {
            return (value < min) ? min : ((value > max) ? max : value);
        }

        public void UpdateConnectionCoordinates() {
            foreach (var item in this._lines) {
                item.UpdatePosition();
            }
        }

        public void SetCoordinates(Point p) {

            // Constrain the X Position
            p.X = this.Clamp((int) p.X, 0, this._canvas.ActualWidth - this._measurements.Width);

            // Constrain the Y Position
            p.Y = this.Clamp(p.Y, 0, this._canvas.ActualHeight - this._measurements.Height);

            this._measurements.X = p.X;
            this._measurements.Y = p.Y;

            p.X = Math.Max(0, p.X);
            p.Y = Math.Max(0, p.Y);

            this._node.Position = new System.Drawing.Point((int) p.X, (int) p.Y);

            Canvas.SetLeft(this, p.X);
            Canvas.SetTop(this, p.Y);

            // Fix the OneWayConnectionBug
            foreach (string name in this._graph.GetAllNodeNames()) {
                if (this._node.Name != name && this._graph.GetNode(name).IsDirectlyConnectedToNode(this._node)) {
                    NodeEllipse.GetNodeEllipseByName(this._canvas, name).UpdateConnectionCoordinates();
                }
            }

            this.UpdateConnectionCoordinates();
            for (int i = 0; i < this._lines.Count; i++) {
                if (this._lines[i].ToNodeEllipse.GetNode().IsDirectlyConnectedToNode(this._node)) {
                    this._lines[i].ToNodeEllipse.UpdateConnectionCoordinates();
                }
            }

        }

        private bool _isDragging;
        private Point _mouseLocationWithinMe;

        private void Control_MouseLeftButtonDown(object sender, MouseEventArgs e) {
            this._isDragging = true;
            this._mouseLocationWithinMe = e.GetPosition(this);
            this.CaptureMouse();
        }

        private void Control_MouseLeftButtonUp(object sender, MouseEventArgs e) {
            this._isDragging = false;
            this.ReleaseMouseCapture();
        }

        private void Control_MouseMove(object sender, MouseEventArgs e) {
            if (this._isDragging) {
                Point mouseWithinParent = e.GetPosition(this._canvas);

                this.SetCoordinates(new Point(mouseWithinParent.X - this._mouseLocationWithinMe.X,
                                              mouseWithinParent.Y - this._mouseLocationWithinMe.Y));
            }
        }
    }
}
