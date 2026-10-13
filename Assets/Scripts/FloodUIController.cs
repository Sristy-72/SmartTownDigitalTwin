
using UnityEngine;
using UnityEngine.UI;

public class FloodUIController : MonoBehaviour
{
    public Slider waterLevelSlider;
    public FloodSimulation floodSimulation;

    void Start()
    {
        if (waterLevelSlider == null || floodSimulation == null)
        {
            Debug.LogError("Assign the slider and FloodSimulation.");
            return;
        }

        waterLevelSlider.minValue = 1f;
        waterLevelSlider.maxValue = 5f;
        waterLevelSlider.wholeNumbers = true;
        waterLevelSlider.value = floodSimulation.maxWaterHeight;

        waterLevelSlider.onValueChanged.AddListener(
            floodSimulation.SetWaterLevel
        );
    }

    void OnDestroy()
    {
        if (waterLevelSlider != null && floodSimulation != null)
        {
            waterLevelSlider.onValueChanged.RemoveListener(
                floodSimulation.SetWaterLevel
            );
        }
    }
}
