using UnityEngine;

public class NPCIdentifier : MonoBehaviour
{
    public bool isTarget;
    public MinigameManager manager;

    public void OnShotByPlayer()
    {
        if (isTarget)
        {
            Debug.Log("TARGET KILLED — YOU WIN");
            manager.WinMinigame();
        }
        else
        {
            Debug.Log("WRONG NPC — YOU LOSE");
            manager.LoseMinigame();
        }
    }
}
