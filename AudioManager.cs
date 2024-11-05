using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioClip winSound;           // Kazanma sesi
    public AudioClip gameOverSound;      // Game Over sesi
    public AudioClip mergeSound;         // 3'lü eþleþme sesi
    public AudioClip clickSound;         // Týklama sesi
    public AudioClip coinReachedSound;   // Altýn hedefe ulaþtýðýnda çalan ses
    public AudioClip backgroundMusic;    // Arka plan müziði

    public AudioSource audioSource;
    public AudioSource musicSource;     // Arka plan müziði için ayrý bir AudioSource

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;        // Müzik sürekli çalsýn
        musicSource.volume = 0.05f;      // Yalnýzca background music ses seviyesi düþük
    }

    private void Start()
    {
        PlayBackgroundMusic();
    }

    public void PlayBackgroundMusic()
    {
        if (backgroundMusic != null && !musicSource.isPlaying)
        {
            musicSource.clip = backgroundMusic;
            musicSource.Play();
        }
    }

    public void SetVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void PlayWinSound()
    {
        if (winSound != null)
        {
            audioSource.PlayOneShot(winSound, 0.2f);  // Kazanma sesi 0.2 ses seviyesinde
        }
    }

    public void PlayGameOverSound()
    {
        if (gameOverSound != null)
        {
            audioSource.PlayOneShot(gameOverSound);
        }
    }

    public void PlayMergeSound()
    {
        if (mergeSound != null)
        {
            audioSource.PlayOneShot(mergeSound);
        }
    }

    public void PlayClickSound()
    {
        if (clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }

    public void PlayCoinReachedSound()
    {
        if (coinReachedSound != null)
        {
            audioSource.PlayOneShot(coinReachedSound);
        }
    }
}
