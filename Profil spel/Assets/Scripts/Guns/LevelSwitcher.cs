using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSwitcher : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            LoadLevel("Main menu");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            LoadLevel("Main menu - Copy");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            LoadLevel("SampleScene");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            LoadLevel("SampleScene 1");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            LoadLevel("SampleScene 2");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            LoadLevel("sigma");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            LoadLevel("Test");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            LoadLevel("Scene8");
        }
    }

    void LoadLevel(string sceneName)
    {
        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("Invalid scene name: " + sceneName);
        }
    }
}
