using UnityEngine;
using TMPro;
using System;

public class EnergyManager : MonoBehaviour
{
    public int maxEnergy = 10;        // Maksimum enerji
    public int currentEnergy;         // Mevcut enerji
    public float rechargeTime = 180f; // 3 dakikal�k s�re (180 saniye)
    private float timer;              // Saya� (3 dakika sayacak)

    public TextMeshProUGUI energyText;   // Enerji miktar�n� g�steren text
    public TextMeshProUGUI timerText;    // Saya� texti

    private DateTime lastTimeChecked;    // Saya� i�in en son kontrol zaman�

    void Start()
    {
        LoadEnergyState();
        UpdateEnergyUI();
        UpdateTimerUI();
    }

    void Update()
    {
        // Enerji yenilenme zaman�
        if (currentEnergy < maxEnergy)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                AddEnergy(1);
                timer = rechargeTime;
            }

            UpdateTimerUI();
        }
    }


    public void DecreaseEnergy()
    {
        if (currentEnergy > 0)
        {
            currentEnergy--;  // Bir enerji azalt
            timer = rechargeTime;  // Timer�i s�f�rla, ��nk� bir enerji azald�
            SaveEnergyState();  // G�ncel durumu kaydet
            UpdateEnergyUI();
        }
    }

    void AddEnergy(int amount)
    {
        if (currentEnergy < maxEnergy)
        {
            currentEnergy = Mathf.Min(currentEnergy + amount, maxEnergy);  // Maksimum enerjiye ula��lana kadar ekle
            SaveEnergyState();  // Enerji g�ncellendi�inde kaydedilsin
            UpdateEnergyUI();
        }
    }

    void UpdateEnergyUI()
    {
        energyText.text = currentEnergy.ToString();
    }

    void UpdateTimerUI()
    {
        if (currentEnergy < maxEnergy)
        {
            int minutes = Mathf.FloorToInt(timer / 60);
            int seconds = Mathf.FloorToInt(timer % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else
        {
            timerText.text = "";  // Enerji doluysa saya� bo� kal�r
        }
    }

    void SaveEnergyState()
    {
        PlayerPrefs.SetInt("CurrentEnergy", currentEnergy);  // Enerjiyi g�ncelle
        PlayerPrefs.SetFloat("CurrentTimer", timer);
        PlayerPrefs.SetString("LastTimeChecked", DateTime.Now.ToString());
        PlayerPrefs.Save();

    }

    void LoadEnergyState()
    {
        currentEnergy = PlayerPrefs.GetInt("CurrentEnergy", maxEnergy);
        timer = PlayerPrefs.GetFloat("CurrentTimer", rechargeTime);
        string lastTimeCheckedString = PlayerPrefs.GetString("LastTimeChecked", DateTime.Now.ToString());
        lastTimeChecked = DateTime.Parse(lastTimeCheckedString);

        TimeSpan timeSpan = DateTime.Now - lastTimeChecked;
        float timePassed = (float)timeSpan.TotalSeconds;

        if (currentEnergy < maxEnergy)
        {
            timer -= timePassed;
            while (timer <= 0 && currentEnergy < maxEnergy)
            {
                AddEnergy(1);
                timer += rechargeTime;
            }
        }
    }

    public void AddExtraEnergy()
    {
        AddEnergy(7);
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveEnergyState();
        }
    }

    void OnApplicationQuit()
    {
        SaveEnergyState();
    }
}
