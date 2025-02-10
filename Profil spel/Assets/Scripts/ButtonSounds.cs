using UnityEngine;
using UnityEngine.UIElements;

public class ButtonSounds : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clickSound;

    private Button startButton;
    private Button quitButton;
    private Button settingsButton;

    void OnEnable()
    {
        // Load the UI Document
        var uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;

        // Get buttons from UI Toolkit
        startButton = root.Q<Button>("Start_Button");
        quitButton = root.Q<Button>("Quit_Button");
        settingsButton = root.Q<Button>("Settings_Button");

        // Add click event listeners
        startButton.clicked += PlaySound;
        quitButton.clicked += PlaySound;
        settingsButton.clicked += PlaySound;
    }

    void PlaySound()
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
        else
        {
            Debug.LogError("AudioSource or ClickSound is missing!");
        }
    }
}
