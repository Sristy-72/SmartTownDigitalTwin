
using UnityEngine;

public class FloodSimulation : MonoBehaviour
{
    public GameObject water;
    public float riseSpeed = 0.5f;
    public float maxWaterHeight = 5f;

    private float startHeight;
    private bool isFlooding = false;

    void Start()
    {
        if (water == null)
        {
            Debug.LogError("FloodWater is not assigned!");
            return;
        }

        startHeight = water.transform.position.y;
        water.SetActive(false);

        Debug.Log("FloodSimulation is ready. Press F, T, or R.");
    }

    void Update()
    {
        // Keyboard controls
        if (Input.GetKeyDown(KeyCode.F))
            StartFlood();

        if (Input.GetKeyDown(KeyCode.T))
            StopFlood();

        if (Input.GetKeyDown(KeyCode.R))
            ResetFlood();

        // Move the water only when flooding
        if (water == null || !isFlooding)
            return;

        Vector3 position = water.transform.position;

        if (position.y < maxWaterHeight)
        {
            position.y += riseSpeed * Time.deltaTime;
            position.y = Mathf.Min(position.y, maxWaterHeight);
            water.transform.position = position;
        }
        else
        {
            isFlooding = false;
            Debug.Log("Maximum water level reached.");
        }
    }

    public void StartFlood()
    {
        if (water == null) return;

        water.SetActive(true);
        isFlooding = true;
        Debug.Log("Flood started.");
    }

    public void StopFlood()
    {
        isFlooding = false;
        Debug.Log("Flood stopped.");
    }

    public void ResetFlood()
    {
        if (water == null) return;

        isFlooding = false;

        Vector3 position = water.transform.position;
        position.y = startHeight;
        water.transform.position = position;

        water.SetActive(false);
        Debug.Log("Flood reset.");
    }

    public void SetWaterLevel(float level)
    {
        maxWaterHeight = level;
    }
}
