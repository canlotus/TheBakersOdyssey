using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotManager : MonoBehaviour
{
    public Image[] slotImages = new Image[7];  // Slotlarda gösterilecek Image bileþenleri
    public Sprite[] objectSprites;  // Her objeye karþýlýk gelen resimler
    public Image[] objectGoalImages = new Image[4];  // Hangi objeyi toplamamýz gerektiðini gösterecek görsel slotlar
    public TextMeshProUGUI[] objectCountTexts = new TextMeshProUGUI[4]; // 4 obje sayýsý
    public TextMeshProUGUI winMessageText;

    private int currentSlotIndex = 0;

    // LevelData'dan gelen requiredObjects kullanýlarak kazanma koþullarý belirlenecek
    public List<RequiredObject> requiredObjects = new List<RequiredObject>();

    void Start()
    {
        // Baþlangýçta tüm slotlarý görünmez yapýyoruz (saydam olacaklar)
        foreach (var image in slotImages)
        {
            image.color = new Color(1, 1, 1, 0);  // Saydam hale getir
        }

        // Obje hedef slotlarýný (objectGoalImages) boþ tutarak baþlatýyoruz
        foreach (var goalImage in objectGoalImages)
        {
            goalImage.color = new Color(1, 1, 1, 0);  // Baþlangýçta görünmez olacaklar
        }
    }

    // JSON'dan gelen veriyi buraya gönderiyoruz
    public void InitializeWinCondition(WinCondition winCondition)
    {
        requiredObjects = new List<RequiredObject>(winCondition.requiredObjects);

        // Objeleri UI'ye yazalým (artýk isim yerine görsel göstereceðiz)
        int index = 0;
        foreach (var obj in requiredObjects)
        {
            if (index < objectGoalImages.Length)
            {
                Sprite objectSprite = GetSpriteByObjectName(obj.objectName);  // Objeye göre görseli bul
                if (objectSprite != null)
                {
                    objectGoalImages[index].sprite = objectSprite;
                    objectGoalImages[index].enabled = true;
                    objectGoalImages[index].color = new Color(1, 1, 1, 1);  // Görünür yap
                }
                objectCountTexts[index].text = obj.count.ToString();  // Gerekli miktarý yaz
                index++;
            }
        }

        // Kalan image slotlarýný boþ tutuyoruz
        for (int i = index; i < objectGoalImages.Length; i++)
        {
            objectGoalImages[i].sprite = null;
            objectGoalImages[i].color = new Color(1, 1, 1, 0);  // Görünmez yap
            objectCountTexts[i].text = "";  // Boþ text býrakýyoruz
        }
    }

    // Seçilen objeyi sýradaki boþ slota yerleþtir ve objeyi yok et
    public void MoveToNextSlot(GameObject obj)
    {
        if (currentSlotIndex < slotImages.Length)
        {
            Sprite objectSprite = GetSpriteByObjectName(obj.name);  // Obje adýna göre resim seç
            if (objectSprite != null)
            {
                slotImages[currentSlotIndex].sprite = objectSprite;
                slotImages[currentSlotIndex].enabled = true;
                slotImages[currentSlotIndex].color = new Color(1, 1, 1, 1);  // Görünür yap
            }

            currentSlotIndex++;

            // 3'lü eþleþme kontrolü
            CheckForMerge();

            // Kazanma koþulunu kontrol et
            UpdateGoalProgress(obj);

            // Týklanan objeyi yok et
            Destroy(obj);

            // 7 slot dolduysa ve eþleþme yoksa oyunu kaybet
            if (currentSlotIndex == slotImages.Length && !HasMergeableObjects())
            {
                GameOver();
            }
        }
    }

    // Kazanma koþuluna göre toplanan objeleri kontrol et ve UI'yi güncelle
    void UpdateGoalProgress(GameObject collectedObject)
    {
        string collectedName = collectedObject.name;

        // requiredObjects listesi içinde karþýlýk gelen bir obje var mý kontrol et
        foreach (var requiredObject in requiredObjects)
        {
            if (collectedName == requiredObject.objectName)
            {
                requiredObject.count--;
                UpdateUIText(requiredObject.objectName);  // UI'yi güncelle

                // Eðer tüm objeler toplandýysa kazanma durumu
                if (AllGoalsCompleted())
                {
                    if (winMessageText != null)
                    {
                        winMessageText.text = "Kazandýn!";
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

                return;  // Ýþlem tamam
            }
        }
    }

    // UI'deki text alanlarýný günceller
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

    // Kazanma koþulunun tamamlanýp tamamlanmadýðýný kontrol et
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

    // 3'lü eþleþme kontrolü
    void CheckForMerge()
    {
        Dictionary<Sprite, int> imageCounts = new Dictionary<Sprite, int>();

        for (int i = 0; i < currentSlotIndex; i++)
        {
            Sprite currentImage = slotImages[i].sprite;

            if (currentImage == null) continue;  // Eðer null ise bu slotu atla

            if (imageCounts.ContainsKey(currentImage))
            {
                imageCounts[currentImage]++;
            }
            else
            {
                imageCounts[currentImage] = 1;
            }

            // Eðer 3 ayný resim varsa, bunlarý birleþtiriyoruz
            if (imageCounts[currentImage] == 3)
            {
                MergeImages(currentImage);

                // 3'lü eþleþme sesi çal
                if (AudioManager.instance != null)
                {
                    AudioManager.instance.PlayMergeSound();
                }

                break;
            }
        }
    }

    // Boþ slotlar olup olmadýðýný kontrol eder, eþleþecek 3 ayný obje (image) varsa True döner
    bool HasMergeableObjects()
    {
        Dictionary<Sprite, int> imageCounts = new Dictionary<Sprite, int>();

        for (int i = 0; i < currentSlotIndex; i++)
        {
            Sprite currentImage = slotImages[i].sprite;

            if (currentImage == null) continue;  // Eðer null ise bu slotu atla

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

    // 3 adet ayný resmi birleþtirip kalanlarý sýralýyoruz
    void MergeImages(Sprite image)
    {
        List<int> slotsToClear = new List<int>();

        // 3 ayný resmi bulup listeden kaldýrýyoruz
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

        // Slotlarý temizliyoruz ama sprite'ý null yapmadan içeriklerini kaydýrýyoruz
        foreach (int slotIndex in slotsToClear)
        {
            slotImages[slotIndex].sprite = null;  // Resmi temizle
            slotImages[slotIndex].enabled = false;  // Slotu devre dýþý býrak
            slotImages[slotIndex].color = new Color(1, 1, 1, 0);  // Saydam yap
        }

        // Objeler silindiði için slotlarý yeniden sýralýyoruz
        RearrangeSlots();
    }

    // Kalan objeleri boþ slotlara sýralama
    void RearrangeSlots()
    {
        int shiftIndex = 0;

        // Slotlarý sola kaydýrma
        for (int i = 0; i < slotImages.Length; i++)
        {
            if (slotImages[i].sprite != null)
            {
                if (shiftIndex != i)
                {
                    slotImages[shiftIndex].sprite = slotImages[i].sprite;
                    slotImages[shiftIndex].enabled = true;
                    slotImages[shiftIndex].color = new Color(1, 1, 1, 1);  // Görünür yap
                    slotImages[i].sprite = null;
                    slotImages[i].enabled = false;
                    slotImages[i].color = new Color(1, 1, 1, 0);  // Saydam yap
                }
                shiftIndex++;
            }
        }

        // Son slot indeksi güncelleniyor
        currentSlotIndex = shiftIndex;
    }

    // Game Over durumu
    void GameOver()
    {
        Debug.Log("Game Over! 7 obje yerleþtirildi ve eþleþme yok.");

        Menu menu = FindObjectOfType<Menu>();
        if (menu != null)
        {
            menu.ShowGameOverPanel();
        }
    }

    // Objeye karþýlýk gelen resmi bulur
    Sprite GetSpriteByObjectName(string objectName)
    {
        // Objeye karþýlýk gelen sprite'ý döndür (A'dan Z'ye kadar harfler için)
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
                return null;  // Eðer objenin ismi harf dýþýnda bir þeyse 
        }
    }
}
