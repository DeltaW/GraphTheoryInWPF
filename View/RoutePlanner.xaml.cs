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

        //private double _canvasWidth;
        //private double _canvasHeight;

        //public double CanvasWidth {
        //    get { return this._canvasWidth; }
        //    set {
        //        this._canvasWidth = value;
        //        this.RaisePropertyChanged();
        //    }
        //}
        //public double CanvasHeight {
        //    get { return this._canvasHeight; }
        //    set {
        //        this._canvasHeight = value;
        //        this.RaisePropertyChanged();
        //    }
        //}

        private readonly RoutePlannerVM _rpvm;

        //public string SavePath;

        public RoutePlanner(Graph graph) {
            this.InitializeComponent();
            this._rpvm = new RoutePlannerVM(graph, this.AllPossibleRoutesTextBlock, this.RadioButtonAllRoutes, this.RadioButtonShortestRoute, this.ShortestRouteCanvas);
            this.DataContext = this._rpvm;
        }

        //public RoutePlanner(Graph graph, string savePath) {
        //    this.InitializeComponent();
        //    this._rpvm = new RoutePlannerVM(graph, this.AllPossibleRoutesTextBlock, this.RadioButtonAllRoutes, this.RadioButtonShortestRoute, this.ShortestRouteCanvas);
        //    this.DataContext = this._rpvm;
        //    this.SavePath = savePath;
        //}

        private void RouteButtonClick(Object sender, RoutedEventArgs e) {
            this._rpvm.OnNodeSelectorChanged();
        }

        private void Button_Click_Plus(Object sender, RoutedEventArgs e) {
            this._rpvm.ButtonPlus();
            this._rpvm.OnNodeSelectorChanged();
        }

        private void RadioButton_Checked(Object sender, RoutedEventArgs e) {
            if (this._rpvm != null)
                this._rpvm.OnNodeSelectorChanged();
        }
    }
}
