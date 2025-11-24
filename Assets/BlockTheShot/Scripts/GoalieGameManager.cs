using UnityEngine;

public class GoalieGameManager : MonoBehaviour
{
    public GameObject loseScreen; // A UI element that will show when the game is lost
    public float resetDelay = 3f; // Delay before resetting the game or reloading the scene

    // Method to trigger the lose sequence
    public void TriggerLoseSequence()
    {
        // Show the lose screen (you can enable/disable UI elements here)
        if (loseScreen != null)
        {
            loseScreen.SetActive(true); // Show the "you lost" UI
        }

        // Optionally, disable player input or other actions while in the lose state
        // For example, disable the ball and goalie controls
        Time.timeScale = 0f; // Freeze the game

        // Reset or restart after a delay
        Invoke("ResetGame", resetDelay); // Call ResetGame after a delay
    }

    // Reset or reload the game
    private void ResetGame()
    {
        // Here you can reload the scene, reset the ball position, etc.
        // Reload the current scene:
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);

        // Or reset game objects manually (like moving the ball back to its starting position):
        // Reset other game objects or player states here.
    }
}
