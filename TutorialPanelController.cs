using UnityEngine;
using UnityEngine.UI;

public class TutorialPanelController : MonoBehaviour
{
    public GameObject tutorialPanel;        // Öðretici paneli
    public Image[] tutorialImages;          // 5 adet öðretici görseli
    public Button nextButton;               // Ýleri butonu
    public Button closeButton;              // Kapat butonu
    private int currentImageIndex = 0;      // Þu anda gösterilen görselin indeksi

    private void Start()
    {
        // Baþlangýçta paneli ve "Kapat" butonunu kapalý tut
        tutorialPanel.SetActive(false);
        closeButton.gameObject.SetActive(false);

        // Ýleri butonunun iþlevini ayarla
        nextButton.onClick.AddListener(OnNextButtonClick);

        // Kapat butonunun iþlevini ayarla
        closeButton.onClick.AddListener(OnCloseButtonClick);
    }

    // Öðretici panelini açar ve ilk görseli gösterir
    public void OpenTutorialPanel()
    {
        tutorialPanel.SetActive(true);
        currentImageIndex = 0;
        UpdateTutorialImage();
        closeButton.gameObject.SetActive(false);   // Kapat butonu kapalý
        nextButton.gameObject.SetActive(true);     // Ýleri butonu açýk
    }

    // Ýleri butonuna týklanýnca çalýþacak iþlev
    private void OnNextButtonClick()
    {
        currentImageIndex++;  // Sonraki görsele geç
        UpdateTutorialImage();

        // Son görsele geldiðinde "Ýleri" butonunu kapat, "Kapat" butonunu aç
        if (currentImageIndex >= tutorialImages.Length - 1)
        {
            nextButton.gameObject.SetActive(false);   // Ýleri butonunu kapat
            closeButton.gameObject.SetActive(true);   // Kapat butonunu aç
        }
    }

    // Kapat butonuna týklanýnca çalýþacak iþlev
    private void OnCloseButtonClick()
    {
        tutorialPanel.SetActive(false); // Paneli kapat
    }

    // Aktif olan resmi günceller, diðerlerini gizler
    private void UpdateTutorialImage()
    {
        for (int i = 0; i < tutorialImages.Length; i++)
        {
            tutorialImages[i].gameObject.SetActive(i == currentImageIndex);
        }
    }
}
