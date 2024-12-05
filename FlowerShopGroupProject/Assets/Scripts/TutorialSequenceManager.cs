using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialSequenceManager : MonoBehaviour
{
    public GameObject[] tutorialPanels; 
    public float displayDuration = 8f; // Time to display each panel

    private int currentPanelIndex = 0;

    void Start()
    {
        // Start the tutorial sequence
        ShowPanel(currentPanelIndex);
    }

    void ShowPanel(int index)
    {
        // Deactivate all panels first
        foreach (var panel in tutorialPanels)
        {
            panel.SetActive(false);
        }

        // Activate the current panel
        if (index < tutorialPanels.Length)
        {
            tutorialPanels[index].SetActive(true);

            // Schedule the next panel 
            if (index == tutorialPanels.Length - 1)
            {
                // no hide the last panel
                return;
            }

            Invoke(nameof(NextPanel), displayDuration);
        }
    }

    void NextPanel()
    {
        currentPanelIndex++;
        ShowPanel(currentPanelIndex);
    }

    public void LoadNextScene(string nextScene)
    {
        SceneManager.LoadScene(nextScene);
    }


}