using UnityEngine;
using System;

public class ClockManager : MonoBehaviour
{
    [Header("Clock Hands")]
    public Transform hourHand;
    public Transform minuteHand;
    public Transform secondHand;

    [Header("Chime Settings")]
    public AudioSource audioSource;
    public AudioClip chimeSound;

    [Header("Random Start Time (Seconds Before 12:00)")]
    public int minSecondsBeforeNoon = 10;   // Editable in inspector
    public int maxSecondsBeforeNoon = 30;   // Editable in inspector

    private DateTime currentTime;
    private bool hasRung = false;

    public event Action OnClockStrike12;

    void Start()
    {
        int randomOffset = UnityEngine.Random.Range(minSecondsBeforeNoon, maxSecondsBeforeNoon + 1);
        currentTime = DateTime.Today.AddHours(12).AddSeconds(-randomOffset);

        SetClockHands(currentTime);
    }

    void Update()
    {
        currentTime = currentTime.AddSeconds(Time.deltaTime);
        SetClockHands(currentTime);

        CheckForNoon();
    }

    void SetClockHands(DateTime time)
    {
        float hours = time.Hour % 12 + time.Minute / 60f + time.Second / 3600f;
        float minutes = time.Minute + time.Second / 60f;
        float seconds = time.Second;

        hourHand.localEulerAngles = new Vector3(0, 0, -hours * 30f);
        minuteHand.localEulerAngles = new Vector3(0, 0, -minutes * 6f);
        secondHand.localEulerAngles = new Vector3(0, 0, -seconds * 6f);
    }

    void CheckForNoon()
    {
        if (!hasRung && currentTime.Hour == 12 && currentTime.Minute == 0 && currentTime.Second >= 0)
        {
            audioSource.PlayOneShot(chimeSound);
            hasRung = true;

            OnClockStrike12?.Invoke();
        }
    }
}
