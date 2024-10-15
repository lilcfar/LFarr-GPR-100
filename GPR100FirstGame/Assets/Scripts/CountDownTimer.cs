using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CountDownTimer : MonoBehaviour
{
    public static float timeLeft = 60;
    public Text timerTxt;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        UpdateTimer(timeLeft);
        //displayTime = timeLeft.ToString();

        if (timeLeft < 0)
        {
            SceneManager.LoadScene("LoseScene");
        }


    }

    void UpdateTimer(float timeleft)
    {
        timerTxt.text = timeleft.ToString("00");
    }
}
