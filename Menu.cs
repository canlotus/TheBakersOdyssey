using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class Menu : MonoBehaviour
{
    public GameObject gameOverPanel;
    public GameObject winPanel;
    public GameObject pausePanel;
    public GameObject countdownPanel;         // Geri say�m paneli
    public TextMeshProUGUI countdownText;     // Geri say�m TMP Text
    public TextMeshProUGUI timerText;
    public float timerDuration = 90f;
    private float currentTime;
    public bool isPaused = false;

    public Image warningImage; // Uyar� resmi (Z rotasyonu animasyonu i�in)
    private bool isWarningActive = false; // Animasyon kontrol�

    void Start()
    {
        gameOverPanel.SetActive(false);
        winPanel.SetActive(false);
        pausePanel.SetActive(false);
        countdownPanel.SetActive(false); // Geri say�m panelini kapal� ba�lat�yoruz
        currentTime = timerDuration;
        Time.timeScale = 1;

        StartCountdown(); // Geri say�m� ba�lat
    }

    void Update()
    {
        if (!isPaused)
        {
            if (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
                UpdateTimerText();
            }
            else
            {
                ShowGameOverPanel();
            }
        }
    }

    void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (currentTime <= 30 && !isWarningActive && warningImage != null)
        {
            isWarningActive = true;
            StartCoroutine(StartWarningAnimation());
        }

        timerText.color = currentTime <= 30 ? Color.red : Color.grey;
    }

    public void ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;

        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayGameOverSound();
        }
    }

    public void ShowWinPanel()
    {
        winPanel.SetActive(true);
        Time.timeScale = 0;

        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayWinSound();
        }
    }

    public void BackToStartScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("StartScene");
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;
        pausePanel.SetActive(true);
    }

    public void ContinueGame()
    {
        isPaused = false;
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }

    public void BackToMenuFromPause()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("StartScene");
    }

    public void StartCountdown()
    {
        StartCoroutine(CountdownCoroutine());
    }

    private IEnumerator CountdownCoroutine()
    {
        countdownPanel.SetActive(true);
        isPaused = true; // Sadece oyun s�resi durdurulacak, spawn i�lemi devam edecek

        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return StartCoroutine(FadeAndScaleText(0.66f)); // Her say� i�in 0.66 saniye
        }

        countdownPanel.SetActive(false);
        isPaused = false; // Geri say�m bitince oyun s�resi devam edecek
    }

    private IEnumerator FadeAndScaleText(float duration)
    {
        Vector3 originalScale = countdownText.transform.localScale;

        // Text'in b�y�y�p k���lme animasyonu
        for (float t = 0; t <= 1f; t += Time.unscaledDeltaTime / (duration / 2))
        {
            countdownText.transform.localScale = Vector3.Lerp(originalScale * 0.5f, originalScale, t);
            countdownText.color = new Color(countdownText.color.r, countdownText.color.g, countdownText.color.b, Mathf.Lerp(0f, 1f, t));
            yield return null;
        }

        for (float t = 0; t <= 1f; t += Time.unscaledDeltaTime / (duration / 2))
        {
            countdownText.transform.localScale = Vector3.Lerp(originalScale, originalScale * 0.5f, t);
            countdownText.color = new Color(countdownText.color.r, countdownText.color.g, countdownText.color.b, Mathf.Lerp(1f, 0f, t));
            yield return null;
        }
    }

    private IEnumerator StartWarningAnimation()
    {
        for (int i = 0; i < 5; i++)
        {
            StartCoroutine(RotateWarningImage());
            yield return new WaitForSeconds(2f);
        }

        isWarningActive = false;
    }

    private IEnumerator RotateWarningImage()
    {
        for (int j = 0; j < 5; j++)
        {
            warningImage.rectTransform.rotation = Quaternion.Euler(0, 0, -20);
            yield return new WaitForSeconds(0.1f);
            warningImage.rectTransform.rotation = Quaternion.Euler(0, 0, 20);
            yield return new WaitForSeconds(0.1f);
        }

        warningImage.rectTransform.rotation = Quaternion.identity;
    }
}
