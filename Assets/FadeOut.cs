using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    private Image img;
    private const float FADE_STEP = 0.005f;

    private void Awake()
    {
        img = GetComponent<Image>();
    }

    public IEnumerator Fade()
    {
        while(img.color.a < 1)
        {
            img.color = new Color(0, 0, 0, img.color.a + FADE_STEP);
            yield return new WaitForEndOfFrame();
        }

        FindObjectOfType<LevelManager>().LoadNextLevelAsync();
        gameObject.SetActive(false);
        yield return null;
    }
}
