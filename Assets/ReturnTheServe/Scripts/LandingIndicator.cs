using UnityEngine;

public class LandingIndicator : MonoBehaviour
{
    public TennisBall ball;
    public float yOffset = 0.05f;
    public float fadeSpeed = 5f;

    private Renderer rend;
    private Color indicatorColor;
    private bool visible = false;

    void Start()
    {
        rend = GetComponent<Renderer>();
        indicatorColor = rend.material.color;

        // Start invisible
        indicatorColor.a = 0f;
        rend.material.color = indicatorColor;

        if (ball != null)
        {
            ball.OnLandingPredicted += MoveIndicator;
        }
    }

    void MoveIndicator(Vector3 landingPosition)
    {
        transform.position = new Vector3(
            landingPosition.x,
            transform.position.y + yOffset,
            landingPosition.z
        );

        visible = true;
    }

    void Update()
    {
        float targetAlpha = visible ? 1f : 0f;

        indicatorColor.a = Mathf.Lerp(indicatorColor.a, targetAlpha, Time.deltaTime * fadeSpeed);
        rend.material.color = indicatorColor;
    }

    public void Hide()
    {
        visible = false;
    }
}
