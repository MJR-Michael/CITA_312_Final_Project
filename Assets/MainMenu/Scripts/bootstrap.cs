using UnityEngine;
using UnityEngine.SceneManagement;

public class BootstrapLoader : MonoBehaviour
{
    void Start()
    {
        // Load your main menu or starting scene
        SceneManager.LoadScene("MainMenu");
    }
}
