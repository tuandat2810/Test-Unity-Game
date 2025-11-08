using UnityEngine;

public class CharacterScreenUI : MonoBehaviour
{
    public GameObject characterScreenPanel;

    private bool isPanelOpen = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
  
    void Start()
    {
        // eNsure the panel is closed at the start
        if (characterScreenPanel != null)
        {
            characterScreenPanel.SetActive(false);
            isPanelOpen = false;
        }
    }

    void Update()
    {
        // Listen for the Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Toggle the panel
            if (isPanelOpen)
            {
                CloseCharacterScreen();
            }
            else
            {
                OpenCharacterScreen();
            }
        }
    }

    public void OpenCharacterScreen()
    {
        isPanelOpen = true;
        characterScreenPanel.SetActive(true);
        
        // Pause the game by setting time scale to 0
        Time.timeScale = 0f; 
    }

    public void CloseCharacterScreen()
    {
        isPanelOpen = false;
        characterScreenPanel.SetActive(false);
        
        // Unpause the game by setting time scale back to 1
        Time.timeScale = 1f;
    }
}
