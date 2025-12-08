using UnityEngine;

public class NPCReaction : MonoBehaviour
{
    public float minReactionTime = 0.2f;
    public float maxReactionTime = 1.0f;

    [Header("Audio")]
    public AudioSource npcAudioSource;   // Assign in inspector
    public AudioClip npcReactionClip;    // Sound for NPC reaction

    private bool npcHasReacted = false;
    private float npcReactionTimer = 0f;
    private bool timerActive = false;

    public System.Action OnNPCReacted;

    private PlayerReaction playerReaction;

    void Start()
    {
        // Find the player reaction script in the scene
        playerReaction = FindFirstObjectByType<PlayerReaction>();

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
                // Only react if the player hasn't already reacted correctly
                if (playerReaction == null || !playerReaction.HasReactedCorrectly())
                {
                    npcHasReacted = true;
                    timerActive = false;

                    Debug.Log("NPC reacted!");
                    OnNPCReacted?.Invoke();

                    if (npcAudioSource && npcReactionClip)
                        npcAudioSource.PlayOneShot(npcReactionClip);
                }
            }
        }
    }
}
