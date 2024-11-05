using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    public GameObject settingsPanel;
    public Button musicOnButton;
    public Button musicOffButton;
    public Button sfxOnButton;
    public Button sfxOffButton;
    public Button closeButton;

    private AudioManager audioManager;

    private void Start()
    {
        audioManager = AudioManager.instance;

        // Ýlk ayarlarý kontrol et ve buton iþlevlerini ayarla
        InitializeSettings();
        musicOnButton.onClick.AddListener(EnableMusic);
        musicOffButton.onClick.AddListener(DisableMusic);
        sfxOnButton.onClick.AddListener(EnableSFX);
        sfxOffButton.onClick.AddListener(DisableSFX);
        closeButton.onClick.AddListener(ClosePanel);
    }

    private void InitializeSettings()
    {
        EnableMusic();
        EnableSFX();
    }

    public void EnableMusic()
    {
        if (audioManager != null)
        {
            audioManager.musicSource.mute = false;
            musicOnButton.interactable = false;
            musicOffButton.interactable = true;
        }
    }

    public void DisableMusic()
    {
        if (audioManager != null)
        {
            audioManager.musicSource.mute = true;
            musicOnButton.interactable = true;
            musicOffButton.interactable = false;
        }
    }

    public void EnableSFX()
    {
        if (audioManager != null)
        {
            audioManager.audioSource.mute = false;
            sfxOnButton.interactable = false;
            sfxOffButton.interactable = true;
        }
    }

    public void DisableSFX()
    {
        if (audioManager != null)
        {
            audioManager.audioSource.mute = true;
            sfxOnButton.interactable = true;
            sfxOffButton.interactable = false;
        }
    }

    public void OpenPanel()
    {
        settingsPanel.SetActive(true);
    }

    public void ClosePanel()
    {
        settingsPanel.SetActive(false);
    }
}
