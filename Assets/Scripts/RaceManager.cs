using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Jika pakai TextMeshPro

public class RaceManager : MonoBehaviour
{
    public CarMovement car;
    public GameObject uiBlockInput; // optional: buat matikan input tombol
    public TextMeshProUGUI countdownText; // gunakan Text jika bukan TMP
    public float countdownTime = 3f;

    void Start()
    {
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        // Nonaktifkan kontrol mobil
        car.enabled = false;

        if (uiBlockInput != null) uiBlockInput.SetActive(true);

        float timeLeft = countdownTime;

        while (timeLeft > 0)
        {
            countdownText.text = Mathf.Ceil(timeLeft).ToString();
            yield return new WaitForSeconds(1f);
            timeLeft--;
        }

        countdownText.text = "GO!";
        yield return new WaitForSeconds(1f);
        countdownText.gameObject.SetActive(false);

        // Aktifkan kontrol mobil
        car.enabled = true;

        if (uiBlockInput != null) uiBlockInput.SetActive(false);
    }
}
