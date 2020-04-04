using GraphTheory.Core;
using GraphTheoryInWPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraphTheoryInWPF.View {
    /// <summary>
    /// Interaction logic for GraphEditor.xaml
    /// </summary>
    public partial class GraphEditor: UserControl {
        private readonly GraphEditorVM _gevm;

        public GraphEditor(Graph graph) {
            this.InitializeComponent();
            this._gevm = new GraphEditorVM(graph, this.GraphEditorCanvas);
            this.DataContext = this._gevm;
        }

        private void Button_Click_AddNode(object sender, RoutedEventArgs e) {
            try {
                this._gevm.ButtonAddNode(this.UserInputTextBlock.Text);
                this.UserInputTextBlock.Text = "";
            } catch (GraphException ge) {
                // Error Message
                /*MessageBoxResult messageBox =*/ MessageBox.Show(ge.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UserInputTextBlock_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                this.Button_Click_AddNode(null, null);
            }
        }
    }
}
