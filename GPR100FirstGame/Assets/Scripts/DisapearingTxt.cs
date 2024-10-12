using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DisapearingTxt : MonoBehaviour
{
    public Text uiText;
    public float displayTime = 7f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DisappearAfterTime(displayTime));
    }

    IEnumerator DisappearAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        // Disable text
        uiText.enabled = false;
    }
}
