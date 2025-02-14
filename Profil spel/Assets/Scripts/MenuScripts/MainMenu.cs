using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements; // Required for UI Toolkit

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        // Match the exact button names from UI Toolkit
        Button startButton = root.Q<Button>("Start_Button");
        Button quitButton = root.Q<Button>("Quit_Button");

        // Check if start button is found and assign the click handler
        if (startButton != null)
        {
            startButton.clicked += PlayGame;
        }
        else
        {
            Debug.LogError("Start button not found in UI Toolkit!");
        }

        // Check if quit button is found and assign the click handler
        if (quitButton != null)
        {
            quitButton.clicked += QuitGame;
        }
        else
        {
            Debug.LogError("Quit button not found in UI Toolkit!");
        }
    }

    private void PlayGame()
    {
        Debug.Log("Start button clicked! Loading SampleScene...");
        SceneManager.LoadScene("Test");
    }

    private void QuitGame()
    {
        Debug.Log("Quit button clicked!");

        // Quit the game or stop play mode in the editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
