using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;

    [Header("Shoot Settings")]
    public float shootRange = 1000f;

    [Header("Scope Settings")]
    public float scopedFOV = 150f;
    public float normalFOV = 80f;
    public float scopeSpeed = 20f;

    bool isScoped = false;
    bool hasShotWrongNPC = false; // track wrong NPC shots

    SnipeActions input;

    void Awake()
    {
        input = new SnipeActions();

        input.Combat.Fire.performed += ctx => TryShoot();
        input.Combat.Scope.performed += ctx => ToggleScope();
    }

    void OnEnable()  => input.Combat.Enable();
    void OnDisable() => input.Combat.Disable();

    void Update()
    {
        HandleScopeFOV();
    }

    // -- SHOOTING ---------------------------------------------------------------
    void TryShoot()
    {
        if (hasShotWrongNPC)
        {
            Debug.Log("You already shot the wrong NPC! No more shots.");
            return;
        }

        if (!isScoped)
        {
            Debug.Log("Cannot shoot unless scoped!");
            return;
        }

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, shootRange))
        {
            Debug.Log("You hit: " + hit.collider.name);

            var npc = hit.collider.GetComponentInParent<NPCIdentifier>();
            if (npc != null)
            {
                npc.OnShotByPlayer();

                // If it's the wrong NPC, block further shots
                if (!npc.isTarget)
                {
                    hasShotWrongNPC = true;
                }
            }
        }
        else
        {
            Debug.Log("Missed shot");
            // Do NOT block shooting here
        }
    }

    // -- SCOPING ---------------------------------------------------------------
    void ToggleScope()
    {
        isScoped = !isScoped;
    }

    void HandleScopeFOV()
    {
        float targetFov = isScoped ? scopedFOV : normalFOV;
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFov, Time.deltaTime * scopeSpeed);
    }
}
