using System;
using System.Windows;
using System.Windows.Controls;

namespace FileSearchingWPF {
    public partial class MainWindow : Window {
        // Fields:
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private FileSearcher fileSearcher;

        // Methods:
        public MainWindow() {
            InitializeComponent();
            SetInitialValuesInTextBoxes();

            fileSearcher = new FileSearcher();
            fileSearcher.NewFileProcessed += NewFileProcessedMsg;
            fileSearcher.NewFileFound += NewFileFoundMsg;
        }

        private void MainWindowSizeChangedHandler(object sender, SizeChangedEventArgs e) {
            if (e.NewSize.Width > 250) {
                treeView.Width = e.NewSize.Width - 250 - 20;
                treeView.Height = e.NewSize.Height - 53;
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
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.folderBrowserDialog.Description = "Задайте директорию для поиска файлов.";
            this.folderBrowserDialog.SelectedPath = folderTextBox.Text;
            System.Windows.Forms.DialogResult dialogResult = this.folderBrowserDialog.ShowDialog();
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
            fileSearcher.NumFiles = 0;
        }

        private void NewFileProcessedMsg(Object o, NewFileProcessedEventArgs e) {
            this.Dispatcher.Invoke(new Action<Int32, String>(
                (num, str) => { qtyFilesLabel.Content = num.ToString(); timeLabel.Content = str; }
                ), fileSearcher.NumFiles, fileSearcher.Time.ToString().Substring(0, 11));
        }

        private void NewFileFoundMsg(Object o, NewFileFoundEventArgs e) {
            this.Dispatcher.Invoke(new Action<String>(str => AddInformationInTreeView(str)), e.FullName);
        }

        private void AddInformationInTreeView(String str) {
            if (treeView.Items.IsEmpty) {
                WhatToDoWhenThereAreNoFoundFilesInTreeView(str);
            } else {
                WhatToDoIfAtLeastOneFoundFileExistsInTreeView(str);
            }            
        }

        private void WhatToDoWhenThereAreNoFoundFilesInTreeView(String str) {
            String[] sArr = str.Split(new Char[] { '\\' });
            TreeViewItem currItem = new TreeViewItem();
            if (sArr.Length > 0) {
                currItem.Header = sArr[0];
                treeView.Items.Add(currItem);
                currItem.IsExpanded = true;
            }
            for (Int32 i = 1; i < sArr.Length; i++) {
                TreeViewItem newTreeViewItem = new TreeViewItem();
                newTreeViewItem.Header = sArr[i];
                currItem.Items.Add(newTreeViewItem);
                currItem = newTreeViewItem;
                currItem.IsExpanded = true;
            }
        }

        private void WhatToDoIfAtLeastOneFoundFileExistsInTreeView(String str) {
            String[] sArr = str.Split(new Char[] {'\\'});
            TreeViewItem currItem = (TreeViewItem)treeView.Items.GetItemAt(0);
            for (Int32 i = 1; i < sArr.Length; i++) {
                Boolean isThisItemExist = false;
                foreach (TreeViewItem item in currItem.Items) {
                    if ((String)item.Header == sArr[i]) {
                        isThisItemExist = true;
                        currItem = item;
                        break;
                    }
                }
                if (isThisItemExist) {
                    continue;
                }
                TreeViewItem newItem = new TreeViewItem();
                newItem.Header = sArr[i];
                currItem.Items.Add(newItem);
                currItem = newItem;
                currItem.IsExpanded = true;
            }
        }

        private void stopButtonClickHandler(object sender, RoutedEventArgs e) {
            fileSearcher.Stop();
        }
    }
}
