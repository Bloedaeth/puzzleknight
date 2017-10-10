using GameLogging;
using UnityEngine;

public class ShadowPuzzle : Puzzle
{
    private bool logged = false;

    private void OnTriggerEnter(Collider o)
    {
        if(o.transform.tag == "Player")
        {
            if(!logged)
            {
                logged = true;
                BuildDebug.Log("Shadow Puzzle door opened");
            }
            CheckFinalizePuzzle();
        }
    }

    public override void CheckFinalizePuzzle()
    {
        solved = true;
    }
}