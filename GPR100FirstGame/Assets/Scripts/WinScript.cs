using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinScript : MonoBehaviour
{
    public Text convoTxt;
    public float displayTime = 3f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (convoTxt != null)
            {
                StartCoroutine(DisappearAfterTime(displayTime));
                convoTxt.gameObject.SetActive(true);
            }

            if (!convoTxt.enabled) { WinGame(); }
        }
    }

    IEnumerator DisappearAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        // Disable text
        convoTxt.enabled = false;
    }

    void WinGame()
    {
        SceneManager.LoadScene("WinScene");
    }
}