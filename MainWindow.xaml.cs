using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
using GraphTheory.Core;
using GraphTheoryInWPF.View;
using Microsoft.Win32;

namespace GraphTheoryInWPF {
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow: Window, INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName] string name = "")
               => this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(name));

        private UserControl _shownView;
        private Graph _currentGraph;
        private string _currentPath;

        public UserControl ShownView {
            get { return this._shownView; }
            set {
                this._shownView = value;
                this.RaisePropertyChanged();
            }
        }

        public MainWindow() {
            #region
            /*
            #region Stadt
            Graph Stadt = new Graph();
            #region const
            const string JAN          = "Chez Jan",
                         MANFRED      = "Chez Manfred",
                         JONAS        = "Chez Jonas",
                         DAMIEN       = "Chez Damien",
                         THOMAS       = "Chez Thomas",
                         MARKTPLATZ   = "Marktplatz",
                         RATHAUS      = "Rathaus",
                         LADEN        = "Laden",
                         KIOSK        = "Kiosk",
                         POSTAMT      = "Postamt",
                         BAUERNHOF    = "Bauernhof";
            #endregion
            #region Creating the nodes
            Stadt.AddNewNodeToGraph(JAN);
            Stadt.AddNewNodeToGraph(JONAS);
            Stadt.AddNewNodeToGraph(DAMIEN);
            Stadt.AddNewNodeToGraph(THOMAS);
            Stadt.AddNewNodeToGraph(MANFRED);
            Stadt.AddNewNodeToGraph(MARKTPLATZ);
            Stadt.AddNewNodeToGraph(RATHAUS);
            Stadt.AddNewNodeToGraph(LADEN);
            Stadt.AddNewNodeToGraph(KIOSK);
            Stadt.AddNewNodeToGraph(POSTAMT);
            Stadt.AddNewNodeToGraph(BAUERNHOF);
            #endregion
            #region Creating the connections
            Stadt.AddTwoWayNodeConnetionToGraph(JAN, POSTAMT);
            Stadt.AddTwoWayNodeConnetionToGraph(JAN, KIOSK);
            Stadt.AddTwoWayNodeConnetionToGraph(JAN, MANFRED);
            Stadt.AddTwoWayNodeConnetionToGraph(MANFRED, RATHAUS);
            Stadt.AddTwoWayNodeConnetionToGraph(RATHAUS, JONAS);
            Stadt.AddTwoWayNodeConnetionToGraph(RATHAUS, LADEN);
            Stadt.AddTwoWayNodeConnetionToGraph(RATHAUS, MARKTPLATZ);
            Stadt.AddTwoWayNodeConnetionToGraph(JONAS, THOMAS);
            Stadt.AddTwoWayNodeConnetionToGraph(THOMAS, DAMIEN);
            Stadt.AddTwoWayNodeConnetionToGraph(DAMIEN, LADEN);
            Stadt.AddTwoWayNodeConnetionToGraph(DAMIEN, BAUERNHOF);
            Stadt.AddTwoWayNodeConnetionToGraph(BAUERNHOF, MARKTPLATZ);
            Stadt.AddTwoWayNodeConnetionToGraph(MARKTPLATZ, POSTAMT);
            Stadt.AddTwoWayNodeConnetionToGraph(MARKTPLATZ, LADEN);
            #endregion
            #endregion
            #region testGraph
            Graph ABCGraph = new Graph();
            #region Creating Nodes
            ABCGraph.AddNewNodeToGraph("A");
            ABCGraph.AddNewNodeToGraph("B");
            ABCGraph.AddNewNodeToGraph("C");
            ABCGraph.AddNewNodeToGraph("D");
            ABCGraph.AddNewNodeToGraph("E");
            ABCGraph.AddNewNodeToGraph("F");
            #endregion
            #region Creating COnnections
            ABCGraph.AddTwoWayNodeConnetionToGraph("A", "B");
            ABCGraph.AddTwoWayNodeConnetionToGraph("A", "C");
            ABCGraph.AddTwoWayNodeConnetionToGraph("B", "F");
            ABCGraph.AddTwoWayNodeConnetionToGraph("B", "D");
            ABCGraph.AddTwoWayNodeConnetionToGraph("B", "C");
            ABCGraph.AddTwoWayNodeConnetionToGraph("D", "C");
            ABCGraph.AddTwoWayNodeConnetionToGraph("E", "C");
            ABCGraph.AddTwoWayNodeConnetionToGraph("E", "D");
            #endregion
            #endregion

            string path = @"D:\Programme\Visual Studio Projects\AOK\WPF\GraphTheoryWPF\JSONDB\meineStadt.txt";
            Graph.SaveGraphAsFile(Stadt, path);

            string path2 = @"D:\Programme\Visual Studio Projects\AOK\WPF\GraphTheoryWPF\JSONDB\ABCGraph.txt";
            Graph.SaveGraphAsFile(ABCGraph, path2);

            Graph g = Graph.LoadGraphFromFile(path2);

            this._currentGraph = g;

            this.InitializeComponent();
            this.DataContext = this;
            this._shownView = new RoutePlanner(this._currentGraph);
            */
            #endregion

            this.InitializeComponent();
            this.DataContext = this;

            this._currentGraph = new Graph();
            this._shownView = new GraphEditor(this._currentGraph);

        }

        protected override void OnClosing(CancelEventArgs e) {
            if (this.AskToSaveChangesAndOrContinue(MessageBoxButton.YesNo))
                base.OnClosing(e);
        }

        //protected override void OnClosed(EventArgs e) {
        //    if (this.AskToSaveChangesAndOrContinue()) {
        //        base.OnClosed(e);
        //    }
        //}

        private bool SavePrompt(MessageBoxButton messageBoxButton) {
            MessageBoxResult result = MessageBox.Show("Would you like to save any changes made to the graph?",
                                                      "GraphTheory", messageBoxButton, MessageBoxImage.Question);

            switch (result) {
                case MessageBoxResult.Cancel:
                    return false;
                case MessageBoxResult.Yes:
                    this.MenuItem_Click_Save(null, null);
                    return true;
                case MessageBoxResult.No:
                    return true;
                default:
                    return false;
            }
        }

        private bool AskToSaveChangesAndOrContinue(MessageBoxButton messageBoxButton = MessageBoxButton.YesNoCancel) {

            // If the graph has no real data in it
            if (this._currentGraph.GetAllNodeNames().ToList().Count == 0) {
                return true;
            }

            // If there is no Path selected
            if (this._currentPath == null)
                return this.SavePrompt(messageBoxButton);

            string a = Graph.SaveGraphAsString(this._currentGraph);
            string b = Graph.SaveGraphAsString(Graph.LoadGraphFromFile(this._currentPath));

            // Compare Graph Data
            if (a != b)
                return this.SavePrompt(messageBoxButton);

            // No need to save - thus the programm may simply move on
            return true;
        }

        private void MenuItem_Click_New(object sender, RoutedEventArgs e) {
            // New

            if (this.AskToSaveChangesAndOrContinue()) {
                this._currentPath = null;
                this._currentGraph = new Graph();
                this.ShownView = new GraphEditor(this._currentGraph);
                this.EditMenuItem.IsEnabled = false;
                this.RoutesMenuItem.IsEnabled = true;
            }
        }

        private void MenuItem_Click_Save(object sender, RoutedEventArgs e) {
            // Save
            if (this._currentPath == null) {
                this.MenuItem_Click_SaveAs(sender, e);
            } else {
                if (this._currentGraph != null) {
                    Graph.SaveGraphAsFile(this._currentGraph, this._currentPath);
                }
            }
        }

        private void MenuItem_Click_SaveAs(object sender, RoutedEventArgs e) {
            // Save As
            SaveFileDialog saveFileDialog = new SaveFileDialog {
                Filter = "Text Document (.txt)|*.txt",
                //InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) 
            };
            if (saveFileDialog.ShowDialog() == true) {
                Graph.SaveGraphAsFile(this._currentGraph, saveFileDialog.FileName);
                this._currentPath = saveFileDialog.FileName;
            }
        }

        private void MenuItem_Click_Open(object sender, RoutedEventArgs e) {
            // Open

            if (this.AskToSaveChangesAndOrContinue()) {
                OpenFileDialog openFileDialog = new OpenFileDialog {
                    DefaultExt = ".txt",
                    Filter     = "Text Document (.txt)|*.txt",
                };
                if (openFileDialog.ShowDialog() == true) {
                    string path = openFileDialog.FileName;
                    this._currentGraph = Graph.LoadGraphFromFile(path);
                    if (this.ShownView is RoutePlanner) {
                        this.ShownView = new RoutePlanner(this._currentGraph);
                    } else if (this.ShownView is GraphEditor) {
                        this.ShownView = new GraphEditor(this._currentGraph);
                    }
                    this._currentPath = openFileDialog.FileName;
                }
            }
        }

        private void MenuItem_Edit(object sender, RoutedEventArgs e) {
            // Edit
            this.ShownView = new GraphEditor(this._currentGraph);
            this.EditMenuItem.IsEnabled = false;
            this.RoutesMenuItem.IsEnabled = true;
        }

        private void MenuItem_Routes(object sender, RoutedEventArgs e) {
            this.ShownView = new RoutePlanner(this._currentGraph);
            this.RoutesMenuItem.IsEnabled = false;
            this.EditMenuItem.IsEnabled = true;
        }
    }
}
