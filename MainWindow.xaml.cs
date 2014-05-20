using System;
using System.Windows;

namespace FileSearchingWPF {
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        private void MainWindowSizeChangedHandler(object sender, SizeChangedEventArgs e) {
            if (e.NewSize.Width > 250) {
                treeView.Width = e.NewSize.Width - 250;
                treeView.Height = e.NewSize.Height;
            }
        }
    }
}
