using UnityEngine;
using UnityEngine.UI;

public class TutorialPanel : MonoBehaviour
{
    public GameObject tutorialPanel;       // ��retici paneli
    public Image[] tutorialImages;         // 5 adet tutorial g�rseli
    public Button nextButton;              // �leri butonu
    public Button finishButton;            // Tamamla butonu

    private int currentImageIndex = 0;     // �u anda g�sterilen g�rselin indeksi

    void Start()
    {
        // �kinci ekran�n giri� kontrol�
        if (PlayerPrefs.GetInt("FirstTimeScreen2", 1) == 1)
        {
            ShowTutorial();
            PlayerPrefs.SetInt("FirstTimeScreen2", 0); // Bir daha g�sterilmemesi i�in kaydet
            PlayerPrefs.Save();
        }
        else
        {
            tutorialPanel.SetActive(false); // �lk giri� de�ilse paneli gizle
        }
    }

    private void ShowTutorial()
    {
        tutorialPanel.SetActive(true);  // Paneli g�ster
        currentImageIndex = 0;          // �lk g�rselden ba�la
        UpdateTutorialImage();          // �lk g�rseli g�ster

        finishButton.gameObject.SetActive(false);  // Tamamla butonunu gizle
        nextButton.gameObject.SetActive(true);     // �leri butonunu g�ster
    }

    public void OnNextButtonClick()
    {
        currentImageIndex++;   // Sonraki g�rsele ge�
        UpdateTutorialImage();

        // 5. g�rselde �leri butonunu gizleyip Tamamla butonunu g�ster
        if (currentImageIndex >= tutorialImages.Length - 1)
        {
            nextButton.gameObject.SetActive(false);   // �leri butonunu gizle
            finishButton.gameObject.SetActive(true);  // Tamamla butonunu g�ster
        }
    }

    public void OnFinishButtonClick()
    {
        tutorialPanel.SetActive(false); // Paneli kapat
    }

    private void UpdateTutorialImage()
    {
        for (int i = 0; i < tutorialImages.Length; i++)
        {
            tutorialImages[i].gameObject.SetActive(i == currentImageIndex); // Sadece aktif g�rseli g�ster
        }
    }
}
