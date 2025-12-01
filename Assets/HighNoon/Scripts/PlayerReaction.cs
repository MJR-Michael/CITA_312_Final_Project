using UnityEngine;

public class PlayerReaction : MonoBehaviour
{
    private bool canReact = false;
    private bool playerHasReacted = false;

    public System.Action OnPlayerReacted;

    void Start()
    {
        ClockManager clock = FindFirstObjectByType<ClockManager>();

        clock.OnClockStrike12 += () =>
        {
            canReact = true;
            playerHasReacted = false;
        };
    }

    void Update()
    {
        if (canReact && !playerHasReacted)
        {
            if (Input.GetKeyDown(KeyCode.Space)) // Your reaction key
            {
                playerHasReacted = true;
                canReact = false;

                Debug.Log("PLAYER reacted!");
                OnPlayerReacted?.Invoke();
            }
        }
    }
}
