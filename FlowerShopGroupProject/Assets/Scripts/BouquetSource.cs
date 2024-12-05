using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// will add feature so it displays bouquet count in player inventory and also doesnt
// let the player use/click the tool if they dont have ant bouquets 
public class BouquetSource : MonoBehaviour
{

    private TextMeshProUGUI BouquetCountTool;
    public TextMeshProUGUI popupText; // Assign this in the inspector to your popup text UI
    public float popupDuration = 2f;

    public static BouquetSource Instance;
    private bool hasBouquet = false;

    public GameObject bouquetSpritePrefab;
    private GameObject bouquetSpriteInstance;

    // Prevents other tools from being used simultaneously
    public static bool isOtherToolInUse = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Instantiate and hide the bouquet sprite initially
        bouquetSpriteInstance = Instantiate(bouquetSpritePrefab);
        bouquetSpriteInstance.SetActive(false);

        // bouquet toolbar count 
        BouquetCountTool = GameObject.Find("BouquetCountTool")?.GetComponent<TextMeshProUGUI>();
        UpdateBouquetCountText();
    }
    public void UpdateBouquetCountText()
    {
        if (BouquetCountTool != null)
        {
            BouquetCountTool.text = PlayerInventory.Instance.HasBouquet()
                ? PlayerInventory.Instance.BouquetCount.ToString()
                : "0";
        }
    }

    public void ActivateBouquetTool()
    {
        if (ShovelSource.isOtherToolInUse || WaterSource.isOtherToolInUse)
        {
            Debug.Log("Another tool is currently in use!");
            return;
        }
        // Check XP
        if (PlayerStats.playerXP < 30)
        {
            ShowPopup("You need at least 30 XP to use the bouquet tool!");
            return;
        }
        if (hasBouquet) 
        {
            DeactivateBouquetTool();
        }
        else
        {
            hasBouquet = true;
            isOtherToolInUse = true;
            bouquetSpriteInstance.SetActive(true);
            Debug.Log("Bouquet tool activated!");
        }
    }

    public void DeactivateBouquetTool()
    {
        hasBouquet = false;
        isOtherToolInUse = false;
        bouquetSpriteInstance.SetActive(false);
        Debug.Log("Bouquet tool deactivated!");
    }

    private void Update()
    {
        // If holding a bouquet, follow the cursor
        if (hasBouquet)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0; 
            bouquetSpriteInstance.transform.position = mousePosition;

            // Check for a click to interact with an NPC tagged sprite
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
                if (hit.collider != null && hit.collider.CompareTag("NPC"))
                {
                    NPCDialogueTrigger npc = hit.collider.GetComponent<NPCDialogueTrigger>();
                    if (npc != null)
                    {
                        npc.ReceiveBouquet();
                        DeactivateBouquetTool();
                    }
                }
            }
        }
    }


    // for popup
    private void ShowPopup(string message)
    {
        if (popupText != null)
        {
            popupText.text = message;
            popupText.gameObject.SetActive(true);
            CancelInvoke(nameof(HidePopup));
            Invoke(nameof(HidePopup), popupDuration);
        }
    }

    private void HidePopup()
    {
        if (popupText != null)
        {
            popupText.gameObject.SetActive(false);
        }
    }

    // ------------------ 
    private void OnMouseDown()
    {
        Debug.Log("BouquetSource clicked!");
        ActivateBouquetTool();
    }
}