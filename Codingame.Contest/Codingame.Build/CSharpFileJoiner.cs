using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

namespace Codingame.Build
{
    public class CSharpFileJoiner
    {
        private bool includeSubdirectories;
        public bool IncludeSubdirectories
        {
            get { return includeSubdirectories; }
            set { includeSubdirectories = value; }
        }

        private bool includeNamespace;
        public bool IncludeNamespace
        {
            get { return includeNamespace; }
            set { includeNamespace = value; }
        }

        private string fileType;
        public string FileType { get => fileType; set => fileType = value; }

        private string destFile;
        public string DestFile { get => destFile; set => destFile = value; }

        string workingPath = String.Empty;
        public string WorkingPath {
            get { return workingPath; }
            set { workingPath = value; }
        }

        public string GetFullWorkingPath {
            get {
                return Path.GetFullPath(@WorkingPath);
            }
        }

        List<string> supportedFileExtensions = new List<string>();
        public List<string> SupportedFileExtensions
        {
            get
            {
                if (supportedFileExtensions == null)
                {
                    supportedFileExtensions = new List<string>();
                }
                if (supportedFileExtensions.Count <= 0)
                {
                    supportedFileExtensions.Add("*.cs");
                }

                return supportedFileExtensions;
            }
        }
        //private List<string> GetSupportedFiles()
        //{
        //    return new List<string>();
        //}

        Collection<string> usings = new Collection<string>();
        public Collection<string> Usings
        {
            get { return usings; }
        }

        Collection<CSFileInfo> fileList = new Collection<CSFileInfo>();
        public Collection<CSFileInfo> FileList
        {
            get {
                if (fileList is null)
                {
                    fileList = new Collection<CSFileInfo>();
                }
                return fileList; 
            }
            set { fileList = value; }
        }

        string joinedSourceCode = String.Empty;
        public string JoinedSourceCode
        {
            get { return joinedSourceCode; }
            set { joinedSourceCode = value; }
        }

        public CSharpFileJoiner(string workingPath, string destFile = "output.cs", string filetype="*.cs", bool includeSubdirectories = true, bool includeNamespace = false)
        {
            this.FileType = filetype;
            this.DestFile = destFile;
            this.WorkingPath = workingPath;
            this.IncludeSubdirectories = includeSubdirectories;
            this.IncludeNamespace = includeNamespace;

            // refresh!
            UpdateSourceCodeFileListToProcess(this.WorkingPath);
        }

        // slow but text control.
        public void MultipleFilesToSingleFile(string dirPath, string filePattern, string destFile)
        {
            string[] fileAry = Directory.GetFiles(dirPath, filePattern);

            Console.WriteLine("Total File Count : " + fileAry.Length);

            using (TextWriter tw = new StreamWriter(destFile, true))
            {
                foreach (string filePath in fileAry)
                {
                    using (TextReader tr = new StreamReader(filePath))
                    {
                        tw.WriteLine(tr.ReadToEnd());
                        tr.Close();
                        tr.Dispose();
                    }
                    Console.WriteLine("File Processed : " + filePath);
                }

                tw.Close();
                tw.Dispose();
            }
        }

        private void UpdateSourceCodeFileListToProcess(string directory)
        {
            var searchSubdirectories = SearchOption.AllDirectories;
            if (!this.IncludeSubdirectories)
            {
                searchSubdirectories = SearchOption.TopDirectoryOnly;
            }

            string[] matchingFiles = Directory.GetFiles(this.WorkingPath, this.FileType, searchSubdirectories);
            Console.WriteLine("Total File Count : " + matchingFiles.Length);
            this.FileList.Clear();
            foreach (string f in matchingFiles)
            {
                // this.FileList.Add(new CSFileInfo(f));
                // Add file info and update usings list
                AddFileInfoIfNotFoundInList(new CSFileInfo(f));
            }
        }

        // fast but binary
        public void CombineMultipleFilesIntoSingleFile(string inputDirectoryPath, string inputFileNamePattern, string outputFilePath)
        {
            string[] inputFilePaths = Directory.GetFiles(inputDirectoryPath, inputFileNamePattern);
            Console.WriteLine("Number of files: {0}.", inputFilePaths.Length);

            using (var outputStream = File.Create(outputFilePath))
            {
                foreach (var inputFilePath in inputFilePaths)
                {
                    using (var inputStream = File.OpenRead(inputFilePath))
                    {
                        // Buffer size can be passed as the second argument.
                        inputStream.CopyTo(outputStream);
                    }
                    Console.WriteLine("The file {0} has been processed.", inputFilePath);
                }
            }
        }

        public void JoinSourceCodeFiles()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(Environment.NewLine);
            // Adding the merged usings.
            foreach (string import in usings)
            {
                stringBuilder.Append("using " + import + ";" + Environment.NewLine);
            }

            if (this.IncludeNamespace)
            {
                stringBuilder.Append(Environment.NewLine + "namespace " + fileList[0].Namespace +
                    Environment.NewLine + "{" + Environment.NewLine);
            }

            foreach (CSFileInfo file in fileList)
            {
                int lineLength = 80;

                string commentLine = new String('/', lineLength) + Environment.NewLine;
                string fileNamePrefix = "//  Code from: " + file.Name;

                string fileNameSuffix = new String(' ', lineLength - fileNamePrefix.Length - 2) +
                    "//" + Environment.NewLine;

                stringBuilder.Append(Environment.NewLine + commentLine + fileNamePrefix +
                    fileNameSuffix + commentLine);

                stringBuilder.Append(file.ClassTextInNamespace);
            }

            if (this.IncludeNamespace)
            {
                stringBuilder.Append("}" + Environment.NewLine);
            }

            JoinedSourceCode = stringBuilder.ToString();
        }

        public void SaveJoinedSourceCode()
        {
            using (TextWriter tw = new StreamWriter(this.destFile, false))
            {
                tw.Write(this.JoinedSourceCode);

                tw.Close();
                tw.Dispose();
            }
        }

        private void AddFileInfoIfNotFoundInList(CSFileInfo fileInfo)
        {
            if (!fileList.Any(file => file.FullPathName.Equals(fileInfo.FullPathName)))
            {
                fileList.Add(fileInfo);
                AddUsingsFromFileInfo(fileInfo);
            }
        }

        private void AddUsingsFromFileInfo(CSFileInfo fileInfo)
        {
            List<string> updatedUsings = new List<string>(fileInfo.Usings.Union(usings));
            updatedUsings.Sort();

            usings = new Collection<string>(updatedUsings);
        }
    }
}
