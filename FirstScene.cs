using UnityEngine;
using UnityEngine.UI;

public class FirstScene : MonoBehaviour
{
    public GameObject languageSelectionPanel; // Ýlk giriþte gösterilecek dil seçme paneli
    public Button selectTurkishButton;        // Dil seçme panelindeki Türkçe butonu
    public Button selectEnglishButton;        // Dil seçme panelindeki Ýngilizce butonu

    private void Start()
    {
        // Ýlk ekranýn giriþ kontrolü
        if (PlayerPrefs.GetInt("FirstTimeScreen1", 1) == 1)
        {
            ShowLanguageSelectionPanel();
        }
        else
        {
            languageSelectionPanel.SetActive(false); // Ýlk giriþ deðilse paneli kapalý tut
        }

        // Dil seçim butonlarýnýn iþlevlerini ayarla
        selectTurkishButton.onClick.AddListener(() => SelectLanguage("tr"));
        selectEnglishButton.onClick.AddListener(() => SelectLanguage("id"));
    }

    private void ShowLanguageSelectionPanel()
    {
        languageSelectionPanel.SetActive(true); // Dil seçim panelini göster
    }

    private void SelectLanguage(string langCode)
    {
        // Dil ayarýný kaydet
        PlayerPrefs.SetString("GameLanguage", langCode);
        PlayerPrefs.SetInt("FirstTimeScreen1", 0); // Ýlk ekran giriþi tamamlandý
        PlayerPrefs.Save();

        languageSelectionPanel.SetActive(false); // Paneli kapat
    }
}
