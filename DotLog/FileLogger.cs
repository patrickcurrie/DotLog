using System.IO;

namespace DotLog
{
    class FileLogger : DotLogger
    {
        public string FilePath { get; private set; }

        public FileLogger(string filePath)
        {
            FilePath = filePath;
        }

        public override void Log(string message)
        {
            lock (threadLock)
            {
                using (StreamWriter streamWriter = new StreamWriter(FilePath, true))
                {
                    streamWriter.WriteLine(message);
                    streamWriter.Close();
                }
            }
        }
    }
    }
}
