using UnityEngine;
using TMPro;
using System.Collections;

public class ReactionManager : MonoBehaviour
{
    private bool npcReacted = false;
    private bool playerReacted = false;
    private bool early = false;

    [Header("UI")]
    public RectTransform reactionTextRect;     // Assign the Text's RectTransform
    public TextMeshProUGUI reactionTextUI;     // Assign the TextMeshProUGUI
    public float moveDuration = 1.5f;          // How long to move to center
    public Vector2 centerPosition = Vector2.zero; // Usually (0,0) for canvas
    public MinigameManager manager;

    private Coroutine moveCoroutine;

    void Start()
    {
        NPCReaction npc = FindFirstObjectByType<NPCReaction>();
        PlayerReaction player = FindFirstObjectByType<PlayerReaction>();

        npc.OnNPCReacted += NPCWins;
        player.OnPlayerReacted += PlayerWins;
        player.OnPlayerEarly += PlayerReactedTooEarly;

        // Optionally, start text off-screen
        if (reactionTextRect != null)
            reactionTextRect.anchoredPosition = new Vector2(0, 500);
    }

    void NPCWins()
    {
        if (!playerReacted && !early)
        {
            npcReacted = true;
            Debug.Log("NPC wins! Player was too slow!");
            ShowReactionText("You were bested");
            manager.LoseMinigame();
        }
    }

    void PlayerWins()
    {
        if (!npcReacted && !early)
        {
            playerReacted = true;
            Debug.Log("Player wins! Fastest reaction!");
            ShowReactionText("Victorious!");
            manager.WinMinigame();
        }
    }

    void PlayerReactedTooEarly()
    {
        early = true;
        Debug.Log("Player loses! You reacted too early!");
        ShowReactionText("You were bested");
        manager.LoseMinigame();
    }

    private void ShowReactionText(string message)
    {
        if (reactionTextUI == null || reactionTextRect == null) return;

        reactionTextUI.text = message;

        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        moveCoroutine = StartCoroutine(MoveTextToCenter());
    }

    private IEnumerator MoveTextToCenter()
    {
        Vector2 startPos = reactionTextRect.anchoredPosition;
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / moveDuration;
            t = Mathf.SmoothStep(0f, 1f, t);
            reactionTextRect.anchoredPosition = Vector2.Lerp(startPos, centerPosition, t);
            yield return null;
        }

        reactionTextRect.anchoredPosition = centerPosition;
    }
}
