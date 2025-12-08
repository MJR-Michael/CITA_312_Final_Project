using UnityEngine;
using UnityEngine.AI;
using TMPro; // For TextMeshPro

public class NPCKillerChase : MonoBehaviour
{
    [Header("Chase Settings")]
    public string playerTag = "Player";
    public float turnSpeed = 10f;
    public float angularSpeed = 500f;
    public float acceleration = 10f;

    public MinigameManager manager;

    [Header("Survival Timer")]
    [SerializeField] private float minSurvivalTime = 5f;
    [SerializeField] private float maxSurvivalTime = 10f;

    public string timerTextName = "TimerText"; // Name of your Timer Text GameObject
    private TMP_Text timerText;

    private NavMeshAgent agent;
    private Transform player;
    private float timer;
    private float timeToWin;
    private bool chaseActive = true;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.angularSpeed = angularSpeed;
        agent.acceleration = acceleration;
        agent.stoppingDistance = 0.5f;
        agent.radius = 0.5f;

        GameObject playerObj = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogError("Player not found!");

        // Find the TimerText dynamically
        GameObject timerObj = GameObject.Find(timerTextName);
        if (timerObj != null)
            timerText = timerObj.GetComponent<TMP_Text>();
        else
            Debug.LogWarning("Timer Text not found in scene!");

        // Random survival time between min and max
        timeToWin = Random.Range(minSurvivalTime, maxSurvivalTime);
        timer = 0f;
    }

    void Update()
    {
        if (player == null || !chaseActive) return;

        // Update timer
        timer += Time.deltaTime;
        float timeLeft = Mathf.Max(timeToWin - timer, 0f);

        if (timerText != null)
            timerText.text = $"Time Left: {timeLeft:F1}s";

        if (timer >= timeToWin)
        {
            PlayerWins();
            return;
        }

        // Move towards player
        Vector3 targetPos = player.position;
        targetPos.y = transform.position.y;
        agent.SetDestination(targetPos);

        RotateTowardsPlayer(targetPos);
    }

    void RotateTowardsPlayer(Vector3 targetPos)
    {
        Vector3 direction = targetPos - transform.position;
        direction.y = 0;
        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
    }

    public void PlayerCaught()
    {
        if (!chaseActive) return;
        chaseActive = false;
        agent.isStopped = true;
        if (timerText != null) timerText.text = "GAME OVER!";
        Debug.Log("GAME OVER! Killer caught the player.");
        manager.LoseMinigame();
    }

    void PlayerWins()
    {
        if (!chaseActive) return;
        chaseActive = false;
        agent.isStopped = true;
        if (timerText != null) timerText.text = "PLAYER WINS!";
        Debug.Log("PLAYER WINS! Killer didn't catch the player in time.");
        manager.WinMinigame();
    }
}
