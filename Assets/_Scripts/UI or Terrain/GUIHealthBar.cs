using UnityEngine;

public class GUIHealthBar : MonoBehaviour
{
    private RectTransform rect;

    private const float FULL_HEALTH_TOP = 44f;
    private const float NO_HEALTH_TOP = 123f;

    private void Awake()
    {
        rect = GetComponentInChildren<RectTransform>();
    }

    public void UpdateGUI(int curHP, int maxHP)
    {
        float percent = (float)curHP / maxHP;
        float newRectTop = NO_HEALTH_TOP - ((NO_HEALTH_TOP - FULL_HEALTH_TOP) * percent);
        rect.offsetMax = new Vector2(rect.offsetMax.x, -newRectTop);
    }
}
