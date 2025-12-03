using UnityEngine;

public class NPCIdentifier : MonoBehaviour
{
    public bool isTarget;

    public void OnShotByPlayer()
    {
        if (isTarget)
            Debug.Log("TARGET KILLED — YOU WIN");
        else
            Debug.Log("WRONG NPC — YOU LOSE");
    }
}
