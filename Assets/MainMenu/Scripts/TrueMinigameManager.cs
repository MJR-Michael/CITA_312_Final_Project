using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MinigameManager : MonoBehaviour
{
    [Header("Minigame Setup")]
    public List<string> minigameNames;
    public List<string> minigameSceneNames;

    private List<int> minigameQueue = new List<int>();

    [Header("UI Elements")]
    public TMP_Text currentGameText;
    public TMP_Text nextGameText;
    public TMP_Text scoreText;
    public List<GameObject> hearts;

    [Header("Game Stats")]
    public int heartsCount = 3;
    private int trophies = 0;

    [Header("Scene Names")]
    public string trueGameOverSceneName = "GameOverScene";

    private int currentMinigameIndex = -1;


    void Start()
    {
        StartNewQueue();
        ShowNextMinigame();
        UpdateHeartsUI();
        UpdateScoreUI();
        //LoadCurrentMinigameScene();
    }

    // Creates a new shuffled queue of all minigames
    void StartNewQueue()
    {
        minigameQueue.Clear();

        for (int i = 0; i < minigameNames.Count; i++)
            minigameQueue.Add(i);

        // Shuffle
        for (int i = minigameQueue.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (minigameQueue[i], minigameQueue[j]) = (minigameQueue[j], minigameQueue[i]);
        }
    }

    // Picks the next minigame from the queue. If empty → regenerate but keep score/hearts.
    void ShowNextMinigame()
    {
        // If we have played all minigames once, start a new shuffled batch
        if (minigameQueue.Count == 0)
            StartNewQueue();

        currentMinigameIndex = minigameQueue[0];
        minigameQueue.RemoveAt(0);

        // Update UI
        currentGameText.text = "Current Game: " + minigameNames[currentMinigameIndex];

        if (minigameQueue.Count > 0)
            nextGameText.text = "Next Game: " + minigameNames[minigameQueue[0]];
        else
            nextGameText.text = "Next Game: ???";
    }

    public void LoadCurrentMinigameScene()
    {
        string sceneToLoad = minigameSceneNames[currentMinigameIndex];

        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogError("Invalid scene name at index: " + currentMinigameIndex);
        }
    }

    public void WinMinigame()
    {
        trophies++;
        UpdateScoreUI();

        ShowNextMinigame();
        LoadCurrentMinigameScene();
    }

    public void LoseMinigame()
    {
        heartsCount--;
        UpdateHeartsUI();

        if (heartsCount <= 0)
        {
            LoadTrueGameOver();
            return;
        }

        ShowNextMinigame();
        LoadCurrentMinigameScene();
    }


    void UpdateHeartsUI()
    {
        for (int i = 0; i < hearts.Count; i++)
            hearts[i].SetActive(i < heartsCount);
    }

    void UpdateScoreUI()
    {
        scoreText.text = "Trophies: " + trophies;
    }

    void LoadTrueGameOver()
    {
        SceneManager.LoadScene(trueGameOverSceneName);
    }
}
