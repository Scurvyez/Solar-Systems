using System.Collections;
using Saving;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

public class StarSelectionController : MonoBehaviour
{
    public Button SaveButton;
    public float JiggleAmount = 3f;
    public float JiggleDuration = 1f;
    
    private Vector3 _originalScale;
    private bool _jiggling = false;
    
    private void Start()
    {
        _originalScale = SaveButton.transform.localScale;
    }

    public void Back()
    {
        StartCoroutine(DelayedBack());
    }

    public void Forward()
    {
        string dataPath = Application.persistentDataPath;
        string saveFilePath = dataPath + "/" + SaveManager.Instance.ActiveSave + ".xml";

        if (System.IO.File.Exists(saveFilePath) && !string.IsNullOrEmpty(SaveManager.Instance.ActiveSave.starClassAsString))
        {
            StartCoroutine(DelayedForward());
        }
        else
        {
            StartButtonJiggle();
        }
    }

    private static IEnumerator DelayedBack()
    {
        yield return new WaitForSeconds(ConstantsUtil.BUTTON_ACTION_DELAY_SOUND);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    
    private static IEnumerator DelayedForward()
    {
        yield return new WaitForSeconds(ConstantsUtil.BUTTON_ACTION_DELAY_SOUND);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    private void StartButtonJiggle()
    {
        _jiggling = true;
        StartCoroutine(JiggleButton());
    }

    private IEnumerator JiggleButton()
    {
        float t = 0;
        Quaternion startRotation = SaveButton.transform.rotation;
        float angularDisplacement = JiggleAmount;
        float angularSpeed = angularDisplacement / JiggleDuration;

        while (_jiggling)
        {
            if (t >= JiggleDuration)
            {
                _jiggling = false;
                SaveButton.transform.localScale = _originalScale;
                SaveButton.transform.rotation = startRotation;
                break;
            }

            t += Time.deltaTime;

            float angleOffset = Mathf.Sin(t * angularSpeed * Mathf.PI * 2) * angularDisplacement / 2f;
            Quaternion newRotation = startRotation * Quaternion.Euler(0, 0, angleOffset);
            SaveButton.transform.rotation = newRotation;

            float scale = Mathf.Sin(t * angularSpeed * Mathf.PI * 2) * 0.05f + 1f;
            SaveButton.transform.localScale = _originalScale * scale;

            yield return null;
        }
        SaveButton.transform.rotation = startRotation;
        SaveButton.transform.localScale = _originalScale;
    }
}
