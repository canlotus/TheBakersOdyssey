using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class StartScene : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    public GameObject rewardPanel;            // Panel referansý
    public Button claimButton;                // Claim butonu
    public Button adClaimButton;              // 20 altýn veren buton
    public GameObject coinPrefab;             // Altýn görseli prefabý
    public Transform targetPosition;          // Altýnlarýn gideceði hedef konum
    public TextMeshProUGUI goldText;          // Altýn miktarýný gösteren text
    private int currentLevel;
    private int previousLevel;
    private int goldAmount;

    void Start()
    {
        currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
        previousLevel = PlayerPrefs.GetInt("PreviousLevel", 1);
        goldAmount = PlayerPrefs.GetInt("GoldAmount", 0);
        goldText.text = goldAmount.ToString();
        levelText.text = "Level: " + currentLevel;

        if (currentLevel > previousLevel)
        {
            rewardPanel.SetActive(true);
        }
        else
        {
            rewardPanel.SetActive(false);
        }
    }

    // 10 altýn ekleyen buton iþlevi
    public void OnClaimButtonClicked()
    {
        DisableButtons(); // Butonlarý devre dýþý býrak
        StartCoroutine(AnimateCoins(10));
        PlayerPrefs.SetInt("PreviousLevel", currentLevel);  // Yeni level kaydedilir
    }

    // 20 altýn ekleyen buton iþlevi (reklam izlenmesi karþýlýðý)
    public void OnAdClaimButtonClicked()
    {
        DisableButtons(); // Butonlarý devre dýþý býrak
        StartCoroutine(AnimateCoins(20));
        PlayerPrefs.SetInt("PreviousLevel", currentLevel);  // Yeni level kaydedilir
    }

    // Altýn animasyonlarýný baþlatýr ve miktarý belirtir
    private IEnumerator AnimateCoins(int coinsToAdd)
    {
        List<GameObject> coinInstances = new List<GameObject>();

        for (int i = 0; i < coinsToAdd; i++)
        {
            GameObject coin = Instantiate(coinPrefab, claimButton.transform.position, Quaternion.identity, rewardPanel.transform);
            coinInstances.Add(coin);
            yield return new WaitForSeconds(0.1f);  // Her bir altýn arasý gecikme
        }

        foreach (GameObject coin in coinInstances)
        {
            StartCoroutine(MoveCoinToTarget(coin, coinsToAdd));
            yield return new WaitForSeconds(0.2f);  // Her bir altýn hedefe sýrayla ulaþýr
        }
    }

    // Her bir altýný hedef konuma taþýyan fonksiyon
    private IEnumerator MoveCoinToTarget(GameObject coin, int coinsToAdd)
    {
        while (Vector3.Distance(coin.transform.position, targetPosition.position) > 0.1f)
        {
            coin.transform.position = Vector3.MoveTowards(coin.transform.position, targetPosition.position, 1500 * Time.deltaTime);
            yield return null;
        }

        Destroy(coin);
        goldAmount++;
        goldText.text = goldAmount.ToString();
        PlayerPrefs.SetInt("GoldAmount", goldAmount);

        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayCoinReachedSound();
        }

        // Altýnlarýn tamamý eklendiyse panel kapanýr  
        if (goldAmount % coinsToAdd == 0)
        {
            rewardPanel.SetActive(false);
            EnableButtons();  // Panel kapandýðýnda butonlarý yeniden etkinleþtir
        }
    }

    // Butonlarý devre dýþý býrakýr
    private void DisableButtons()
    {
        claimButton.interactable = false;
        adClaimButton.interactable = false;
    }

    // Butonlarý etkinleþtirir
    private void EnableButtons()
    {
        claimButton.interactable = true;
        adClaimButton.interactable = true;
    }
}
