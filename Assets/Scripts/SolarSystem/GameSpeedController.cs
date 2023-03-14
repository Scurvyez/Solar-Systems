using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSpeedController : MonoBehaviour
{
    public Button[] buttons;

    public void SetGameSpeed(Button button)
    {
        float speed = button.name switch
        {
            "Pause" => 0.0f,
            "Slowww" => 0.005f,
            "Quarter" => 0.25f,
            "Half" => 0.5f,
            "Normal" => 1f,
            "Fast" => 1.5f,
            "Double" => 2f,
            _ => throw new System.ArgumentException($"Unknown speed name '{button.name}'")
        };
        Time.timeScale = speed;
    }

}
