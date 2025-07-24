using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Object")]
    public CarMovement carMovement;
    public GameObject finishScreen;


    [Header("UI & Control")]
    public TMP_Text countdownText;
    public TMP_Text gameCounterText;
    private float countdownDuration = 1f;
    private float gameTimeRemaining = 10f;
    private bool isGameTimerRunning = false;


    private bool gameStarted = false;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip startMusic;
    public AudioClip backgroundMusic;


    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        Time.timeScale = 1f;
    }

    void Start()
    {
        if (carMovement != null)
        {
            carMovement.enabled = false;
        }

        StartCoroutine(StartCountdown());
    }

    void Update()
    {
        if (isGameTimerRunning)
        {
            gameTimeRemaining -= Time.deltaTime;

            if (gameTimeRemaining <= 0f)
            {
                gameTimeRemaining = 0f;
                isGameTimerRunning = false;
                gameCounterText.text = "00:00";

                // TODO: Panggil GameOver atau logic selesai game
                carMovement.enabled = false;
                MuteAllEngineSounds();

                if (finishScreen != null)
                {
                    finishScreen.SetActive(true);
                }
                Time.timeScale = 0f;
            }
            else
            {
                int minutes = Mathf.FloorToInt(gameTimeRemaining / 60f);
                int seconds = Mathf.FloorToInt(gameTimeRemaining % 60f);
                gameCounterText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            }
        }
    }

    IEnumerator StartCountdown()
    {
        if (audioSource != null && startMusic != null)
        {
            audioSource.clip = startMusic;
            audioSource.loop = false;
            audioSource.Play();
        }

        countdownText.gameObject.SetActive(true);

        for (int i = 3; i > 0; i--)
        {
            MuteAllEngineSounds();
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(countdownDuration);
        }

        countdownText.text = "GO!";
        yield return new WaitForSeconds(0.5f);
        countdownText.gameObject.SetActive(false);

        isGameTimerRunning = true;
        gameCounterText.gameObject.SetActive(true);

        if (audioSource != null && backgroundMusic != null)
        {
            audioSource.clip = backgroundMusic;
            audioSource.loop = true;
            audioSource.Play();
        }

        gameStarted = true;
        if (carMovement != null)
        {
            carMovement.enabled = true;
        }
    }

    public bool IsGameStarted()
    {
        return gameStarted;
    }

    public void MuteAllEngineSounds()
    {
        if (carMovement != null)
        {
            carMovement.MuteAllEngineSounds();
        }
    }
}
