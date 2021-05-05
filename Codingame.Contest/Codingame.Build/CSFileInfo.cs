using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Codingame.Build
{
    public class CSFileInfo : FileInfo
    {
        List<string> usings;
        public List<string> Usings
        {
            get { return usings; }
            set { usings = value; }
        }

        string _namespace;
        public string Namespace
        {
            get { return _namespace; }
            set { _namespace = value; }
        }

        string classTextInNamespace;
        public string ClassTextInNamespace
        {
            get { return classTextInNamespace; }
            set { classTextInNamespace = value; }
        }

        bool hasMainMethod;
        public bool HasMainMethod
        {
            get { return hasMainMethod; }
            set { hasMainMethod = value; }
        }

        public CSFileInfo(string fullPathName) : base(fullPathName)
        {
            string textFromFile = System.IO.File.ReadAllText(@fullPathName);

            ParseUsings(textFromFile);
            ParseNamespace(textFromFile);
            ParseAllClassesInNamespace(textFromFile);
            FindMainMethodAndSetProperty(textFromFile);
        }

        private void FindMainMethodAndSetProperty(string textFromFile)
        {
            string pattern = @"Main\(string\s*\[\s*\]\s+\w+\)\s*\{";
            Match match = Regex.Match(textFromFile, pattern);

            hasMainMethod = match.Equals(Match.Empty) ? false : true;
        }

        private void ParseAllClassesInNamespace(string textFromFile)
        {
            string pattern = @"namespace\s+[\w\.]+\s+\{(?<allClasses>[\w\W]+)\}";
            Match match = Regex.Match(textFromFile, pattern);

            classTextInNamespace = match.Groups["allClasses"].Value;
        }

        private void ParseNamespace(string textFromFile)
        {
            string pattern = @"namespace\s+(?<namespace>[\w\.]+)";
            Match match = Regex.Match(textFromFile, pattern);

            _namespace = match.Groups["namespace"].Value;
        }

        private void ParseUsings(string textFromFile)
        {
            usings = new List<string>();
            string pattern = @"using\s+(?<import>[\w\.]+);";

            foreach (Match match in Regex.Matches(textFromFile, pattern))
            {
                usings.Add(match.Groups["import"].Value);
            }
        }

        public new int CompareTo(object obj)
        {
            if (obj == null) return 1;

            CSFileInfo otherFileInfo = obj as CSFileInfo;

            if (otherFileInfo != null)
            {
                if (this.hasMainMethod && !otherFileInfo.hasMainMethod)
                {
                    return -1;
                }
                else if (!this.hasMainMethod && otherFileInfo.hasMainMethod)
                {
                    return 1;
                }
                else
                {
                    if (Name.Equals(otherFileInfo.Name))
                    {
                        return FullPathName.CompareTo(otherFileInfo.FullPathName);
                    }
                    else
                    {
                        return Name.CompareTo(otherFileInfo.Name);
                    }
                }
            }
            else
            {
                throw new ArgumentException("Object is not a CSFileInfo");
            }
        }

    }
}
