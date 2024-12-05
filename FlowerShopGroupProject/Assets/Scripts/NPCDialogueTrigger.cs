using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class NPCDialogueTrigger : MonoBehaviour
{
    public GameObject dialogueBox; // panel
    public TextMeshProUGUI npcText;
    public Button[] responseButtons;
    public Image reactionSprite; // Image to display the reaction sprite
    public Sprite[] reactions; // Array of reaction sprites
    public string[] dialogues; // Array of dialogue strings for different interactions
    public float dialogueCooldown = 60f; // Cooldown in seconds
    public string defaultMessage = "I'm busy. Comeback later"; // message during coooldown

    private int interactionCount = 0; // Tracks the number of interactions
    private bool isOnCooldown = false; // Tracks if the NPC is on cooldown
    private float lastInteractionTime = -60f; // Time of the last interaction
    private bool hasResponded = false;

    // for xp stuff 
    public int xpReward = 10; // XP points for positive reactions
    public int xpPenalty = 1; // XP points removed for negative reactions
    public int[] reactionTypes; // Array to indicate reaction type

    // Reference to PlayerInventory for bouquet stuff
    private PlayerInventory playerInventory;
    public GameObject bouquetReceivedSprite;

    private bool bouquetReceived = false; // Tracks if the bouquet has been given
    public string dateProposalDialogue = "Would you like to go on a date with me?"; // Date dialogue
    public string dateResponse = "Yes, I'd love to!";
    private bool dateProposalTriggered = false;

    void Start()
    {
        // Find the PlayerInventory instance
        playerInventory = PlayerInventory.Instance;
        if (playerInventory == null)
        {
            Debug.LogError("PlayerInventory instance not found");
        }
    }

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
        hasResponded = false;

        if (Time.time - lastInteractionTime < dialogueCooldown)
        {
            npcText.text = defaultMessage; // Show default message during cooldown
            foreach (var button in responseButtons)
            {
                button.gameObject.SetActive(false); // Hide buttons during cooldown
            }
        }
        // give player date proposal option 
        else if (bouquetReceived && !dateProposalTriggered)
        {
            npcText.text = "Through much persistance and facing your fears you've mustered up enough courage and Love XP... Would you like to ask Them on a Date?";
            SetupDateProposalButtons();
        }

        else if (!hasResponded)
        {
            lastInteractionTime = Time.time;

            if (interactionCount < dialogues.Length)
            {
                npcText.text = dialogues[interactionCount];
            }
            else if (playerInventory != null && PlayerStats.playerXP >= 10 && playerInventory.HasBouquet())
            {
                npcText.text = "You can give me a bouquet using the bouquet tool.";
                //responseButtons.gameObject.SetActive(false); // Hide buttons during this message
                
            }
            else
            {
                npcText.text = "We've talked enough for now.";
            }
            // Re-enable buttons after cooldown and bouquet 
            foreach (var button in responseButtons)
            {
                button.gameObject.SetActive(true);
            }
            SetupResponses();
        }
    }

    // for giving bouquet 
    public void ReceiveBouquet()
    {
        bouquetReceived = true;
        npcText.text = "Wow! Thank you for the bouquet!";
        playerInventory.UseBouquet(); // Deduct bouquet
        if (bouquetReceivedSprite != null)
        {
            bouquetReceivedSprite.SetActive(true);
        }
    }

    private void SetupResponses()
    {
        responseButtons[0].onClick.RemoveAllListeners();
        responseButtons[1].onClick.RemoveAllListeners();

        responseButtons[0].onClick.AddListener(() => Respond(0));
        responseButtons[1].onClick.AddListener(() => Respond(1));

        if (interactionCount == 0)
        {
            //"Well hello there.. whats your name?"
            responseButtons[0].GetComponentInChildren<TMP_Text>().text = "[Lie and Say your name is] Morty, nice to meet you!";
            responseButtons[1].GetComponentInChildren<TMP_Text>().text = "Hello! Well... at my last job they called me the executioner!";
        }
        else if (interactionCount == 1)
        {
            // "Oh you again! So you're new here I hear. Where were you befor? What brought you here?"
            responseButtons[0].GetComponentInChildren<TMP_Text>().text = "Oh you know, just wanted to get away from the rat race of the city and start fresh";
            responseButtons[1].GetComponentInChildren<TMP_Text>().text = "[Tell the truth, that's the right thing to do... right?] I escaped my job as a soul collector in the underworld because I like flowers and girls";

        }
        else if (interactionCount > 1) 
        {
            // "I really enjoy your devilish appearence today... How do you like my eyelashes?"
            responseButtons[0].GetComponentInChildren<TMP_Text>().text = "Thank you. I thought I'd get dressed up since you're here so often and your eylashes are so lovely";
            responseButtons[1].GetComponentInChildren<TMP_Text>().text = "Well in the underworld your eyelashes would've been burnt off by now so not very practical";

        }

    }

    private void Respond(int responseIndex)
    {
        // Update the reaction sprite
        reactionSprite.sprite = reactions[responseIndex];
        reactionSprite.gameObject.SetActive(true);

        // Adjust player XP based on the reaction type
        if (responseIndex < reactionTypes.Length)
        {
            if (reactionTypes[responseIndex] == 0)
            {
                PlayerStats.playerXP += xpReward; // Positive reaction adds XP
                Debug.Log("Positive reaction! Player gained XP.");
            }
            else if (reactionTypes[responseIndex] == 1)
            {
                PlayerStats.playerXP -= xpPenalty; // Negative reaction removes XP
                Debug.Log("Negative reaction! Player lost XP.");
            }
        }

        Invoke("HideReactionImage", 2f);
        // Mark the response as completed and allow the dialogue to progress
        hasResponded = true;
        // add interaction count after responds
        interactionCount++;
        dialogueBox.SetActive(false);
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

    // date stuff --------------------------------
    private void SetupDateProposalButtons()
    {
        foreach (var button in responseButtons)
        {
            button.gameObject.SetActive(true);
        }

        responseButtons[0].GetComponentInChildren<TMP_Text>().text = "Ask on a date";
        responseButtons[1].GetComponentInChildren<TMP_Text>().text = "Not now";

        responseButtons[0].onClick.RemoveAllListeners();
        responseButtons[1].onClick.RemoveAllListeners();

        responseButtons[0].onClick.AddListener(() => TriggerDateProposal());
        responseButtons[1].onClick.AddListener(() => DeclineDateProposal());
    }

    private void TriggerDateProposal()
    {
        foreach (var button in responseButtons)
        {
            button.gameObject.SetActive(false);
        }
        dateProposalTriggered = true;
        npcText.text = dateProposalDialogue;
        Invoke("SwitchToDateScene", 3f);
    }

    private void DeclineDateProposal()
    {
        npcText.text = "Maybe another time.";
        dialogueBox.SetActive(false);
    }

    private void SwitchToDateScene()
    {
        SceneManager.LoadScene("EndScene");
    }
}