using System;
using System.Windows;
using Shackal.Gui.ViewModel;

namespace Shackal.Gui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
            DataContext = new MainViewModel();
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var oldContext = dependencyPropertyChangedEventArgs.OldValue as IDisposable;
            oldContext?.Dispose();

            var newContext = dependencyPropertyChangedEventArgs.NewValue as MainViewModel;
            newContext?.InitializeView(this);
        }
    }
}
