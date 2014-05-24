using System;
using System.Windows;
using System.Windows.Forms;

namespace FileSearchingWPF {
    public partial class MainWindow : Window {
        // Fields:
        private FolderBrowserDialog folderBrowserDialog;
        private FileSearcher fileSearcher;

        // Methods:
        public MainWindow() {
            InitializeComponent();
            SetInitialValuesInTextBoxes();

            fileSearcher = new FileSearcher();
            fileSearcher.NewFileProcessed += NewFileProcessedMsg;
        }

        private void MainWindowSizeChangedHandler(object sender, SizeChangedEventArgs e) {
            if (e.NewSize.Width > 250) {
                treeView.Width = e.NewSize.Width - 250;
                treeView.Height = e.NewSize.Height;
            }
        }

        private void MainWindowClosedHandler(object sender, EventArgs e) {
            SaveSettingsOfTextBoxes();
        }

        private void SetInitialValuesInTextBoxes() {
            folderTextBox.Text = Properties.Settings.Default.Folder;
            fileTextBox.Text = Properties.Settings.Default.FileName;
        }

        private void SaveSettingsOfTextBoxes() {
            Properties.Settings.Default.Folder = folderTextBox.Text;
            Properties.Settings.Default.FileName = fileTextBox.Text;
            Properties.Settings.Default.Save();
        }

        private void WorkingWithFolderBrowserDialog() {
            this.folderBrowserDialog = new FolderBrowserDialog();
            this.folderBrowserDialog.Description = "Задайте директорию для поиска файлов.";
            this.folderBrowserDialog.SelectedPath = folderTextBox.Text;
            DialogResult dialogResult = this.folderBrowserDialog.ShowDialog();
            if (dialogResult == System.Windows.Forms.DialogResult.OK) {
                folderTextBox.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void folderButtonClickHandler(object sender, RoutedEventArgs e) {
            WorkingWithFolderBrowserDialog();
        }

        private async void startButtonClickHandler(object sender, RoutedEventArgs e) {
            EmptyTreeView();
            SetSearchingParameters();
            await fileSearcher.StartSearching();           
        }

        private void EmptyTreeView() {
            treeView.Items.Clear();
        }

        private void SetSearchingParameters() {
            fileSearcher.Directory = folderTextBox.Text;
            fileSearcher.FilePattern = fileTextBox.Text;            
        }

        private void NewFileProcessedMsg(Object o, NewFileProcessedEventArgs e) {
            this.Dispatcher.Invoke(new Action<int>(num => qtyFilesLabel.Content = num.ToString()), e.NumFiles);
        }
    }
}
