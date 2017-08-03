using UnityEditor;
using System.Diagnostics;

public class AutoBuild
{
	static void BuildGame()
    {
        string[] scenes = { "Assets/_Scenes/Testing Maze.unity" };
        BuildPipeline.BuildPlayer(scenes, "Builds/Windows/PuzzleKnight.exe", BuildTarget.StandaloneWindows, BuildOptions.None);
    }

    static void PlayGame()
    {
        Process proc = new Process();
        proc.StartInfo.FileName = System.IO.Directory.GetCurrentDirectory() + "/Builds/Windows/PuzzleKnight.exe";
        proc.Start();
    }
}
