using UnityEngine;
using UnityEngine.UI;

public class Soakable : MonoBehaviour
{
    public float currentSoak = 0f;
    public float maxSoak = 100f;
    public Slider soakSlider;
    public float baseMoveSpeed = 6f;
    public float currentMoveSpeed;

    void Start()
    {
        currentMoveSpeed = baseMoveSpeed;
        UpdateUI();
    }

    public void ApplySoak(float amount)
    {
        currentSoak = Mathf.Clamp(currentSoak + amount, 0f, maxSoak);
        UpdateUI();
        OnSoakedEffects();
    }

    void UpdateUI()
    {
        if (soakSlider != null) soakSlider.value = currentSoak / maxSoak;
    }

    void OnSoakedEffects()
    {
        float factor = 1f - (currentSoak / maxSoak) * 0.5f;
        currentMoveSpeed = baseMoveSpeed * factor;
    }
}
