using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public TMP_Text countdownText;
    public CarMovement carMovement;
    public float countdownDuration = 1f;

    private bool gameStarted = false;

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

    IEnumerator StartCountdown()
    {
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
