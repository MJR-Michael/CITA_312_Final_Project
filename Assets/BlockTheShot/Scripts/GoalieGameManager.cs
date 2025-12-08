using UnityEngine;

public class GoalieGameManager : MonoBehaviour
{
    public MinigameManager manager;

    public void GameOver()
    {
        Debug.Log("GAME OVER — The ball hit the net!");
        manager.LoseMinigame();
    }

    public void Win()
    {
        Debug.Log("WIN — The ball missed the net or hit the goalie!");
        manager.WinMinigame();

    }
}
