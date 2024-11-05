using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class TableTextManager : MonoBehaviour
{
    public TextMeshProUGUI tableText;         // Tabeladaki TMP metni
    public TMP_InputField inputField;         // Kullan�c�n�n metin girece�i TMP alan�
    public Button confirmButton;              // Onay butonu
    private int maxCharacterLimit = 20;       // Maksimum karakter say�s�
    private bool isTextSet = false;           // �smin tabloya kaydedildi�ini takip eder
    private TextMeshProUGUI placeholderText;  // Placeholder metni

    private string filePath;                  // JSON dosyas�n�n yolu

    private void Start()
    {
        filePath = Application.persistentDataPath + "/userdata.json"; // JSON dosya yolu belirleme
        inputField.characterLimit = maxCharacterLimit;

        // Onay butonunu gizle ve placeholder efektini ba�lat
        confirmButton.gameObject.SetActive(false);
        placeholderText = inputField.placeholder as TextMeshProUGUI;

        // JSON dosyas�ndan kaydedilmi� ismi y�kle
        LoadTableName();

        // InputField�a t�kland���nda butonu g�ster
        inputField.onSelect.AddListener(ShowConfirmButton);

        // Butona t�kland���nda metni onayla ve klavyeyi kapat
        confirmButton.onClick.AddListener(ConfirmText);
    }

    private void ShowConfirmButton(string text)
    {
        confirmButton.gameObject.SetActive(true); // Onay butonunu g�ster
    }

    private void ConfirmText()
    {
        string newText = inputField.text;

        if (!string.IsNullOrEmpty(newText) && newText.Length <= maxCharacterLimit)
        {
            tableText.text = newText;               // Metni tabelaya yaz
            SaveTableName(newText);                 // JSON dosyas�na kaydet
            isTextSet = true;
        }
        else
        {
            Debug.LogWarning("Metin 20 karakterden uzun veya bo� olamaz!");
        }

        // Klavyeyi kapat ve onay butonunu gizle
        inputField.DeactivateInputField();
        confirmButton.gameObject.SetActive(false);
    }

    private void SaveTableName(string tableName)
    {
        UserData data = new UserData { tableName = tableName };
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(filePath, json);  // JSON dosyas�na yazma
    }

    private void LoadTableName()
    {
        if (File.Exists(filePath))  // JSON dosyas� varsa
        {
            string json = File.ReadAllText(filePath); // JSON dosyas�n� oku
            UserData data = JsonUtility.FromJson<UserData>(json);
            tableText.text = data.tableName;          // �smi tabelaya yaz
            inputField.text = data.tableName;         // InputField'da g�ster
            isTextSet = true;
        }
        else
        {
            StartCoroutine(BlinkPlaceholderEffect()); // E�er kay�tl� isim yoksa placeholder yan�p s�ns�n
        }
    }

    private IEnumerator BlinkPlaceholderEffect()
    {
        if (placeholderText == null)
        {
            yield break; // Placeholder metni yoksa ��k�� yap
        }

        Color originalColor = placeholderText.color;

        while (!isTextSet) // �smin girildi�ini takip eder
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

        placeholderText.color = originalColor; // �smin ayarland��� durumda normal rengi d�nd�r
    }
}
