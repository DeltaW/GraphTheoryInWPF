using GraphTheory.Core;
using GraphTheoryInWPF.Components;
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

namespace GraphTheoryInWPF.View {
    /// <summary>
    /// Interaction logic for SettingsEditor.xaml
    /// </summary>
    public partial class SettingsEditor: UserControl {

        public int MinNodeEllipsePadding { get; set; }
        public int MaxNodeEllipsePadding { get; set; }
        public int ExtraPaddingPerConnection { get; set; }
        public int NodeEllipseStrokeThickness { get; set; }
        public int ConnectionStrokeThickness { get; set; }
        public int NodeEllipseFontSize { get; set; }
        public int ArrowHeadLength { get; set; }
        public bool UseDynamicNodeEllipsePadding { get; set; }

        public System.Drawing.Color EllipseFillColour;
        public System.Drawing.Color EllipseStrokeColour;
        public System.Drawing.Color EllipseTextColour;
        public System.Drawing.Color ConnectionColour;
        public System.Drawing.Color RouteColour;
        public System.Drawing.Color CanvasBackgroundColour;
        public System.Drawing.Color CanvasNodeConnectionPreviewColour;

        private readonly Graph _graph;

        public SettingsEditor(Graph graph) {
            this.InitializeComponent();
            this._graph = graph;

            // Padding stuff
            this.MinNodeEllipsePadding = (int) Properties.Settings.Default["MinNodeEllipsePadding"];
            this.MaxNodeEllipsePadding = (int) Properties.Settings.Default["MaxNodeEllipsePadding"];
            this.ExtraPaddingPerConnection = (int) Properties.Settings.Default["ExtraPaddingPerConnection"];
            this.UseDynamicNodeEllipsePadding = (bool) Properties.Settings.Default["UseDynamicNodeEllipsePadding"];
            this.NodeEllipseFontSize = (int) Properties.Settings.Default["FontSize"];
            this.NodeEllipseStrokeThickness = (int) Properties.Settings.Default["NodeEllipseStrokeThickness"];
            this.ConnectionStrokeThickness = (int) Properties.Settings.Default["ConnectionStrokeThickness"];
            this.ArrowHeadLength = (int) Properties.Settings.Default["ConnectionArrowHeadLength"];

            // Colours
            this.EllipseFillColour = (System.Drawing.Color) Properties.Settings.Default["NodeEllipseFillBrushColour"];
            this.EllipseStrokeColour = (System.Drawing.Color) Properties.Settings.Default["NodeEllipseStrokeBrushColour"];
            this.EllipseTextColour = (System.Drawing.Color) Properties.Settings.Default["NodeEllipseTextBrushColour"];
            this.ConnectionColour = (System.Drawing.Color) Properties.Settings.Default["NodeConnectionNormalBrushColour"];
            this.RouteColour = (System.Drawing.Color) Properties.Settings.Default["NodeConnectionPathBrushColour"];
            this.CanvasBackgroundColour = (System.Drawing.Color) Properties.Settings.Default["CanvasBackgroundBrushColour"];
            this.CanvasNodeConnectionPreviewColour = (System.Drawing.Color) Properties.Settings.Default["CanvasNodeConnectionPreviewBrushColour"];

            // Padding Stuff
            this.TextBox_ExtraPaddingPerConnection.Text = ((int) Properties.Settings.Default["ExtraPaddingPerConnection"]).ToString();
            this.TextBox_MinNodeEllipsePadding.Text = ((int) Properties.Settings.Default["MinNodeEllipsePadding"]).ToString();
            this.TextBox_MaxNodeEllipsePadding.Text = ((int) Properties.Settings.Default["MaxNodeEllipsePadding"]).ToString();
            this.CheckBox_UseDynamicNodeEllipsePadding.IsChecked = (bool) Properties.Settings.Default["UseDynamicNodeEllipsePadding"];
            this.TextBox_NodeEllipseFontSize.Text = ((int) Properties.Settings.Default["FontSize"]).ToString();
            this.TextBox_NodeEllipseStrokeThickness.Text = ((int) Properties.Settings.Default["NodeEllipseStrokeThickness"]).ToString();
            this.TextBox_ConnectionStrokeThickness.Text = ((int) Properties.Settings.Default["ConnectionStrokeThickness"]).ToString();
            this.TextBox_ArrowHeadLength.Text = ((int) Properties.Settings.Default["ConnectionArrowHeadLength"]).ToString();

            // Colours
            this.EllipseFillPreview.Background = new SolidColorBrush(new System.Windows.Media.Color() {
                A = ((System.Drawing.Color) (Properties.Settings.Default["NodeEllipseFillBrushColour"])).A,
                R = ((System.Drawing.Color) (Properties.Settings.Default["NodeEllipseFillBrushColour"])).R,
                G = ((System.Drawing.Color) (Properties.Settings.Default["NodeEllipseFillBrushColour"])).G,
                B = ((System.Drawing.Color) (Properties.Settings.Default["NodeEllipseFillBrushColour"])).B,
            });
            this.EllipseStrokePreview.Background = new SolidColorBrush(new System.Windows.Media.Color() {
                A = ((System.Drawing.Color) (Properties.Settings.Default["NodeEllipseStrokeBrushColour"])).A,
                R = ((System.Drawing.Color) (Properties.Settings.Default["NodeEllipseStrokeBrushColour"])).R,
                G = ((System.Drawing.Color) (Properties.Settings.Default["NodeEllipseStrokeBrushColour"])).G,
                B = ((System.Drawing.Color) (Properties.Settings.Default["NodeEllipseStrokeBrushColour"])).B,
            });
            this.EllipseTextPreview.Background = new SolidColorBrush(new System.Windows.Media.Color() {
                A = ((System.Drawing.Color) (Properties.Settings.Default["NodeEllipseTextBrushColour"])).A,
                R = ((System.Drawing.Color) (Properties.Settings.Default["NodeEllipseTextBrushColour"])).R,
                G = ((System.Drawing.Color) (Properties.Settings.Default["NodeEllipseTextBrushColour"])).G,
                B = ((System.Drawing.Color) (Properties.Settings.Default["NodeEllipseTextBrushColour"])).B,
            });
            this.ConnectionPreview.Background = new SolidColorBrush(new System.Windows.Media.Color() {
                A = ((System.Drawing.Color) (Properties.Settings.Default["NodeConnectionNormalBrushColour"])).A,
                R = ((System.Drawing.Color) (Properties.Settings.Default["NodeConnectionNormalBrushColour"])).R,
                G = ((System.Drawing.Color) (Properties.Settings.Default["NodeConnectionNormalBrushColour"])).G,
                B = ((System.Drawing.Color) (Properties.Settings.Default["NodeConnectionNormalBrushColour"])).B,
            });
            this.RoutePreview.Background = new SolidColorBrush(new System.Windows.Media.Color() {
                A = ((System.Drawing.Color) (Properties.Settings.Default["NodeConnectionPathBrushColour"])).A,
                R = ((System.Drawing.Color) (Properties.Settings.Default["NodeConnectionPathBrushColour"])).R,
                G = ((System.Drawing.Color) (Properties.Settings.Default["NodeConnectionPathBrushColour"])).G,
                B = ((System.Drawing.Color) (Properties.Settings.Default["NodeConnectionPathBrushColour"])).B,
            });
            this.CanvasBackgroundPreview.Background = new SolidColorBrush(new System.Windows.Media.Color() {
                A = ((System.Drawing.Color) (Properties.Settings.Default["CanvasBackgroundBrushColour"])).A,
                R = ((System.Drawing.Color) (Properties.Settings.Default["CanvasBackgroundBrushColour"])).R,
                G = ((System.Drawing.Color) (Properties.Settings.Default["CanvasBackgroundBrushColour"])).G,
                B = ((System.Drawing.Color) (Properties.Settings.Default["CanvasBackgroundBrushColour"])).B,
            });
            this.CanvasNodeConnectionPreview.Background = new SolidColorBrush(new System.Windows.Media.Color() {
                A = ((System.Drawing.Color) (Properties.Settings.Default["CanvasNodeConnectionPreviewBrushColour"])).A,
                R = ((System.Drawing.Color) (Properties.Settings.Default["CanvasNodeConnectionPreviewBrushColour"])).R,
                G = ((System.Drawing.Color) (Properties.Settings.Default["CanvasNodeConnectionPreviewBrushColour"])).G,
                B = ((System.Drawing.Color) (Properties.Settings.Default["CanvasNodeConnectionPreviewBrushColour"])).B,
            });

            // Fill the Canvas
            this.OnSettingsChanged();

        }

        private void OnSettingsChanged() {
            this.GraphPreviewCanvas.Children.Clear();
            NodeEllipse.FillCanvasWithAllNodes(this.GraphPreviewCanvas, this._graph, this);
        }

        public void AskToSaveSettings() {
            MessageBoxResult result = MessageBox.Show("Would you like to save any changes made to the settings?",
                                                      "GraphTheory", MessageBoxButton.YesNo, MessageBoxImage.Question);

            switch (result) {
                case MessageBoxResult.Yes:
                    this.Button_Click_SaveSettings(null, null);
                    break;
                case MessageBoxResult.No:
                    break;
                default:
                    break;
            }
        }

        public bool WereSettingsChanged() {
            bool a = this.MinNodeEllipsePadding == (int) Properties.Settings.Default["MinNodeEllipsePadding"];
            bool b = this.MaxNodeEllipsePadding == (int) Properties.Settings.Default["MaxNodeEllipsePadding"];
            bool c = this.ExtraPaddingPerConnection == (int) Properties.Settings.Default["ExtraPaddingPerConnection"];
            bool d = this.UseDynamicNodeEllipsePadding == (bool) Properties.Settings.Default["UseDynamicNodeEllipsePadding"];
            bool e = this.EllipseFillColour == (System.Drawing.Color) Properties.Settings.Default["NodeEllipseFillBrushColour"];
            bool f = this.EllipseStrokeColour == (System.Drawing.Color) Properties.Settings.Default["NodeEllipseStrokeBrushColour"];
            bool g = this.EllipseTextColour == (System.Drawing.Color) Properties.Settings.Default["NodeEllipseTextBrushColour"];
            bool h = this.ConnectionColour == (System.Drawing.Color) Properties.Settings.Default["NodeConnectionNormalBrushColour"];
            bool i = this.RouteColour == (System.Drawing.Color) Properties.Settings.Default["NodeConnectionPathBrushColour"];
            bool j = this.CanvasBackgroundColour == (System.Drawing.Color) Properties.Settings.Default["CanvasBackgroundBrushColour"];
            bool k = this.CanvasNodeConnectionPreviewColour == (System.Drawing.Color) Properties.Settings.Default["CanvasNodeConnectionPreviewBrushColour"];
            bool l = this.NodeEllipseFontSize == (int) Properties.Settings.Default["FontSize"];
            bool m = this.NodeEllipseStrokeThickness == (int) Properties.Settings.Default["NodeEllipseStrokeThickness"];
            bool n = this.ConnectionStrokeThickness == (int) Properties.Settings.Default["ConnectionStrokeThickness"];
            bool o = this.ArrowHeadLength == (int) Properties.Settings.Default["ConnectionArrowHeadLength"];
            return !(a && b && c && d && e && f && g && h && i && j && k && l && m && n && o);
        }

        public void ResetSettings() {
            // Padding stuff
            Properties.Settings.Default["MinNodeEllipsePadding"] = this.MinNodeEllipsePadding;
            Properties.Settings.Default["MaxNodeEllipsePadding"] = this.MaxNodeEllipsePadding;
            Properties.Settings.Default["ExtraPaddingPerConnection"] = this.ExtraPaddingPerConnection;
            Properties.Settings.Default["UseDynamicNodeEllipsePadding"] = this.UseDynamicNodeEllipsePadding;

            Properties.Settings.Default["FontSize"] = this.NodeEllipseFontSize;
            Properties.Settings.Default["NodeEllipseStrokeThickness"] = this.NodeEllipseStrokeThickness;
            Properties.Settings.Default["ConnectionStrokeThickness"] = this.ConnectionStrokeThickness;
            Properties.Settings.Default["ConnectionArrowHeadLength"] = this.ArrowHeadLength;

            // Colours
            Properties.Settings.Default["NodeEllipseFillBrushColour"] = this.EllipseFillColour;
            Properties.Settings.Default["NodeEllipseStrokeBrushColour"] = this.EllipseStrokeColour;
            Properties.Settings.Default["NodeEllipseTextBrushColour"] = this.EllipseTextColour;
            Properties.Settings.Default["NodeConnectionNormalBrushColour"] = this.ConnectionColour;
            Properties.Settings.Default["NodeConnectionPathBrushColour"] = this.RouteColour;
            Properties.Settings.Default["CanvasBackgroundBrushColour"] = this.CanvasBackgroundColour;
            Properties.Settings.Default["CanvasNodeConnectionPreviewBrushColour"] = this.CanvasNodeConnectionPreviewColour;
        }

        private void Button_Click_SaveSettings(object sender, RoutedEventArgs e) {
            // Padding stuff
            this.MinNodeEllipsePadding = (int) Properties.Settings.Default["MinNodeEllipsePadding"];
            this.MaxNodeEllipsePadding = (int) Properties.Settings.Default["MaxNodeEllipsePadding"];
            this.ExtraPaddingPerConnection = (int) Properties.Settings.Default["ExtraPaddingPerConnection"];
            this.UseDynamicNodeEllipsePadding = (bool) Properties.Settings.Default["UseDynamicNodeEllipsePadding"];

            this.NodeEllipseFontSize = (int) Properties.Settings.Default["FontSize"];
            this.NodeEllipseStrokeThickness = (int) Properties.Settings.Default["NodeEllipseStrokeThickness"];
            this.ConnectionStrokeThickness = (int) Properties.Settings.Default["ConnectionStrokeThickness"];
            this.ArrowHeadLength = (int) Properties.Settings.Default["ConnectionArrowHeadLength"];

            // Colours
            this.EllipseFillColour = (System.Drawing.Color) Properties.Settings.Default["NodeEllipseFillBrushColour"];
            this.EllipseStrokeColour = (System.Drawing.Color) Properties.Settings.Default["NodeEllipseStrokeBrushColour"];
            this.EllipseTextColour = (System.Drawing.Color) Properties.Settings.Default["NodeEllipseTextBrushColour"];
            this.ConnectionColour = (System.Drawing.Color) Properties.Settings.Default["NodeConnectionNormalBrushColour"];
            this.RouteColour = (System.Drawing.Color) Properties.Settings.Default["NodeConnectionPathBrushColour"];
            this.CanvasBackgroundColour = (System.Drawing.Color) Properties.Settings.Default["CanvasBackgroundBrushColour"];
            this.CanvasNodeConnectionPreviewColour = (System.Drawing.Color) Properties.Settings.Default["CanvasNodeConnectionPreviewBrushColour"];

            Properties.Settings.Default.Save();

            MessageBox.Show("Save successfull!",
                            "GraphTheory", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e) {
            if ((TextBox) sender == this.TextBox_MinNodeEllipsePadding && int.TryParse(this.TextBox_MinNodeEllipsePadding.Text, out int minPadding)) {
                Properties.Settings.Default["MinNodeEllipsePadding"] = minPadding;
                this.OnSettingsChanged();
                return;
            }
            if ((TextBox) sender == this.TextBox_MaxNodeEllipsePadding && int.TryParse(this.TextBox_MaxNodeEllipsePadding.Text, out int maxPadding)) {
                Properties.Settings.Default["MaxNodeEllipsePadding"] = maxPadding;
                this.OnSettingsChanged();
                return;
            }
            if ((TextBox) sender == this.TextBox_ExtraPaddingPerConnection && int.TryParse(this.TextBox_ExtraPaddingPerConnection.Text, out int extraPadding)) {
                Properties.Settings.Default["ExtraPaddingPerConnection"] = extraPadding;
                this.OnSettingsChanged();
                return;
            }
            if ((TextBox) sender == this.TextBox_ConnectionStrokeThickness && int.TryParse(this.TextBox_ConnectionStrokeThickness.Text, out int connectionStrokeThickness)) {
                Properties.Settings.Default["ConnectionStrokeThickness"] = connectionStrokeThickness;
                this.OnSettingsChanged();
                return;
            }
            if ((TextBox) sender == this.TextBox_NodeEllipseStrokeThickness && int.TryParse(this.TextBox_NodeEllipseStrokeThickness.Text, out int ellipseStrokeThickness)) {
                Properties.Settings.Default["NodeEllipseStrokeThickness"] = ellipseStrokeThickness;
                this.OnSettingsChanged();
                return;
            }
            if ((TextBox) sender == this.TextBox_NodeEllipseFontSize && int.TryParse(this.TextBox_NodeEllipseFontSize.Text, out int fontSize)) {
                Properties.Settings.Default["FontSize"] = fontSize;
                this.OnSettingsChanged();
                return;
            }
            if ((TextBox) sender == this.TextBox_ArrowHeadLength && int.TryParse(this.TextBox_ArrowHeadLength.Text, out int arrowLength)) {
                Properties.Settings.Default["ConnectionArrowHeadLength"] = arrowLength;
                this.OnSettingsChanged();
                return;
            }
        }

        private void CheckBox_UseDynamicNodeEllipsePadding_Changed(object sender, RoutedEventArgs e) {
            Properties.Settings.Default["UseDynamicNodeEllipsePadding"] = (bool) ((CheckBox) sender).IsChecked;
            this.OnSettingsChanged();
        }

        #region Colours
        private void ListBoxItem_MouseDoubleClick_EllipseFillColour(object sender, MouseButtonEventArgs e) {
            // Ellipse Fill Colour
            System.Windows.Forms.ColorDialog temp = new System.Windows.Forms.ColorDialog {
                Color = (System.Drawing.Color) Properties.Settings.Default["NodeEllipseFillBrushColour"]
            };
            if (temp.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                Properties.Settings.Default["NodeEllipseFillBrushColour"] = temp.Color;
                this.EllipseFillPreview.Background = new SolidColorBrush(new System.Windows.Media.Color() {
                    A = temp.Color.A,
                    R = temp.Color.R,
                    G = temp.Color.G,
                    B = temp.Color.B,
                });
                this.OnSettingsChanged();
            }
        }
        private void ListBoxItem_MouseDoubleClick_EllipseStrokeColour(object sender, MouseButtonEventArgs e) {
            // Ellipse Stroke Colour
            System.Windows.Forms.ColorDialog temp = new System.Windows.Forms.ColorDialog {
                Color = (System.Drawing.Color) Properties.Settings.Default["NodeEllipseStrokeBrushColour"]
            };
            if (temp.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                Properties.Settings.Default["NodeEllipseStrokeBrushColour"] = temp.Color;
                this.EllipseStrokePreview.Background = new SolidColorBrush(new System.Windows.Media.Color() {
                    A = temp.Color.A,
                    R = temp.Color.R,
                    G = temp.Color.G,
                    B = temp.Color.B,
                });
                this.OnSettingsChanged();
            }
        }
        private void ListBoxItem_MouseDoubleClick_TextColour(object sender, MouseButtonEventArgs e) {
            // Text Colour
            System.Windows.Forms.ColorDialog temp = new System.Windows.Forms.ColorDialog {
                Color = (System.Drawing.Color) Properties.Settings.Default["NodeEllipseTextBrushColour"]
            };
            if (temp.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                Properties.Settings.Default["NodeEllipseTextBrushColour"] = temp.Color;
                this.EllipseTextPreview.Background = new SolidColorBrush(new System.Windows.Media.Color() {
                    A = temp.Color.A,
                    R = temp.Color.R,
                    G = temp.Color.G,
                    B = temp.Color.B,
                });
                this.OnSettingsChanged();
            }
        }
        private void ListBoxItem_MouseDoubleClick_ConnectionColour(object sender, MouseButtonEventArgs e) {
            // Connection Colour
            System.Windows.Forms.ColorDialog temp = new System.Windows.Forms.ColorDialog {
                Color = (System.Drawing.Color) Properties.Settings.Default["NodeConnectionNormalBrushColour"]
            };
            if (temp.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                Properties.Settings.Default["NodeConnectionNormalBrushColour"] = temp.Color;
                this.ConnectionPreview.Background = new SolidColorBrush(new System.Windows.Media.Color() {
                    A = temp.Color.A,
                    R = temp.Color.R,
                    G = temp.Color.G,
                    B = temp.Color.B,
                });
                this.OnSettingsChanged();
            }
        }
        private void ListBoxItem_MouseDoubleClick_RouteColour(object sender, MouseButtonEventArgs e) {
            // Route Colour
            System.Windows.Forms.ColorDialog temp = new System.Windows.Forms.ColorDialog {
                Color = (System.Drawing.Color) Properties.Settings.Default["NodeConnectionPathBrushColour"]
            };
            if (temp.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                Properties.Settings.Default["NodeConnectionPathBrushColour"] = temp.Color;
                this.RoutePreview.Background = new SolidColorBrush(new System.Windows.Media.Color() {
                    A = temp.Color.A,
                    R = temp.Color.R,
                    G = temp.Color.G,
                    B = temp.Color.B,
                });
                this.OnSettingsChanged();
            }
        }
        private void ListBoxItem_MouseDoubleClick_CanvasBackgroundColour(object sender, MouseButtonEventArgs e) {
            // Canvas Background Colour
            System.Windows.Forms.ColorDialog temp = new System.Windows.Forms.ColorDialog {
                Color = (System.Drawing.Color) Properties.Settings.Default["CanvasBackgroundBrushColour"]
            };
            if (temp.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                Properties.Settings.Default["CanvasBackgroundBrushColour"] = temp.Color;
                this.CanvasBackgroundPreview.Background = new SolidColorBrush(new System.Windows.Media.Color() {
                    A = temp.Color.A,
                    R = temp.Color.R,
                    G = temp.Color.G,
                    B = temp.Color.B,
                });
                this.OnSettingsChanged();
            }
        }
        private void ListBoxItem_MouseDoubleClick_CanvasNodeConnectionPreviewColour(object sender, MouseButtonEventArgs e) {
            // Canvas Node Connection Preview Colour
            System.Windows.Forms.ColorDialog temp = new System.Windows.Forms.ColorDialog {
                Color = (System.Drawing.Color) Properties.Settings.Default["CanvasNodeConnectionPreviewBrushColour"]
            };
            if (temp.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                Properties.Settings.Default["CanvasNodeConnectionPreviewBrushColour"] = temp.Color;
                this.CanvasNodeConnectionPreview.Background = new SolidColorBrush(new System.Windows.Media.Color() {
                    A = temp.Color.A,
                    R = temp.Color.R,
                    G = temp.Color.G,
                    B = temp.Color.B,
                });
                this.OnSettingsChanged();
            }
        }
        #endregion

        private void GraphPreviewCanvas_ContextMenuOpening(object sender, ContextMenuEventArgs e) {
            this.p = Mouse.GetPosition(this.GraphPreviewCanvas);
        }

        private Point p; // temporary point to get the position used when creating a new node via the canvas - ugly but it works

        private void MenuItem_Click(object sender, RoutedEventArgs e) {
            string nodeName = Microsoft.VisualBasic.Interaction.InputBox("Please type in a uniqe Node name", "GraphTheory", "");
            try {
                this._graph.AddNewNodeToGraph(nodeName, new System.Drawing.Point((int) this.p.X, (int) this.p.Y));
                this.GraphPreviewCanvas.Children.Clear();
                NodeEllipse.FillCanvasWithAllNodes(this.GraphPreviewCanvas, this._graph, this);
            } catch (GraphException ge) {
                MessageBox.Show(ge.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
