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
    public GameObject dialoguePanel;
    public TextMeshProUGUI speakerNameText;
    public TextMeshProUGUI dialogueLineText;

    [Header("Choice References")]
    public GameObject choicePanel;
    public GameObject choiceButtonPrefab;

    // Private variables
    private DialogueNode currentNode;
    private PlayerStats playerStats;
    private List<GameObject> spawnedButtons = new List<GameObject>();

    void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Find the PlayerStats once
        playerStats = FindObjectOfType<PlayerStats>(); // Assumes only one Player
        
        // Start with the UI hidden
        dialogueScreen.SetActive(false);
    }

    // This is called by the NPC when interaction starts
    public void StartDialogue(DialogueNode startNode)
    {
        // Pause the game (like in CharacterScreenUI)
        Time.timeScale = 0f;

        // Turn on the UI
        dialogueScreen.SetActive(true);
        
        // (Optional: Hide other UI like Hotbar/SkillPanel)
        // UIManager.Instance.HideGameUI(); 
        
        // Start processing the first node
        ShowNode(startNode);
    }

    // This function displays a single node's content
    private void ShowNode(DialogueNode node)
    {
        currentNode = node;

        // 1. Set the text
        speakerNameText.text = node.speakerName;
        dialogueLineText.text = node.dialogueLine;

        // 2. Clear out old choice buttons
        foreach (GameObject button in spawnedButtons)
        {
            Destroy(button);
        }
        spawnedButtons.Clear();

        // 3. Check if this node has choices or is linear
        if (node.playerResponses.Count > 0)
        {
            // --- HAS CHOICES ---
            // Show the choice panel, hide the main dialogue text
            dialoguePanel.SetActive(false);
            choicePanel.SetActive(true);
            
            // Create a button for each choice
            foreach (PlayerResponse response in node.playerResponses)
            {
                // Create the button from the prefab
                GameObject buttonGO = Instantiate(choiceButtonPrefab, choicePanel.transform);
                spawnedButtons.Add(buttonGO);

                // Set the button's text
                TextMeshProUGUI buttonText = buttonGO.GetComponentInChildren<TextMeshProUGUI>();
                buttonText.text = response.responseText;

                // Add a listener to the button's onClick event
                // This tells the button what to do when clicked
                Button button = buttonGO.GetComponent<Button>();
                button.onClick.AddListener(() => {
                    SelectChoice(response);
                });
            }
        }
        else
        {
            // --- NO CHOICES (Linear dialogue) ---
            // Show the main dialogue, hide the choice panel
            dialoguePanel.SetActive(true);
            choicePanel.SetActive(false);
            
            // We'll add a "click to continue" check in Update()
        }
    }
    
    // This is called by the button's listener
    private void SelectChoice(PlayerResponse response)
    {
        // Check for special "combat" keyword
        if (response.responseText.Contains("[CHALLENGE]"))
        {
            // This is a special command, not a new node
            playerStats.EnterCombatState();
            EndDialogue();
            return;
        }

        // If it's a normal choice, go to the next node
        if (response.nextNode != null)
        {
            ShowNode(response.nextNode);
        }
        else // If this choice leads to null, end the conversation
        {
            EndDialogue();
        }
    }

    // This handles the "click to continue"
    void Update()
    {
        // Only run if the dialogue UI is active
        if (!dialogueScreen.activeInHierarchy)
        {
            return;
        }

        // Check if we are in a "linear" node (no choices)
        if (currentNode != null && currentNode.playerResponses.Count == 0)
        {
            // If player clicks (or presses F, J, etc.)
            if (Input.GetKeyDown(KeyCode.F) || Input.GetButtonDown("Fire1"))
            {
                // This was the last line
                EndDialogue();
            }
        }
    }
    
    // Cleans up the UI and unpauses the game
    private void EndDialogue()
    {
        dialogueScreen.SetActive(false);
        Time.timeScale = 1f; // Unpause
        
        // (Optional: Show game UI again)
        // UIManager.Instance.ShowGameUI();
    }

}
