using UnityEngine;

public class ReactionManager : MonoBehaviour
{
    private bool npcReacted = false;
    private bool playerReacted = false;
    private bool early = false;

    void Start()
    {
        NPCReaction npc = FindFirstObjectByType<NPCReaction>();
        PlayerReaction player = FindFirstObjectByType<PlayerReaction>();

        npc.OnNPCReacted += NPCWins;
        player.OnPlayerReacted += PlayerWins;
        player.OnPlayerEarly += PlayerReactedTooEarly;
    }

    void NPCWins()
    {
        if (!playerReacted && !early)
        {
            npcReacted = true;
            Debug.Log("NPC wins! Player was too slow!");
        }
    }

    void PlayerWins()
    {
        if (!npcReacted && !early)
        {
            playerReacted = true;
            Debug.Log("Player wins! Fastest reaction!");
        }
    }

    void PlayerReactedTooEarly()
    {
        early = true;
        Debug.Log("Player loses! You reacted too early!");
    }
}
