using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCDialogueTrigger : MonoBehaviour
{
    public GameObject dialogueBox; // panel
    public Text npcText;
    public Button[] responseButtons;
    public Image reactionSprite; // Image to display the reaction sprite
    public Sprite[] reactions; // Array of reaction sprites

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ShowDialogue();
        }
    }

    private void ShowDialogue()
    {
        dialogueBox.SetActive(true);
        npcText.text = "Hello! Do my eyelashes look good?";
        SetupResponses();
    }

    private void SetupResponses()
    {
        responseButtons[0].onClick.AddListener(() => Respond(0));
        responseButtons[1].onClick.AddListener(() => Respond(1));
        //responseButtons[2].onClick.AddListener(() => Respond(2));

        responseButtons[0].GetComponentInChildren<Text>().text = "Yes!";
        responseButtons[1].GetComponentInChildren<Text>().text = "Ehhhhh... what eyelashes?";
       // responseButtons[2].GetComponentInChildren<Text>().text = "Option 3";
    }

    private void Respond(int responseIndex)
    {
        if (reactionSprite == null)
        {
            Debug.LogError("Reaction sprite not assigned");
            return;
        }

        if (responseIndex < 0 || responseIndex >= reactions.Length)
        {
            Debug.LogError("Invalid response index");
            return;
        }

        // Set the sprite and show the reaction image
        reactionSprite.sprite = reactions[responseIndex];
        //reactionSprite.transform.position = --add sprite?--Transform.position + new Vector3(0, 1f, 0);
        reactionSprite.gameObject.SetActive(true);

        Invoke("HideReactionImage", 2f);

        if (dialogueBox != null)
        {
            dialogueBox.SetActive(false);
        }
    }

    private void HideReactionImage()
    {
        reactionSprite.gameObject.SetActive(false);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            dialogueBox.SetActive(false);
        }
    }
}
