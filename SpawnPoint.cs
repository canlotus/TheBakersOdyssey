using UnityEngine;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;

public class SpawnPoint : MonoBehaviour
{
    public GameObject[] objectPrefabs;   // Oyundaki tüm prefablar
    private LevelData[] levels;  // JSON'dan gelen tüm seviyeler
    private List<GameObject> spawnedObjects = new List<GameObject>(); // Spawn edilen objelerin listesi
    private SlotManager slotManager;

    void Start()
    {
        slotManager = FindObjectOfType<SlotManager>();

        int savedLevel = PlayerPrefs.GetInt("CurrentLevel", 1);

        // JsonLoader'ý kullanarak seviyeleri yüklüyoruz
        JsonLoader jsonLoader = FindObjectOfType<JsonLoader>();
        if (jsonLoader != null)
        {
            StartCoroutine(jsonLoader.LoadJson(jsonContent =>
            {
                if (!string.IsNullOrEmpty(jsonContent))
                {
                    levels = JsonConvert.DeserializeObject<LevelData[]>(jsonContent);

                    if (levels == null || savedLevel - 1 >= levels.Length)
                    {
                        return;
                    }

                    // Kaydedilen seviyeye göre objeleri spawnlamadan önce eskilerini temizliyoruz
                    ClearSpawnedObjects();

                    // Objeleri karýþtýrarak spawn etmeye baþla
                    StartCoroutine(SpawnObjectsInMixedOrder(levels[savedLevel - 1]));
                }
            }));
        }
    }

    // Daha önce spawn edilen objeleri temizler
    void ClearSpawnedObjects()
    {
        foreach (GameObject obj in spawnedObjects)
        {
            Destroy(obj);
        }
        spawnedObjects.Clear(); // Listeyi de temizler
    }

    // Objeleri rastgele bir sýrayla spawn ederken kazanma koþulunu baþlatýr
    IEnumerator SpawnObjectsInMixedOrder(LevelData levelData)
    {
        bool winConditionInitialized = false;

        // Tüm objeleri birer birer spawn edeceðimiz bir liste oluþturuyoruz
        List<GameObject> allObjectsToSpawn = new List<GameObject>();

        // Her spawn noktasý için tüm objeleri listeye ekliyoruz
        foreach (var spawnPoint in levelData.spawnPoints)
        {
            GameObject prefab = GetPrefabByName(spawnPoint.prefabName);
            if (prefab != null)
            {
                for (int i = 0; i < spawnPoint.quantity; i++)
                {
                    allObjectsToSpawn.Add(prefab);
                }
            }
        }

        // Tüm objeler tek listede olduðu için bu listeyi karýþtýralým
        ShuffleList(allObjectsToSpawn);

        // Karýþýk listede objeleri 5'erli olarak spawn edelim
        for (int i = 0; i < allObjectsToSpawn.Count; i += 5)
        {
            for (int j = 0; j < 5 && i + j < allObjectsToSpawn.Count; j++)
            {
                GameObject prefab = allObjectsToSpawn[i + j];

                float randomX = Random.Range(-1.3f, 1.3f);
                float randomY = Random.Range(0f, -1.80f);
                float randomZ = Random.Range(-0.75f, 0.75f);
                Quaternion randomRotation = Random.rotation;

                Vector3 spawnPosition = new Vector3(randomX, randomY, randomZ);
                GameObject spawnedObject = Instantiate(prefab, spawnPosition, randomRotation);
                spawnedObject.name = prefab.name;

                // Spawn edilen objeyi listeye ekleyelim
                spawnedObjects.Add(spawnedObject);

                // Ýlk obje spawn edildiðinde kazanma koþulunu baþlatýyoruz
                if (!winConditionInitialized)
                {
                    slotManager.InitializeWinCondition(levelData.winCondition);
                    winConditionInitialized = true;
                }
            }

            // 5 obje spawn ettikten sonra bir frame bekleyin
            yield return null;
        }
    }

    // Listeyi karýþtýrmak için bir yardýmcý fonksiyon (GameObject listesi için)
    void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    GameObject GetPrefabByName(string prefabName)
    {
        foreach (GameObject prefab in objectPrefabs)
        {
            if (prefab.name == prefabName)
            {
                return prefab;
            }
        }
        return null;
    }

    public void SaveCurrentLevel(int levelNumber)
    {
        PlayerPrefs.SetInt("CurrentLevel", levelNumber);
        PlayerPrefs.Save();
    }
}
