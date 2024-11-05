using UnityEngine;
using TMPro;
using System;

public class EnergyManager : MonoBehaviour
{
    public int maxEnergy = 10;        // Maksimum enerji
    public int currentEnergy;         // Mevcut enerji
    public float rechargeTime = 180f; // 3 dakikalýk süre (180 saniye)
    private float timer;              // Sayaç (3 dakika sayacak)

    public TextMeshProUGUI energyText;   // Enerji miktarýný gösteren text
    public TextMeshProUGUI timerText;    // Sayaç texti

    private DateTime lastTimeChecked;    // Sayaç için en son kontrol zamaný

    void Start()
    {
        LoadEnergyState();
        UpdateEnergyUI();
        UpdateTimerUI();
    }

    void Update()
    {
        // Enerji yenilenme zamaný
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
            timer = rechargeTime;  // Timer’i sýfýrla, çünkü bir enerji azaldý
            SaveEnergyState();  // Güncel durumu kaydet
            UpdateEnergyUI();
        }
    }

    void AddEnergy(int amount)
    {
        if (currentEnergy < maxEnergy)
        {
            currentEnergy = Mathf.Min(currentEnergy + amount, maxEnergy);  // Maksimum enerjiye ulaþýlana kadar ekle
            SaveEnergyState();  // Enerji güncellendiðinde kaydedilsin
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
            timerText.text = "";  // Enerji doluysa sayaç boþ kalýr
        }
    }

    void SaveEnergyState()
    {
        PlayerPrefs.SetInt("CurrentEnergy", currentEnergy);  // Enerjiyi güncelle
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
