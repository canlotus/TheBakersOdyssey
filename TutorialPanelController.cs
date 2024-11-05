using UnityEngine;
using UnityEngine.UI;

public class TutorialPanelController : MonoBehaviour
{
    public GameObject tutorialPanel;        // ��retici paneli
    public Image[] tutorialImages;          // 5 adet ��retici g�rseli
    public Button nextButton;               // �leri butonu
    public Button closeButton;              // Kapat butonu
    private int currentImageIndex = 0;      // �u anda g�sterilen g�rselin indeksi

    private void Start()
    {
        // Ba�lang��ta paneli ve "Kapat" butonunu kapal� tut
        tutorialPanel.SetActive(false);
        closeButton.gameObject.SetActive(false);

        // �leri butonunun i�levini ayarla
        nextButton.onClick.AddListener(OnNextButtonClick);

        // Kapat butonunun i�levini ayarla
        closeButton.onClick.AddListener(OnCloseButtonClick);
    }

    // ��retici panelini a�ar ve ilk g�rseli g�sterir
    public void OpenTutorialPanel()
    {
        tutorialPanel.SetActive(true);
        currentImageIndex = 0;
        UpdateTutorialImage();
        closeButton.gameObject.SetActive(false);   // Kapat butonu kapal�
        nextButton.gameObject.SetActive(true);     // �leri butonu a��k
    }

    // �leri butonuna t�klan�nca �al��acak i�lev
    private void OnNextButtonClick()
    {
        currentImageIndex++;  // Sonraki g�rsele ge�
        UpdateTutorialImage();

        // Son g�rsele geldi�inde "�leri" butonunu kapat, "Kapat" butonunu a�
        if (currentImageIndex >= tutorialImages.Length - 1)
        {
            nextButton.gameObject.SetActive(false);   // �leri butonunu kapat
            closeButton.gameObject.SetActive(true);   // Kapat butonunu a�
        }
    }

    // Kapat butonuna t�klan�nca �al��acak i�lev
    private void OnCloseButtonClick()
    {
        tutorialPanel.SetActive(false); // Paneli kapat
    }

    // Aktif olan resmi g�nceller, di�erlerini gizler
    private void UpdateTutorialImage()
    {
        for (int i = 0; i < tutorialImages.Length; i++)
        {
            tutorialImages[i].gameObject.SetActive(i == currentImageIndex);
        }
    }
}
