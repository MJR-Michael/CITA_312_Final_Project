using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    private NPCKillerChase npc;

    void Start()
    {
        npc = GetComponentInParent<NPCKillerChase>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            npc.PlayerCaught();
    }
}
