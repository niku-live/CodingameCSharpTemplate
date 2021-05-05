using Microsoft.Build.Framework;
using MSBuildTask = Microsoft.Build.Utilities.Task;

namespace Codingame.Build
{
    public class JoinSourceFilesTask : MSBuildTask
    {
        public string SourceFolder { get; set; }
        public string DestFile { get; set; }

        public override bool Execute()
        {
            Log.LogMessage(MessageImportance.High, $"Aloha; source: {SourceFolder}; dest: {DestFile}");

            CSharpFileJoiner joiner = new CSharpFileJoiner(SourceFolder);
            joiner.JoinSourceCodeFiles();
            System.IO.File.WriteAllText(DestFile, joiner.JoinedSourceCode);
            return true;
        }
    }
}
