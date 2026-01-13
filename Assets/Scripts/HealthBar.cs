using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthBarSlider;
    [SerializeField] private Health enemyHealth; 

    void Update()
    {
        if (enemyHealth != null && healthBarSlider != null)
        {
            // 获取当前血量百分比并设置到 Slider 的 Value
            float normalizedHealth = enemyHealth.GetHealthAmountNormalized();
            healthBarSlider.value = normalizedHealth * healthBarSlider.maxValue;
        }
    }
}
