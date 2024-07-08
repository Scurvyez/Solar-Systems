using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace SolarSystemUI
{
    public class UI_SystemEvents : MonoBehaviour
    {
        public void Back()
        {
            StartCoroutine(DelayedBack());
        }
        
        private static IEnumerator DelayedBack()
        {
            yield return new WaitForSeconds(ConstantsUtil.BUTTON_ACTION_DELAY_SOUND);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }
}