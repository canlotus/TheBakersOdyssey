using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class StartScene : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    public GameObject rewardPanel;            // Panel referans�
    public Button claimButton;                // Claim butonu
    public Button adClaimButton;              // 20 alt�n veren buton
    public GameObject coinPrefab;             // Alt�n g�rseli prefab�
    public Transform targetPosition;          // Alt�nlar�n gidece�i hedef konum
    public TextMeshProUGUI goldText;          // Alt�n miktar�n� g�steren text
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

    // 10 alt�n ekleyen buton i�levi
    public void OnClaimButtonClicked()
    {
        DisableButtons(); // Butonlar� devre d��� b�rak
        StartCoroutine(AnimateCoins(10));
        PlayerPrefs.SetInt("PreviousLevel", currentLevel);  // Yeni level kaydedilir
    }

    // 20 alt�n ekleyen buton i�levi (reklam izlenmesi kar��l���)
    public void OnAdClaimButtonClicked()
    {
        DisableButtons(); // Butonlar� devre d��� b�rak
        StartCoroutine(AnimateCoins(20));
        PlayerPrefs.SetInt("PreviousLevel", currentLevel);  // Yeni level kaydedilir
    }

    // Alt�n animasyonlar�n� ba�lat�r ve miktar� belirtir
    private IEnumerator AnimateCoins(int coinsToAdd)
    {
        List<GameObject> coinInstances = new List<GameObject>();

        for (int i = 0; i < coinsToAdd; i++)
        {
            GameObject coin = Instantiate(coinPrefab, claimButton.transform.position, Quaternion.identity, rewardPanel.transform);
            coinInstances.Add(coin);
            yield return new WaitForSeconds(0.1f);  // Her bir alt�n aras� gecikme
        }

        foreach (GameObject coin in coinInstances)
        {
            StartCoroutine(MoveCoinToTarget(coin, coinsToAdd));
            yield return new WaitForSeconds(0.2f);  // Her bir alt�n hedefe s�rayla ula��r
        }
    }

    // Her bir alt�n� hedef konuma ta��yan fonksiyon
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

        // Alt�nlar�n tamam� eklendiyse panel kapan�r  
        if (goldAmount % coinsToAdd == 0)
        {
            rewardPanel.SetActive(false);
            EnableButtons();  // Panel kapand���nda butonlar� yeniden etkinle�tir
        }
    }

    // Butonlar� devre d��� b�rak�r
    private void DisableButtons()
    {
        claimButton.interactable = false;
        adClaimButton.interactable = false;
    }

    // Butonlar� etkinle�tirir
    private void EnableButtons()
    {
        claimButton.interactable = true;
        adClaimButton.interactable = true;
    }
}
