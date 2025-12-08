using UnityEngine;
using System.Collections;

public class TargetController : MonoBehaviour
{
    public float animationDuration = 0.5f;

    private Vector3 deactivatedPositionOffset = new Vector3(0, 3f, 0);
    private bool isDeactivated = false;
    private float initialYRotation;
    private TargetManager manager;

    void Start()
    {
        initialYRotation = transform.eulerAngles.y;
    }

    public void AssignManager(TargetManager mgr)
    {
        manager = mgr;
    }

    public void DeactivateTarget()
    {
        if (!isDeactivated)
        {
            isDeactivated = true;
            manager.TargetHit();   // notify manager
            StartCoroutine(AnimateDeactivation());
        }
    }

    private IEnumerator AnimateDeactivation()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + deactivatedPositionOffset;

        Quaternion startRot = transform.rotation;
        Quaternion endRot = Quaternion.Euler(90f, initialYRotation, 0f);

        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / animationDuration);

            transform.position = Vector3.Lerp(startPos, endPos, t);
            transform.rotation = Quaternion.Lerp(startRot, endRot, t);

            yield return null;
        }

        transform.position = endPos;
        transform.rotation = endRot;
    }
}
