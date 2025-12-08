using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetManager : MonoBehaviour
{
    [Header("Targets")]
    public List<GameObject> allTargets;
    public int minActiveTargets = 8;
    public int maxActiveTargets = 12;
    public MinigameManager manager;
    private int activeTargetCount;
    private int hitTargetCount;
    private bool roundEnded = false;

    [Header("Timing Settings")]
    public float startDelay = 3f;   // 3 second delay before the game starts
    public float roundTime = 4f;    // Time allowed to hit all targets

    void Start()
    {
        // Hide all targets immediately at game start
        foreach (var target in allTargets)
            target.SetActive(false);

        StartCoroutine(GameFlow());
    }

    private IEnumerator GameFlow()
    {
        Debug.Log("Game starting in 3 seconds...");

        yield return new WaitForSeconds(startDelay);

        Debug.Log("GO! Targets activated.");

        ActivateRandomTargets();

        StartCoroutine(RoundTimer());
    }

    void ActivateRandomTargets()
    {
        activeTargetCount = 0;
        hitTargetCount = 0;

        minActiveTargets = Mathf.Clamp(minActiveTargets, 0, allTargets.Count);
        maxActiveTargets = Mathf.Clamp(maxActiveTargets, minActiveTargets, allTargets.Count);

        int targetsToActivate = Random.Range(minActiveTargets, maxActiveTargets + 1);

        // Shuffle list
        List<GameObject> shuffled = new List<GameObject>(allTargets);
        for (int i = 0; i < shuffled.Count; i++)
        {
            int rand = Random.Range(i, shuffled.Count);
            (shuffled[i], shuffled[rand]) = (shuffled[rand], shuffled[i]);
        }

        // Activate targets
        for (int i = 0; i < shuffled.Count; i++)
        {
            bool activate = i < targetsToActivate;
            shuffled[i].SetActive(activate);

            if (activate)
            {
                activeTargetCount++;

                TargetController controller = shuffled[i].GetComponent<TargetController>();
                controller.AssignManager(this);
            }
        }
    }

    public void TargetHit()
    {
        if (roundEnded) return;

        hitTargetCount++;

        if (hitTargetCount >= activeTargetCount)
        {
            Win();
        }
    }

    private IEnumerator RoundTimer()
    {
        yield return new WaitForSeconds(roundTime);

        if (!roundEnded)
            Lose();
    }

    private void Win()
    {
        roundEnded = true;
        Debug.Log("YOU WIN! All targets hit in time.");
        manager.WinMinigame();

        
    }

    private void Lose()
    {
        roundEnded = true;
        Debug.Log("YOU LOSE! Not all targets were hit in time.");
        manager.LoseMinigame();

    }
}
