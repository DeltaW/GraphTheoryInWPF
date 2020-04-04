using GraphTheoryInWPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaktionslogik für NodeSelector.xaml
    /// </summary>
    public partial class NodeSelector: UserControl, INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged([CallerMemberName] string property = "")
               => PropertyChanged(this, new PropertyChangedEventArgs(property));

        public string GetContent() {
            if (this.NodeSelectorComboBox.SelectedItem == null)
                return "elchFehlerMeldung";
            else {
                return this.NodeSelectorComboBox.SelectedItem.ToString();
            }
        }

        private readonly RoutePlannerVM _rpvm;
        private int _orderNumber;
        public int OrderNumber {
            get { return this._orderNumber; }
            set {
                this._orderNumber = value;
                this.LabelText = value + "";
            }
        }

        private string _labelText = "";

        public string LabelText {
            get { return this._labelText; }
            set {
                this._labelText = ((value == "0") ? "Start" : $"Goal {value}");
                this.RaisePropertyChanged();
            }
        }
        public ObservableCollection<string> NodeCollection {
            get { return (ObservableCollection<string>) this.GetValue(NodeCollectionProperty); }
            set { this.SetValue(NodeCollectionProperty, value); }
        }

        public NodeSelector() {
            this.InitializeComponent();
        }
        public NodeSelector(int ordernum, ObservableCollection<string> nodes, RoutePlannerVM rpvm) {
            this.InitializeComponent();
            this._rpvm = rpvm;
            NodeCollection = nodes;
            _orderNumber = ordernum;
            this._labelText = ((ordernum.ToString() == "0") ? "Start" : $"Goal {ordernum}");
        }

        public static readonly DependencyProperty NodeCollectionProperty =
                    DependencyProperty.Register("NodeCollection", typeof(ObservableCollection<string>), typeof(NodeSelector), new PropertyMetadata(new ObservableCollection<string>()));
        private void NodeSelectorComboBox_SelectionChanged(Object sender, SelectionChangedEventArgs e) {
            this._rpvm.OnNodeSelectorChanged();
        }
        private void Button_Click_MINUS(Object sender, RoutedEventArgs e) {
            if (this._rpvm.NodeSelectors.Count > 2) {
                this._rpvm.GoalCounter--;
                this._rpvm.NodeSelectors.Remove(this);
                this._rpvm.UpdateOrders();
                this._rpvm.OnNodeSelectorChanged();
            }
        }

        private void Button_Click_MoveUp(Object sender, RoutedEventArgs e) {
            if (this.OrderNumber != 0) {
                NodeSelector temp = this._rpvm.NodeSelectors[this.OrderNumber - 1];
                this._rpvm.NodeSelectors[this.OrderNumber - 1] = this;
                this._rpvm.NodeSelectors[this.OrderNumber] = temp;
                this._rpvm.UpdateOrders();
                this._rpvm.OnNodeSelectorChanged();
            }
        }

        private void Button_Click_MoveDown(Object sender, RoutedEventArgs e) {
            if (this.OrderNumber != this._rpvm.NodeSelectors.Count - 1) {
                NodeSelector temp = this._rpvm.NodeSelectors[this.OrderNumber + 1];
                this._rpvm.NodeSelectors[this.OrderNumber + 1] = this;
                this._rpvm.NodeSelectors[this.OrderNumber] = temp;
                this._rpvm.UpdateOrders();
                this._rpvm.OnNodeSelectorChanged();
            }
        }
    }
}
