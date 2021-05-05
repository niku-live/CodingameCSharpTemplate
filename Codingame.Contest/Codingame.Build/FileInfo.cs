using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Codingame.Build
{
    public class FileInfo : IComparable
    {
        string path;
        public string Path
        {
            get { return path; }
            set { path = value; }
        }

        string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        string fullPathName;
        public string FullPathName
        {
            get { return fullPathName; }
            set { fullPathName = value; }
        }

        public FileInfo(string fullPathName)
        {
            string[] pathComponents = fullPathName.Split('\\');
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < pathComponents.Length - 1; i++)
            {
                if (i > 0)
                {
                    sb.Append('\\');
                }
                sb.Append(pathComponents[i]);
            }

            path = sb.ToString();
            name = pathComponents[pathComponents.Length - 1];
            this.fullPathName = fullPathName;
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            FileInfo otherFileInfo = obj as FileInfo;

            if (otherFileInfo != null)
            {
                if (name.Equals(otherFileInfo.Name))
                {
                    return FullPathName.CompareTo(otherFileInfo.FullPathName);
                }
                else
                {
                    return name.CompareTo(otherFileInfo.Name);
                }
            }
            else
            {
                throw new ArgumentException("Object is not a FileInfo");
            }
        }
    }
}
