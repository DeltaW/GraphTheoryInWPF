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
        public bool UseDynamicNodeEllipsePadding { get; set; }

        private readonly Graph _graph;

        public SettingsEditor(Graph graph) {
            this.InitializeComponent();
            this._graph = graph;

            this.MinNodeEllipsePadding = (int) Properties.Settings.Default["MinNodeEllipsePadding"];
            this.MaxNodeEllipsePadding = (int) Properties.Settings.Default["MaxNodeEllipsePadding"];
            this.ExtraPaddingPerConnection = (int) Properties.Settings.Default["ExtraPaddingPerConnection"];
            this.UseDynamicNodeEllipsePadding = (bool) Properties.Settings.Default["UseDynamicNodeEllipsePadding"];

            this.TextBox_ExtraPaddingPerConnection.Text = ((int) Properties.Settings.Default["ExtraPaddingPerConnection"]).ToString();
            this.TextBox_MinNodeEllipsePadding.Text = ((int) Properties.Settings.Default["MinNodeEllipsePadding"]).ToString();
            this.TextBox_MaxNodeEllipsePadding.Text = ((int) Properties.Settings.Default["MaxNodeEllipsePadding"]).ToString();
            this.CheckBox_UseDynamicNodeEllipsePadding.IsChecked = (bool) Properties.Settings.Default["UseDynamicNodeEllipsePadding"];

            // Fill the Canvas
            this.OnSettingsChanged();

        }

        private void OnSettingsChanged() {
            this.GraphPreviewCanvas.Children.Clear();
            NodeEllipse.FillCanvasWithAllNodes(this.GraphPreviewCanvas, this._graph);
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
            return !(a && b && c && d);
        }

        public void ResetSettings() {
            Properties.Settings.Default["MinNodeEllipsePadding"] = this.MinNodeEllipsePadding;
            Properties.Settings.Default["MaxNodeEllipsePadding"] = this.MaxNodeEllipsePadding;
            Properties.Settings.Default["ExtraPaddingPerConnection"] = this.ExtraPaddingPerConnection;
            Properties.Settings.Default["UseDynamicNodeEllipsePadding"] = this.UseDynamicNodeEllipsePadding;
        }

        private void Button_Click_SaveSettings(object sender, RoutedEventArgs e) {
            this.MinNodeEllipsePadding = (int) Properties.Settings.Default["MinNodeEllipsePadding"];
            this.MaxNodeEllipsePadding = (int) Properties.Settings.Default["MaxNodeEllipsePadding"];
            this.ExtraPaddingPerConnection = (int) Properties.Settings.Default["ExtraPaddingPerConnection"];
            this.UseDynamicNodeEllipsePadding = (bool) Properties.Settings.Default["UseDynamicNodeEllipsePadding"];

            Properties.Settings.Default.Save();
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
        }

        private void CheckBox_UseDynamicNodeEllipsePadding_Changed(object sender, RoutedEventArgs e) {
            Properties.Settings.Default["UseDynamicNodeEllipsePadding"] = (bool) ((CheckBox) sender).IsChecked;
            this.OnSettingsChanged();
        }
    }
}
