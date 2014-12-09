using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FilesExtractor.Code;
using FilesExtractor.Code.Extensions;
using FilesExtractor.Code.Filters;

namespace FilesExtractor {
    class Program {
        public const string SOURCE_DIRECTORY_KEY = "dir";
        public const int MAX_TASK_POOL_SIZE = 30;

        static void Main(string[] args) {
            Dictionary<string, string> argumentsKeyValues = new CmdLineArgumentsParser().Parse(args);

            string sourceDirectory;
            if(!argumentsKeyValues.TryGetValue(SOURCE_DIRECTORY_KEY, out sourceDirectory) || !Directory.Exists(sourceDirectory)) {
                Console.WriteLine(" \n      Please specify the directory where zipped files are located. Use '{0}' parameter. For example \"{0}:D:/myDirWithZippedFolders\" ",
                    SOURCE_DIRECTORY_KEY);
                Console.ReadKey();
                return;
            }

            var fileExtractor = new FileExtractor(FiltersFactory.CreateFiltersByArguments(argumentsKeyValues));

            try {
                string[] zippedFilesPaths = Directory.GetFiles(sourceDirectory, string.Format("{0}", FileExtractor.ZIP_FILES_EXTENSION));
                Queue<Task> pendingTaskQueue = CreateTaskQueue(zippedFilesPaths, fileExtractor);
                var taskPool = new Task[Math.Min(pendingTaskQueue.Count, MAX_TASK_POOL_SIZE)];
                TaskHelper.ProcessTaskPoolToCompletion(taskPool, pendingTaskQueue); // Note: Processes tasks by batches of size MAX_TASK_POOL_SIZE each
            } catch(AggregateException ae) {
                ae.Flatten().InnerExceptions.ForEach(e => Console.WriteLine(e.Message));
            }

            Console.WriteLine("Task finished");
            Console.ReadKey();
        }

        private static Queue<Task> CreateTaskQueue(string[] filesPaths, FileExtractor fileExtractor) {
            var pendingTaskQueue = new Queue<Task>();

            filesPaths.ForEach(filePath => pendingTaskQueue.Enqueue(new Task(() => {
                string destination = filePath.Substring(0,
                    filePath.LastIndexOf(FileExtractor.ZIP_FILES_EXTENSION, StringComparison.OrdinalIgnoreCase));
                fileExtractor.Extract(filePath, destination);
            }, TaskCreationOptions.LongRunning)));
            
            return pendingTaskQueue;
        }
    }
}