using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class TableTextManager : MonoBehaviour
{
    public TextMeshProUGUI tableText;         // Tabeladaki TMP metni
    public TMP_InputField inputField;         // Kullanýcýnýn metin gireceði TMP alaný
    public Button confirmButton;              // Onay butonu
    private int maxCharacterLimit = 20;       // Maksimum karakter sayýsý
    private bool isTextSet = false;           // Ýsmin tabloya kaydedildiðini takip eder
    private TextMeshProUGUI placeholderText;  // Placeholder metni

    private string filePath;                  // JSON dosyasýnýn yolu

    private void Start()
    {
        filePath = Application.persistentDataPath + "/userdata.json"; // JSON dosya yolu belirleme
        inputField.characterLimit = maxCharacterLimit;

        // Onay butonunu gizle ve placeholder efektini baþlat
        confirmButton.gameObject.SetActive(false);
        placeholderText = inputField.placeholder as TextMeshProUGUI;

        // JSON dosyasýndan kaydedilmiþ ismi yükle
        LoadTableName();

        // InputField’a týklandýðýnda butonu göster
        inputField.onSelect.AddListener(ShowConfirmButton);

        // Butona týklandýðýnda metni onayla ve klavyeyi kapat
        confirmButton.onClick.AddListener(ConfirmText);
    }

    private void ShowConfirmButton(string text)
    {
        confirmButton.gameObject.SetActive(true); // Onay butonunu göster
    }

    private void ConfirmText()
    {
        string newText = inputField.text;

        if (!string.IsNullOrEmpty(newText) && newText.Length <= maxCharacterLimit)
        {
            tableText.text = newText;               // Metni tabelaya yaz
            SaveTableName(newText);                 // JSON dosyasýna kaydet
            isTextSet = true;
        }
        else
        {
            Debug.LogWarning("Metin 20 karakterden uzun veya boþ olamaz!");
        }

        // Klavyeyi kapat ve onay butonunu gizle
        inputField.DeactivateInputField();
        confirmButton.gameObject.SetActive(false);
    }

    private void SaveTableName(string tableName)
    {
        UserData data = new UserData { tableName = tableName };
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(filePath, json);  // JSON dosyasýna yazma
    }

    private void LoadTableName()
    {
        if (File.Exists(filePath))  // JSON dosyasý varsa
        {
            string json = File.ReadAllText(filePath); // JSON dosyasýný oku
            UserData data = JsonUtility.FromJson<UserData>(json);
            tableText.text = data.tableName;          // Ýsmi tabelaya yaz
            inputField.text = data.tableName;         // InputField'da göster
            isTextSet = true;
        }
        else
        {
            StartCoroutine(BlinkPlaceholderEffect()); // Eðer kayýtlý isim yoksa placeholder yanýp sönsün
        }
    }

    private IEnumerator BlinkPlaceholderEffect()
    {
        if (placeholderText == null)
        {
            yield break; // Placeholder metni yoksa çýkýþ yap
        }

        Color originalColor = placeholderText.color;

        while (!isTextSet) // Ýsmin girildiðini takip eder
        {
            for (float alpha = 1f; alpha >= 0; alpha -= Time.deltaTime)
            {
                placeholderText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                yield return null;
            }

            for (float alpha = 0; alpha <= 1f; alpha += Time.deltaTime)
            {
                placeholderText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                yield return null;
            }
        }

        placeholderText.color = originalColor; // Ýsmin ayarlandýðý durumda normal rengi döndür
    }
}
