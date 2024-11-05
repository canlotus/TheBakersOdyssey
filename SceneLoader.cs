using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public GameObject outOfEnergyPanel;  // Enerji bittiðinde açýlacak panel
    private EnergyManager energyManager;

    void Start()
    {
        energyManager = FindObjectOfType<EnergyManager>();
        outOfEnergyPanel.SetActive(false);  // Panel baþta kapalý
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
            Debug.Log("Yetersiz Enerji, Panel Açýldý"); // Enerji yeterli deðilse panel açýlýyor
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
