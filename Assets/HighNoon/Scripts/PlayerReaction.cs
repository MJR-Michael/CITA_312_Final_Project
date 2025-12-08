using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerReaction : MonoBehaviour
{
    [Header("Reaction Input")]
    public InputAction reactionAction;

    [Header("Audio")]
    public AudioSource reactionAudioSource; // Assign an AudioSource in the inspector
    public AudioClip correctReactionClip;   // Sound for correct reaction
    public AudioClip earlyReactionClip;     // Sound for early reaction

    private bool canReact = false;        // true only after clock hits 12
    private bool playerHasReacted = false;
    private bool roundStarted = false;    // becomes true at start of scene

    public System.Action OnPlayerReacted;
    public System.Action OnPlayerEarly;

    void OnEnable()
    {
        reactionAction.Enable();
    }

    void OnDisable()
    {
        reactionAction.Disable();
    }

    void Start()
    {
        roundStarted = true;

        ClockManager clock = FindFirstObjectByType<ClockManager>();

        clock.OnClockStrike12 += () =>
        {
            canReact = true;
            playerHasReacted = false;
        };
    }

    void Update()
    {
        // Only care about player input if round has begun
        if (!playerHasReacted && roundStarted)
        {
            if (reactionAction.WasPerformedThisFrame())
            {
                // Player pressed too early (before 12:00)
                if (!canReact)
                {
                    Debug.Log("PLAYER reacted too early!");
                    playerHasReacted = true;
                    OnPlayerEarly?.Invoke();

                    if (reactionAudioSource && earlyReactionClip)
                        reactionAudioSource.PlayOneShot(earlyReactionClip);

                    return;
                }

                // Player pressed at correct time (after 12:00)
                playerHasReacted = true;
                canReact = false;

                Debug.Log("PLAYER reacted!");
                OnPlayerReacted?.Invoke();

                if (reactionAudioSource && correctReactionClip)
                    reactionAudioSource.PlayOneShot(correctReactionClip);
            }
        }
    }

    /// <summary>
    /// Returns true if the player reacted correctly (after clock hit 12)
    /// </summary>
    public bool HasReactedCorrectly()
    {
        return playerHasReacted && !canReact;
    }
}
