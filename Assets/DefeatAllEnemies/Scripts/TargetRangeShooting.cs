using UnityEngine;
using UnityEngine.InputSystem;

public class TargetRangeShooting : MonoBehaviour
{
    public float range = 100f;
    public LayerMask targetLayer;
    public AudioClip hitSound;
    public float soundVolume = 1.0f;

    private DefeatAllActions controls;

    void Awake()
    {
        controls = new DefeatAllActions();
        controls.Combat.Fire.performed += ctx => Shoot();
    }

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }

    void Shoot()
    {
        // 🔥 Shoot from the center of the camera (crosshair point)
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range, targetLayer))
        {
            Debug.Log("Raycast hit: " + hit.collider.name);

            TargetController tc = hit.collider.GetComponent<TargetController>();

            if (tc != null && hit.collider.gameObject.activeSelf)
            {
                Debug.Log("Target hit & deactivated: " + hit.collider.name);

                tc.DeactivateTarget();

                if (hitSound != null)
                    AudioSource.PlayClipAtPoint(hitSound, hit.point, soundVolume);
            }
        }
        else
        {
            Debug.Log("Raycast hit nothing.");
        }
    }
}
