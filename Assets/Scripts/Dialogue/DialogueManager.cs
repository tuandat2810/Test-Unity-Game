using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    // Singleton pattern
    public static DialogueManager Instance;

    [Header("UI References")]
    public GameObject dialogueScreen;
    public GameObject dialoguePanel;      // The bottom panel for text
    public TextMeshProUGUI speakerNameText;
    public TextMeshProUGUI dialogueLineText;

    [Header("Choice References")]
    public GameObject choicePanel;        // The panel that holds the buttons
    public GameObject choiceButtonPrefab;

    [Header("UI References (Game UI)")]
    public GameObject hotbarPanel; 
    public GameObject skillPanel;

    // Private variables
    private DialogueNode currentNode;
    private PlayerStats playerStats;
    private List<GameObject> spawnedButtons = new List<GameObject>();

    private bool canSkip = false; // To prevent skipping immediately on dialogue start

    void Awake()
    {
        // Singleton setup
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        if (dialogueScreen != null)
            dialogueScreen.SetActive(false);
    }
    
    public void StartDialogue(DialogueNode startNode)
    {
        Time.timeScale = 0f;
        dialogueScreen.SetActive(true); 

        if (hotbarPanel != null) hotbarPanel.SetActive(false);
        if (skillPanel != null) skillPanel.SetActive(false);

        ShowNode(startNode);
    }

    // --- THIS FUNCTION HAS BEEN FIXED ---
    private void ShowNode(DialogueNode node)
    {
        currentNode = node;

        // 1. Always show the main dialogue panel
        dialoguePanel.SetActive(true);
        
        // 2. Set the text
        speakerNameText.text = node.speakerName + ":";
        dialogueLineText.text = node.dialogueLine;

        canSkip = false; // Reset skip flag

        // 3. Clear out old choice buttons
        foreach (GameObject button in spawnedButtons)
        {
            Destroy(button);
        }
        spawnedButtons.Clear();

        // 4. Check if this node has choices
        if (node.playerResponses.Count > 0)
        {
            // --- HAS CHOICES ---
            // Show the choice panel
            choicePanel.SetActive(true);
            
            // Create a button for each choice
            foreach (PlayerResponse response in node.playerResponses)
            {
                GameObject buttonGO = Instantiate(choiceButtonPrefab, choicePanel.transform);
                spawnedButtons.Add(buttonGO);

                TextMeshProUGUI buttonText = buttonGO.GetComponentInChildren<TextMeshProUGUI>();
                buttonText.text = response.responseText;

                Button button = buttonGO.GetComponent<Button>();
                button.onClick.AddListener(() => {
                    SelectChoice(response);
                });
            }
        }
        else
        {
            // --- NO CHOICES (Linear dialogue) ---
            // Hide the choice panel
            choicePanel.SetActive(false);

            StartCoroutine(EnableSkipAfterFrame());
        }
    }
    
    // (SelectChoice function remains the same)
    private void SelectChoice(PlayerResponse response)
    {
        canSkip = false; // Reset skip flag

        if (response.responseText.Contains("[CHALLENGE]"))
        {
            playerStats.EnterCombatState();
            EndDialogue();
            return;
        }

        if (response.nextNode != null)
        {
            ShowNode(response.nextNode);
        }
        else
        {
            EndDialogue();
        }
    }


    // (Update function remains the same)
    void Update()
    {
        if (!dialogueScreen.activeInHierarchy)
        {
            return; // Dialogue is not active
        }

        // Check if we are in a "linear" node AND the "gate" is open
        if (currentNode != null && currentNode.playerResponses.Count == 0 && canSkip)
        {
            // If player clicks (or presses F, J, etc.)
            if (Input.GetKeyDown(KeyCode.F) || Input.GetButtonDown("Fire1"))
            {
                if (currentNode.linearNextNode != null)
                {
                    ShowNode(currentNode.linearNextNode);
                }
                else
                {
                    EndDialogue();
                }
            }
        }
    }

    // Coroutine to enable skipping after one frame
    private IEnumerator EnableSkipAfterFrame()
    {
        yield return null; // Wait for one frame
        canSkip = true;
    }
    
    // (EndDialogue function remains the same)
    private void EndDialogue()
    {
        dialogueScreen.SetActive(false);
        Time.timeScale = 1f; // Unpause

        if (hotbarPanel != null) hotbarPanel.SetActive(true);
        if (skillPanel != null) skillPanel.SetActive(true);
    }
}