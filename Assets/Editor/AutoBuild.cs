using UnityEditor;
using System.Diagnostics;

public class AutoBuild
{
	static void BuildGame()
    {
        string[] scenes = { "Assets/_Scenes/Testing Maze.unity" };
        BuildPipeline.BuildPlayer(scenes, "Builds/Win64/PuzzleKnight.exe", BuildTarget.StandaloneWindows64, BuildOptions.None);
    }

    static void PlayGame()
    {
        Process proc = new Process();
        proc.StartInfo.FileName = System.IO.Directory.GetCurrentDirectory() +"/Builds/Win64/PuzzleKnight.exe";
        proc.Start();
    }
}
