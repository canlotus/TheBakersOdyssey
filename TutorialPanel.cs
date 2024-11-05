using UnityEngine;
using UnityEngine.UI;

public class TutorialPanel : MonoBehaviour
{
    public GameObject tutorialPanel;       // Öðretici paneli
    public Image[] tutorialImages;         // 5 adet tutorial görseli
    public Button nextButton;              // Ýleri butonu
    public Button finishButton;            // Tamamla butonu

    private int currentImageIndex = 0;     // Þu anda gösterilen görselin indeksi

    void Start()
    {
        // Ýkinci ekranýn giriþ kontrolü
        if (PlayerPrefs.GetInt("FirstTimeScreen2", 1) == 1)
        {
            ShowTutorial();
            PlayerPrefs.SetInt("FirstTimeScreen2", 0); // Bir daha gösterilmemesi için kaydet
            PlayerPrefs.Save();
        }
        else
        {
            tutorialPanel.SetActive(false); // Ýlk giriþ deðilse paneli gizle
        }
    }

    private void ShowTutorial()
    {
        tutorialPanel.SetActive(true);  // Paneli göster
        currentImageIndex = 0;          // Ýlk görselden baþla
        UpdateTutorialImage();          // Ýlk görseli göster

        finishButton.gameObject.SetActive(false);  // Tamamla butonunu gizle
        nextButton.gameObject.SetActive(true);     // Ýleri butonunu göster
    }

    public void OnNextButtonClick()
    {
        currentImageIndex++;   // Sonraki görsele geç
        UpdateTutorialImage();

        // 5. görselde Ýleri butonunu gizleyip Tamamla butonunu göster
        if (currentImageIndex >= tutorialImages.Length - 1)
        {
            nextButton.gameObject.SetActive(false);   // Ýleri butonunu gizle
            finishButton.gameObject.SetActive(true);  // Tamamla butonunu göster
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
            tutorialImages[i].gameObject.SetActive(i == currentImageIndex); // Sadece aktif görseli göster
        }
    }
}
