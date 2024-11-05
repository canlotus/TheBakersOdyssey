using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class JsonLoader : MonoBehaviour
{
    private string fileName = "levels.json"; // Dosya adý gerektiði gibi ayarlanabilir

    // Android ve diðer platformlar için JSON yükleme iþlevi
    public IEnumerator LoadJson(System.Action<string> onJsonLoaded)
    {
        string fullPath = Path.Combine(Application.streamingAssetsPath, fileName);

        if (Application.platform == RuntimePlatform.Android)
        {
            UnityWebRequest request = UnityWebRequest.Get(fullPath);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                onJsonLoaded?.Invoke(request.downloadHandler.text);
            }
            else
            {
                onJsonLoaded?.Invoke(null);
            }
        }
        else
        {
            if (File.Exists(fullPath))
            {
                string jsonContent = File.ReadAllText(fullPath);
                onJsonLoaded?.Invoke(jsonContent);
            }
            else
            {
                onJsonLoaded?.Invoke(null);
            }
        }
    }

    void Start()
    {
        StartCoroutine(LoadJson(jsonContent =>
        {
            if (!string.IsNullOrEmpty(jsonContent))
            {
                // JSON baþarýyla yüklendi
            }
            else
            {
                // JSON yükleme baþarýsýz
            }
        }));
    }
}
