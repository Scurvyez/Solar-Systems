using System.Collections;
using Saving;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StarSelectionMenu : MonoBehaviour
{
    public Button TargetButton;
    public float PulseSpeed = 1.0f;
    public float PulseAmount = 0.1f;
    public float PulseDuration = 2.0f;
    private Vector3 _originalScale;
    private bool _pulsing = false;

    private void Start()
    {
        _originalScale = TargetButton.transform.localScale;
    }

    public void Back()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void Forward()
    {
        string dataPath = Application.persistentDataPath;
        string saveFilePath = dataPath + "/" + SaveManager.Instance.ActiveSave + ".xml";

        if (System.IO.File.Exists(saveFilePath))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            StartPulsing();
        }
    }

    private void StartPulsing()
    {
        _pulsing = true;
        StartCoroutine(Pulse());
    }

    private IEnumerator Pulse()
    {
        float t = 0;
        float pulseStartTime = Time.time;

        while (_pulsing)
        {
            if (Time.time - pulseStartTime >= PulseDuration)
            {
                _pulsing = false;
                TargetButton.transform.localScale = _originalScale;
                break;
            }

            t += Time.deltaTime * PulseSpeed;
            float scale = Mathf.Sin(t) * PulseAmount + 1;
            TargetButton.transform.localScale = _originalScale * scale;

            yield return null;
        }
    }
}
