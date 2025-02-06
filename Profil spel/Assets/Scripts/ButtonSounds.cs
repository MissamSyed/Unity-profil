using UnityEngine;
using UnityEngine.UI; // Import Unity's UI namespace to interact with UI elements

public class PlayButtonSound : MonoBehaviour
{
    public AudioSource audioSource; // Reference to the AudioSource
    public Button yourButton; // Reference to the UI Button

    void Start()
    {
        // Ensure the button has a listener to call PlaySound() when clicked
        yourButton.onClick.AddListener(PlaySound);
    }

    // This method will play the sound
    void PlaySound()
    {
        audioSource.Play(); // Play the audio clip on the AudioSource
    }
}
