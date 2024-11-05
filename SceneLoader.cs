using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public GameObject outOfEnergyPanel;  // Enerji bitti�inde a��lacak panel
    private EnergyManager energyManager;

    void Start()
    {
        energyManager = FindObjectOfType<EnergyManager>();
        outOfEnergyPanel.SetActive(false);  // Panel ba�ta kapal�
    }

    public void LoadGameScene()
    {
        if (energyManager != null && energyManager.currentEnergy > 0)
        {
            energyManager.DecreaseEnergy();
            SceneManager.LoadScene("GameScene");
        }
        else
        {
            outOfEnergyPanel.SetActive(true);
            Debug.Log("Yetersiz Enerji, Panel A��ld�"); // Enerji yeterli de�ilse panel a��l�yor
        }
    }

    public void CloseEnergyPanel()
    {
        outOfEnergyPanel.SetActive(false);
    }

    public void AddEnergyFromAd()
    {
        energyManager.AddExtraEnergy();
        outOfEnergyPanel.SetActive(false);
    }
}
