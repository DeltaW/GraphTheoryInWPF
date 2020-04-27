using GraphTheory.Core;
using GraphTheoryInWPF.ViewModel;
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

namespace GraphTheoryInWPF.View {
    /// <summary>
    /// Interaktionslogik für RoutePlanner.xaml
    /// </summary>
    public partial class RoutePlanner: UserControl, INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged([CallerMemberName] string property = "")
               => PropertyChanged(this, new PropertyChangedEventArgs(property));

        public readonly RoutePlannerVM RPVM;
        public readonly MainWindow _mainWindow;

        public RoutePlanner(Graph graph, MainWindow mainWindow) {
            this.InitializeComponent();
            this._mainWindow = mainWindow;
            this.RPVM = new RoutePlannerVM(graph, this);
            this.DataContext = this.RPVM;
        }

        private void RouteButtonClick(Object sender, RoutedEventArgs e) {
            this.RPVM.OnNodeSelectorChanged();
        }

        private void Button_Click_Plus(Object sender, RoutedEventArgs e) {
            this.RPVM.ButtonPlus();
            this.RPVM.OnNodeSelectorChanged();
        }

        private void RadioButton_Checked(Object sender, RoutedEventArgs e) {
            if (this.RPVM != null)
                this.RPVM.OnNodeSelectorChanged();
        }

        private void ShortestRouteCanvas_ContextMenuOpening(object sender, ContextMenuEventArgs e) {
            this.p = Mouse.GetPosition(this.ShortestRouteCanvas);

        }
        private Point p; // temporary point to get the position used when creating a new node via the canvas - ugly but it works

        private void MenuItem_Click(object sender, RoutedEventArgs e) {
            this.RPVM.MenuItemAddNode((int) p.X, (int) p.Y);

        }
    }
}
