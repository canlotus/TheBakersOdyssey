using UnityEngine;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;

public class SpawnPoint : MonoBehaviour
{
    public GameObject[] objectPrefabs;   // Oyundaki t�m prefablar
    private LevelData[] levels;  // JSON'dan gelen t�m seviyeler
    private List<GameObject> spawnedObjects = new List<GameObject>(); // Spawn edilen objelerin listesi
    private SlotManager slotManager;

    void Start()
    {
        slotManager = FindObjectOfType<SlotManager>();

        int savedLevel = PlayerPrefs.GetInt("CurrentLevel", 1);

        // JsonLoader'� kullanarak seviyeleri y�kl�yoruz
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

                    // Kaydedilen seviyeye g�re objeleri spawnlamadan �nce eskilerini temizliyoruz
                    ClearSpawnedObjects();

                    // Objeleri kar��t�rarak spawn etmeye ba�la
                    StartCoroutine(SpawnObjectsInMixedOrder(levels[savedLevel - 1]));
                }
            }));
        }
    }

    // Daha �nce spawn edilen objeleri temizler
    void ClearSpawnedObjects()
    {
        foreach (GameObject obj in spawnedObjects)
        {
            Destroy(obj);
        }
        spawnedObjects.Clear(); // Listeyi de temizler
    }

    // Objeleri rastgele bir s�rayla spawn ederken kazanma ko�ulunu ba�lat�r
    IEnumerator SpawnObjectsInMixedOrder(LevelData levelData)
    {
        bool winConditionInitialized = false;

        // T�m objeleri birer birer spawn edece�imiz bir liste olu�turuyoruz
        List<GameObject> allObjectsToSpawn = new List<GameObject>();

        // Her spawn noktas� i�in t�m objeleri listeye ekliyoruz
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

        // T�m objeler tek listede oldu�u i�in bu listeyi kar��t�ral�m
        ShuffleList(allObjectsToSpawn);

        // Kar���k listede objeleri 5'erli olarak spawn edelim
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

                // �lk obje spawn edildi�inde kazanma ko�ulunu ba�lat�yoruz
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

    // Listeyi kar��t�rmak i�in bir yard�mc� fonksiyon (GameObject listesi i�in)
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
