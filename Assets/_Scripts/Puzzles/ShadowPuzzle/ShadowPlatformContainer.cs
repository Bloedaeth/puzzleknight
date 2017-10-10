using GameLogging;
using UnityEngine;

public class ShadowPlatformContainer : MonoBehaviour
{
    private ShadowModifier[] platforms;

    public GameObject master;
    public GameObject evilMaster;

    private void Start()
    {
        platforms = GetComponentsInChildren<ShadowModifier>(false);
        ResetAllSystems();
    }

    private void ResetAllSystems()
    {
        BuildDebug.Log("Resetting Shadow Platforms");
        if(ParticlesExist())
        {
            BroadcastMessage("KillAllShadowParticles");
        }

        for(int i = 0; i < platforms.Length; i++)
        {
            if(platforms[i].tag == "FalsePlatform")
            {
                BuildDebug.Log("Instantiating false platform.");
                Instantiate(evilMaster, platforms[i].transform);
            }
            else
            {
                BuildDebug.Log("Instantiating platform.");
                Instantiate(master, platforms[i].transform);
            }

            platforms[i].ResetParticleSystem();
        }
    }

    private bool ParticlesExist()
    {
        for(int i = 0; i < platforms.Length; i++)
        {
            if(platforms[i].HasParticleSystem())
            {
                return true;
            }
        }

        return false;
    }
}
