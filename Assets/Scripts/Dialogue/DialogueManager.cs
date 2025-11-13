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
        playerStats = FindObjectOfType<PlayerStats>();
        dialogueScreen.SetActive(false);
    }
    
    public void StartDialogue(DialogueNode startNode)
    {
        Time.timeScale = 0f;
        dialogueScreen.SetActive(true);
        ShowNode(startNode);
    }

    // --- THIS FUNCTION HAS BEEN FIXED ---
    private void ShowNode(DialogueNode node)
    {
        currentNode = node;

        // 1. Always show the main dialogue panel
        dialoguePanel.SetActive(true);
        
        // 2. Set the text
        speakerNameText.text = node.speakerName;
        dialogueLineText.text = node.dialogueLine;

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
        }
    }
    
    // (SelectChoice function remains the same)
    private void SelectChoice(PlayerResponse response)
    {
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
            return;
        }

        if (currentNode != null && currentNode.playerResponses.Count == 0)
        {
            if (Input.GetKeyDown(KeyCode.F) || Input.GetButtonDown("Fire1"))
            {
                EndDialogue();
            }
        }
    }
    
    // (EndDialogue function remains the same)
    private void EndDialogue()
    {
        dialogueScreen.SetActive(false);
        Time.timeScale = 1f; // Unpause
    }
}