using UnityEngine;
using System.Collections;

public class TennisBall : MonoBehaviour
{
    [Header("Serve Settings")]
    public BoxCollider serveArea;    // Drag in your bot's side of the court
    public float serveArcHeight = 3f;
    public float serveSpeed = 6f;

    public System.Action<Vector3> OnLandingPredicted;

    void Start()
    {
        ServeBall();
    }

    // -----------------------------------------------------
    // SERVE FUNCTION — picks a random point in serve area
    // -----------------------------------------------------
    public void ServeBall()
    {
        if (serveArea == null)
        {
            Debug.LogError("Serve area not assigned!");
            return;
        }

        Vector3 target = GetRandomPointInCourt(serveArea);
        StopAllCoroutines();
        StartCoroutine(ServeRoutine(target));
    }

    IEnumerator ServeRoutine(Vector3 targetPoint)
    {
        Vector3 startPos = transform.position;
        float t = 0f;

        OnLandingPredicted?.Invoke(targetPoint);

        while (t < 1f)
        {
            t += Time.deltaTime * serveSpeed;

            Vector3 pos = Vector3.Lerp(startPos, targetPoint, t);
            pos.y += Mathf.Sin(t * Mathf.PI) * serveArcHeight;

            transform.position = pos;

            yield return null;
        }
    }

    // -----------------------------------------------------
    // RETURN FUNCTION — player hits ball to opponent’s court
    // -----------------------------------------------------
    public void ReturnToTarget(Vector3 targetPoint, float speed = 12f, float arcHeight = 2f)
    {
        StopAllCoroutines();
        StartCoroutine(ReturnRoutine(targetPoint, speed, arcHeight));
    }

    IEnumerator ReturnRoutine(Vector3 targetPoint, float speed, float arcHeight)
    {
        Vector3 start = transform.position;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * speed;

            Vector3 pos = Vector3.Lerp(start, targetPoint, t);
            pos.y += Mathf.Sin(t * Mathf.PI) * arcHeight;

            transform.position = pos;
            yield return null;
        }

        // After landing → bot serves again
        ServeBall();
    }

    // -----------------------------------------------------
    // Utility — pick random point inside BoxCollider
    // -----------------------------------------------------
    Vector3 GetRandomPointInCourt(BoxCollider box)
    {
        Vector3 center = box.transform.TransformPoint(box.center);

        Vector3 size = box.size;
        Vector3 worldSize = Vector3.Scale(size, box.transform.lossyScale);

        float x = Random.Range(center.x - worldSize.x / 2f, center.x + worldSize.x / 2f);
        float z = Random.Range(center.z - worldSize.z / 2f, center.z + worldSize.z / 2f);

        return new Vector3(x, box.transform.position.y, z);
    }
}
