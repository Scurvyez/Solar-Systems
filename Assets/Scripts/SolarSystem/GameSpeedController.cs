using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class GameSpeedController : MonoBehaviour
{
    public float TransitionDuration = 3.0f;
    public float CurSpeed;
    public float Speed1 = 0.01f;
    public float Speed2 = 0.25f;
    public float Speed3 = 0.5f;
    public float Speed4 = 1f;
    public float Speed5 = 50f;
    public float Speed6 = 100f;
    public float Speed7 = 200f;
    
    private static GameSpeedController _instance;
    
    public static GameSpeedController Instance
    {
        get
        {
            if (_instance is not null) return _instance;
            _instance = FindObjectOfType<GameSpeedController>();

            if (_instance is not null) return _instance;
            GameObject singletonObject = new ("GameSpeedController");
            _instance = singletonObject.AddComponent<GameSpeedController>();

            return _instance;
        }
    }

    private void Awake()
    {
        // Set curSpeed to 1 when the script instance is being loaded
        CurSpeed = Speed4;
    }

    public void SetGameSpeed(Button button)
    {
        float targetSpeed = CalculateGameSpeed(button);
        StopAllCoroutines(); // Stop any existing transition
        StartCoroutine(SmoothTransition(targetSpeed));
    }

    private float CalculateGameSpeed(Button button)
    {
        float speed = button.name switch
        {
            "Slowww" => Speed1,
            "Quarter" => Speed2,
            "Half" => Speed3,
            "Normal" => Speed4,
            "Fast" => Speed5,
            "Double" => Speed6,
            "Warp Speed" => Speed7,
            _ => throw new System.ArgumentException($"Unknown speed name '{button.name}'")
        };
        return speed;
    }
    
    private IEnumerator SmoothTransition(float targetSpeed)
    {
        float startSpeed = CurSpeed;
        float elapsedTime = 0f;

        while (elapsedTime < TransitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / TransitionDuration;
            CurSpeed = Mathf.Lerp(startSpeed, targetSpeed, EasingFunctionsUtil.EaseInOutCubic(t));
            yield return null;
        }
        CurSpeed = targetSpeed; // Ensure the exact target speed is set at the end
    }
}
