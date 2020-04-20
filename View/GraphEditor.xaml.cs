using GraphTheory.Core;
using GraphTheoryInWPF.Components;
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
        public readonly GraphEditorVM GEVM;

        public GraphEditor(Graph graph) {
            this.InitializeComponent();
            this.GEVM = new GraphEditorVM(graph, this.GraphEditorCanvas, this);
            this.DataContext = this.GEVM;
        }

        private void Button_Click_AddNode(object sender, RoutedEventArgs e) {
            try {
                if (this.UserInputTextBlock.Text == "")
                    throw new GraphException("THE NODE NEEDS TO HAVE A VALID NAME!");
                this.GEVM.ButtonAddNode(this.UserInputTextBlock.Text);
                this.UserInputTextBlock.Text = "";
            } catch (GraphException ge) {
                // Error Message
                /*MessageBoxResult messageBox =*/
                MessageBox.Show(ge.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UserInputTextBlock_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                this.Button_Click_AddNode(null, null);
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e) {
            this.GEVM.MenuItemAddNode((int) p.X, (int) p.Y);
        }

        private Point p; // temporary point to get the position used when creating a new node via the canvas - ugly but it works

        private void GraphEditorCanvas_ContextMenuOpening(object sender, ContextMenuEventArgs e) {
            this.p = Mouse.GetPosition(this.GraphEditorCanvas);
        }
    }
}
