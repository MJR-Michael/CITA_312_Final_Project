using UnityEngine;

public class NPCReaction : MonoBehaviour
{
    public float minReactionTime = 0.2f;
    public float maxReactionTime = 1.0f;

    private bool npcHasReacted = false;
    private float npcReactionTimer = 0f;
    private bool timerActive = false;

    public System.Action OnNPCReacted;

    void Start()
    {
        ClockManager clock = FindFirstObjectByType<ClockManager>();
        clock.OnClockStrike12 += StartNPCReaction;
    }

    void StartNPCReaction()
    {
        npcReactionTimer = Random.Range(minReactionTime, maxReactionTime);
        timerActive = true;
    }

    void Update()
    {
        if (timerActive)
        {
            npcReactionTimer -= Time.deltaTime;

            if (npcReactionTimer <= 0f && !npcHasReacted)
            {
                npcHasReacted = true;
                timerActive = false;

                Debug.Log("NPC reacted!");
                OnNPCReacted?.Invoke();
            }
        }
    }
}
