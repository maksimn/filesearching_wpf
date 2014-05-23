﻿using System;
using System.IO;
using System.Threading;

namespace FileSearchingWPF {
    // FileSearcher class encapsulate logics to find files 
    class FileSearcher {
        public Int32 NumFiles { set; get; } // Number of files processed
        public String Directory { get; set; }
        public String FilePattern { get; set; }

        public event EventHandler<NewFileProcessedEventArgs> NewFileProcessed;

        public void StartSearching() {
            DirectoryInfo dir = new DirectoryInfo(Directory);
            FindFiles(dir);
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
                    OnNewFileProcessed(new NewFileProcessedEventArgs(NumFiles));
                    if (file.Name.Contains(FilePattern)) {
                    }
                }
            } catch (Exception) {
            }
        }

        protected virtual void OnNewFileProcessed(NewFileProcessedEventArgs e) {
            EventHandler<NewFileProcessedEventArgs> temp = Volatile.Read(ref NewFileProcessed);
            if (temp != null) {
                temp(this, e);
            }
        }
    }
}