using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotManager : MonoBehaviour
{
    public Image[] slotImages = new Image[7];  // Slotlarda g�sterilecek Image bile�enleri
    public Sprite[] objectSprites;  // Her objeye kar��l�k gelen resimler
    public Image[] objectGoalImages = new Image[4];  // Hangi objeyi toplamam�z gerekti�ini g�sterecek g�rsel slotlar
    public TextMeshProUGUI[] objectCountTexts = new TextMeshProUGUI[4]; // 4 obje say�s�
    public TextMeshProUGUI winMessageText;

    private int currentSlotIndex = 0;

    // LevelData'dan gelen requiredObjects kullan�larak kazanma ko�ullar� belirlenecek
    public List<RequiredObject> requiredObjects = new List<RequiredObject>();

    void Start()
    {
        // Ba�lang��ta t�m slotlar� g�r�nmez yap�yoruz (saydam olacaklar)
        foreach (var image in slotImages)
        {
            image.color = new Color(1, 1, 1, 0);  // Saydam hale getir
        }

        // Obje hedef slotlar�n� (objectGoalImages) bo� tutarak ba�lat�yoruz
        foreach (var goalImage in objectGoalImages)
        {
            goalImage.color = new Color(1, 1, 1, 0);  // Ba�lang��ta g�r�nmez olacaklar
        }
    }

    // JSON'dan gelen veriyi buraya g�nderiyoruz
    public void InitializeWinCondition(WinCondition winCondition)
    {
        requiredObjects = new List<RequiredObject>(winCondition.requiredObjects);

        // Objeleri UI'ye yazal�m (art�k isim yerine g�rsel g�sterece�iz)
        int index = 0;
        foreach (var obj in requiredObjects)
        {
            if (index < objectGoalImages.Length)
            {
                Sprite objectSprite = GetSpriteByObjectName(obj.objectName);  // Objeye g�re g�rseli bul
                if (objectSprite != null)
                {
                    objectGoalImages[index].sprite = objectSprite;
                    objectGoalImages[index].enabled = true;
                    objectGoalImages[index].color = new Color(1, 1, 1, 1);  // G�r�n�r yap
                }
                objectCountTexts[index].text = obj.count.ToString();  // Gerekli miktar� yaz
                index++;
            }
        }

        // Kalan image slotlar�n� bo� tutuyoruz
        for (int i = index; i < objectGoalImages.Length; i++)
        {
            objectGoalImages[i].sprite = null;
            objectGoalImages[i].color = new Color(1, 1, 1, 0);  // G�r�nmez yap
            objectCountTexts[i].text = "";  // Bo� text b�rak�yoruz
        }
    }

    // Se�ilen objeyi s�radaki bo� slota yerle�tir ve objeyi yok et
    public void MoveToNextSlot(GameObject obj)
    {
        if (currentSlotIndex < slotImages.Length)
        {
            Sprite objectSprite = GetSpriteByObjectName(obj.name);  // Obje ad�na g�re resim se�
            if (objectSprite != null)
            {
                slotImages[currentSlotIndex].sprite = objectSprite;
                slotImages[currentSlotIndex].enabled = true;
                slotImages[currentSlotIndex].color = new Color(1, 1, 1, 1);  // G�r�n�r yap
            }

            currentSlotIndex++;

            // 3'l� e�le�me kontrol�
            CheckForMerge();

            // Kazanma ko�ulunu kontrol et
            UpdateGoalProgress(obj);

            // T�klanan objeyi yok et
            Destroy(obj);

            // 7 slot dolduysa ve e�le�me yoksa oyunu kaybet
            if (currentSlotIndex == slotImages.Length && !HasMergeableObjects())
            {
                GameOver();
            }
        }
    }

    // Kazanma ko�uluna g�re toplanan objeleri kontrol et ve UI'yi g�ncelle
    void UpdateGoalProgress(GameObject collectedObject)
    {
        string collectedName = collectedObject.name;

        // requiredObjects listesi i�inde kar��l�k gelen bir obje var m� kontrol et
        foreach (var requiredObject in requiredObjects)
        {
            if (collectedName == requiredObject.objectName)
            {
                requiredObject.count--;
                UpdateUIText(requiredObject.objectName);  // UI'yi g�ncelle

                // E�er t�m objeler topland�ysa kazanma durumu
                if (AllGoalsCompleted())
                {
                    if (winMessageText != null)
                    {
                        winMessageText.text = "Kazand�n!";
                    }

                    int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
                    int nextLevel = currentLevel + 1;
                    PlayerPrefs.SetInt("CurrentLevel", nextLevel);
                    PlayerPrefs.Save();

                    Menu menu = FindObjectOfType<Menu>();
                    if (menu != null)
                    {
                        menu.ShowWinPanel();
                    }
                }

                return;  // ��lem tamam
            }
        }
    }

    // UI'deki text alanlar�n� g�nceller
    void UpdateUIText(string objectName)
    {
        int index = 0;
        foreach (var obj in requiredObjects)
        {
            if (obj.objectName == objectName)
            {
                int remaining = obj.count;
                objectCountTexts[index].text = remaining.ToString();
                break;
            }
            index++;
        }
    }

    // Kazanma ko�ulunun tamamlan�p tamamlanmad���n� kontrol et
    bool AllGoalsCompleted()
    {
        foreach (var obj in requiredObjects)
        {
            if (obj.count > 0)
            {
                return false;
            }
        }
        return true;
    }

    // 3'l� e�le�me kontrol�
    void CheckForMerge()
    {
        Dictionary<Sprite, int> imageCounts = new Dictionary<Sprite, int>();

        for (int i = 0; i < currentSlotIndex; i++)
        {
            Sprite currentImage = slotImages[i].sprite;

            if (currentImage == null) continue;  // E�er null ise bu slotu atla

            if (imageCounts.ContainsKey(currentImage))
            {
                imageCounts[currentImage]++;
            }
            else
            {
                imageCounts[currentImage] = 1;
            }

            // E�er 3 ayn� resim varsa, bunlar� birle�tiriyoruz
            if (imageCounts[currentImage] == 3)
            {
                MergeImages(currentImage);

                // 3'l� e�le�me sesi �al
                if (AudioManager.instance != null)
                {
                    AudioManager.instance.PlayMergeSound();
                }

                break;
            }
        }
    }

    // Bo� slotlar olup olmad���n� kontrol eder, e�le�ecek 3 ayn� obje (image) varsa True d�ner
    bool HasMergeableObjects()
    {
        Dictionary<Sprite, int> imageCounts = new Dictionary<Sprite, int>();

        for (int i = 0; i < currentSlotIndex; i++)
        {
            Sprite currentImage = slotImages[i].sprite;

            if (currentImage == null) continue;  // E�er null ise bu slotu atla

            if (imageCounts.ContainsKey(currentImage))
            {
                imageCounts[currentImage]++;
            }
            else
            {
                imageCounts[currentImage] = 1;
            }

            if (imageCounts[currentImage] >= 3)
            {
                return true;
            }
        }

        return false;
    }

    // 3 adet ayn� resmi birle�tirip kalanlar� s�ral�yoruz
    void MergeImages(Sprite image)
    {
        List<int> slotsToClear = new List<int>();

        // 3 ayn� resmi bulup listeden kald�r�yoruz
        for (int i = 0; i < currentSlotIndex; i++)
        {
            if (slotImages[i].sprite == image)
            {
                slotsToClear.Add(i);
                if (slotsToClear.Count == 3)
                {
                    break;
                }
            }
        }

        // Slotlar� temizliyoruz ama sprite'� null yapmadan i�eriklerini kayd�r�yoruz
        foreach (int slotIndex in slotsToClear)
        {
            slotImages[slotIndex].sprite = null;  // Resmi temizle
            slotImages[slotIndex].enabled = false;  // Slotu devre d��� b�rak
            slotImages[slotIndex].color = new Color(1, 1, 1, 0);  // Saydam yap
        }

        // Objeler silindi�i i�in slotlar� yeniden s�ral�yoruz
        RearrangeSlots();
    }

    // Kalan objeleri bo� slotlara s�ralama
    void RearrangeSlots()
    {
        int shiftIndex = 0;

        // Slotlar� sola kayd�rma
        for (int i = 0; i < slotImages.Length; i++)
        {
            if (slotImages[i].sprite != null)
            {
                if (shiftIndex != i)
                {
                    slotImages[shiftIndex].sprite = slotImages[i].sprite;
                    slotImages[shiftIndex].enabled = true;
                    slotImages[shiftIndex].color = new Color(1, 1, 1, 1);  // G�r�n�r yap
                    slotImages[i].sprite = null;
                    slotImages[i].enabled = false;
                    slotImages[i].color = new Color(1, 1, 1, 0);  // Saydam yap
                }
                shiftIndex++;
            }
        }

        // Son slot indeksi g�ncelleniyor
        currentSlotIndex = shiftIndex;
    }

    // Game Over durumu
    void GameOver()
    {
        Debug.Log("Game Over! 7 obje yerle�tirildi ve e�le�me yok.");

        Menu menu = FindObjectOfType<Menu>();
        if (menu != null)
        {
            menu.ShowGameOverPanel();
        }
    }

    // Objeye kar��l�k gelen resmi bulur
    Sprite GetSpriteByObjectName(string objectName)
    {
        // Objeye kar��l�k gelen sprite'� d�nd�r (A'dan Z'ye kadar harfler i�in)
        switch (objectName)
        {
            case "A":
                return objectSprites[0];
            case "B":
                return objectSprites[1];
            case "C":
                return objectSprites[2];
            case "D":
                return objectSprites[3];
            case "E":
                return objectSprites[4];
            case "F":
                return objectSprites[5];
            case "G":
                return objectSprites[6];
            case "H":
                return objectSprites[7];
            case "I":
                return objectSprites[8];
            case "J":
                return objectSprites[9];
            case "K":
                return objectSprites[10];
            case "L":
                return objectSprites[11];
            case "M":
                return objectSprites[12];
            case "N":
                return objectSprites[13];
            case "O":
                return objectSprites[14];
            case "P":
                return objectSprites[15];
            case "Q":
                return objectSprites[16];
            case "R":
                return objectSprites[17];
            case "S":
                return objectSprites[18];
            case "T":
                return objectSprites[19];
            case "U":
                return objectSprites[20];
            case "V":
                return objectSprites[21];
            case "W":
                return objectSprites[22];
            case "X":
                return objectSprites[23];
            case "Y":
                return objectSprites[24];
            case "Z":
                return objectSprites[25];
            default:
                return null;  // E�er objenin ismi harf d���nda bir �eyse 
        }
    }
}
