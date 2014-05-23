using System;

namespace FileSearchingWPF {
    class NewFileProcessedEventArgs : EventArgs {
        private readonly Int32 num;

        public NewFileProcessedEventArgs(Int32 n) { num = n; }

        public Int32 NumFiles { get { return num; } }
    }
}
