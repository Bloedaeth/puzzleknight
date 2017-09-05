using UnityEditor;
using System.Diagnostics;

public class AutoBuild
{
	static void BuildGame()
    {
        string[] scenes = { "Assets/_Scenes/Menus/Start Menu.unity", "Assets/_Scenes/Deliverable.unity", "Assets/_Scenes/Menus/Controls Menu.unity", "Assets/_Scenes/Menus/Options Menu.unity" };
        BuildPipeline.BuildPlayer(scenes, "Builds/Windows/PuzzleKnight.exe", BuildTarget.StandaloneWindows, BuildOptions.None);
    }

    static void PlayGame()
    {
        Process proc = new Process();
        proc.StartInfo.FileName = System.IO.Directory.GetCurrentDirectory() + "/Builds/Windows/PuzzleKnight.exe";
        proc.Start();
    }
}
