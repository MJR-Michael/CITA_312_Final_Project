using UnityEngine;

public class ReactionManager : MonoBehaviour
{
    private bool npcReacted = false;
    private bool playerReacted = false;

    void Start()
    {
        NPCReaction npc = FindFirstObjectByType<NPCReaction>();
        PlayerReaction player = FindFirstObjectByType<PlayerReaction>();

        npc.OnNPCReacted += NPCWins;
        player.OnPlayerReacted += PlayerWins;
    }

    void NPCWins()
    {
        if (!playerReacted)
        {
            npcReacted = true;
            Debug.Log("NPC wins! Player was too slow!");
        }
    }

    void PlayerWins()
    {
        if (!npcReacted)
        {
            playerReacted = true;
            Debug.Log("Player wins! You reacted faster!");
        }
    }
}
