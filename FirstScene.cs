using UnityEngine;
using UnityEngine.UI;

public class FirstScene : MonoBehaviour
{
    public GameObject languageSelectionPanel; // �lk giri�te g�sterilecek dil se�me paneli
    public Button selectTurkishButton;        // Dil se�me panelindeki T�rk�e butonu
    public Button selectEnglishButton;        // Dil se�me panelindeki �ngilizce butonu

    private void Start()
    {
        // �lk ekran�n giri� kontrol�
        if (PlayerPrefs.GetInt("FirstTimeScreen1", 1) == 1)
        {
            ShowLanguageSelectionPanel();
        }
        else
        {
            languageSelectionPanel.SetActive(false); // �lk giri� de�ilse paneli kapal� tut
        }

        // Dil se�im butonlar�n�n i�levlerini ayarla
        selectTurkishButton.onClick.AddListener(() => SelectLanguage("tr"));
        selectEnglishButton.onClick.AddListener(() => SelectLanguage("id"));
    }

    private void ShowLanguageSelectionPanel()
    {
        languageSelectionPanel.SetActive(true); // Dil se�im panelini g�ster
    }

    private void SelectLanguage(string langCode)
    {
        // Dil ayar�n� kaydet
        PlayerPrefs.SetString("GameLanguage", langCode);
        PlayerPrefs.SetInt("FirstTimeScreen1", 0); // �lk ekran giri�i tamamland�
        PlayerPrefs.Save();

        languageSelectionPanel.SetActive(false); // Paneli kapat
    }
}
