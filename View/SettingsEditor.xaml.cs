using GraphTheory.Core;
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
            if (int.TryParse(this.TextBox_MinNodeEllipsePadding.Text, out int minPadding)) {
                Properties.Settings.Default["MinNodeEllipsePadding"] = minPadding;
            }
            if (int.TryParse(this.TextBox_MaxNodeEllipsePadding.Text, out int maxPadding)) {
                Properties.Settings.Default["MaxNodeEllipsePadding"] = maxPadding;
            }
            if (int.TryParse(this.TextBox_ExtraPaddingPerConnection.Text, out int extraPadding)) {
                Properties.Settings.Default["ExtraPaddingPerConnection"] = extraPadding;
            }
        }

        private void CheckBox_UseDynamicNodeEllipsePadding_Changed(object sender, RoutedEventArgs e) {
            Properties.Settings.Default["UseDynamicNodeEllipsePadding"] = (bool) ((CheckBox) sender).IsChecked;
        }
    }
}
