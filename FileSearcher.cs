using System;
using System.IO;

namespace FileSearchingWPF {
    // FileSearcher class encapsulate logics to find files 
    class FileSearcher {
        private MainWindow mainWindow;
        public Int32 NumFiles { set; get; } // Number of files processed
        public String Directory { get; set; }
        public String FilePattern { get; set; }

        public void StartSearching() {
            DirectoryInfo dir = new DirectoryInfo(Directory);
            FindFiles(dir);
        }

        public void SetWindowToShowResults(MainWindow mainWindow) {
            this.mainWindow = mainWindow;
        }

        private void FindFiles(DirectoryInfo dir) {
            ProcessDirectories(dir);
        }

        private void ProcessDirectories(DirectoryInfo dir) {
            try {
                DirectoryInfo[] subdirs = dir.GetDirectories();
                FileInfo[] files = dir.GetFiles();
                foreach (var subdir in subdirs) {
                    ProcessDirectories(subdir);
                }
                foreach (var file in files) {
                    NumFiles++;
                    mainWindow.qtyFilesLabel.Content = NumFiles;
                    if (file.Name.Contains(FilePattern)) {
                    }
                }
            } catch (Exception) {
            }
        }
    }
}
