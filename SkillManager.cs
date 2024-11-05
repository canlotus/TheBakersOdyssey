using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SkillManager : MonoBehaviour
{
    public Menu menuScript;               // Menu scriptine referans
    public float skillDuration = 10f;     // Skill süresi toplam 10 saniye
    public int skillCount = 1;            // Baþlangýç skill sayýsý
    public TextMeshProUGUI skillCountText;// Kalan skill sayýsýný gösteren text
    public GameObject noSkillPanel;       // Skill sayýsý 0 olduðunda açýlan panel
    public Image skillImage;              // Skill kullanýldýðýnda hareket eden görsel

    private bool isSkillActive = false;
    private Vector2 initialPosition = new Vector2(19, -342); // Baþlangýç konumu
    private Vector2 targetPosition = new Vector2(-168, -209); // Hedef konumu
    private Vector3 initialScale = new Vector3(2, 2, 1);      // Baþlangýç ölçeði
    private Vector3 targetScale = new Vector3(1, 1, 1);       // Hedef ölçeði
    private float moveDuration = 1.5f;                        // Animasyon süresi (1.5 saniye)

    private void Start()
    {
        if (menuScript == null)
        {
            menuScript = FindObjectOfType<Menu>(); // Menu scriptini sahnede bul
        }

        // Skill sayýsýný PlayerPrefs'ten yükle
        skillCount = PlayerPrefs.GetInt("SkillCount", skillCount);
        UpdateSkillText(); // Baþlangýçta skill sayýsýný UI'ye yansýt
        noSkillPanel.SetActive(false); // Panel baþlangýçta gizli
        skillImage.gameObject.SetActive(false); // Skill görseli baþlangýçta gizli
    }

    public void ActivateTimeStopSkill()
    {
        // Skill sayýsý 0'dan büyükse ve skill etkin deðilse çalýþtýr
        if (skillCount > 0 && !isSkillActive)
        {
            skillCount--;                   // Skill sayýsýný azalt
            UpdateSkillText();              // UI'yi güncelle
            PlayerPrefs.SetInt("SkillCount", skillCount); // Yeni skill sayýsýný kaydet
            PlayerPrefs.Save();
            StartCoroutine(TimeStopSkillCoroutine());
        }
        else if (skillCount <= 0)
        {
            noSkillPanel.SetActive(true);   // Skill sayýsý 0 ise paneli göster
        }
    }

    private IEnumerator TimeStopSkillCoroutine()
    {
        isSkillActive = true;

        // Skill görselini etkinleþtir ve animasyonu baþlat
        skillImage.gameObject.SetActive(true);
        StartCoroutine(AnimateSkillImage());

        // Süreyi durdur
        menuScript.isPaused = true;

        yield return new WaitForSeconds(skillDuration);  // 10 saniye bekle

        // Süreyi tekrar baþlat
        menuScript.isPaused = false;
        isSkillActive = false;

        // Skill görselini baþlangýç durumuna döndür
        skillImage.gameObject.SetActive(false);
        skillImage.rectTransform.anchoredPosition = initialPosition;
        skillImage.rectTransform.localScale = initialScale;
    }

    private IEnumerator AnimateSkillImage()
    {
        float elapsedTime = 0f;

        // Görseli 1.5 saniyede hedef konuma ve ölçeðe getir
        while (elapsedTime < moveDuration)
        {
            skillImage.rectTransform.anchoredPosition = Vector2.Lerp(initialPosition, targetPosition, elapsedTime / moveDuration);
            skillImage.rectTransform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / moveDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Hedef konuma ve ölçeðe tam olarak yerleþtir
        skillImage.rectTransform.anchoredPosition = targetPosition;
        skillImage.rectTransform.localScale = targetScale;
    }

    // Paneldeki "3 Skill Ekle" butonu için fonksiyon
    public void AddSkills()
    {
        skillCount += 3;                    // Skill sayýsýný 3 artýr
        UpdateSkillText();                  // UI'yi güncelle
        PlayerPrefs.SetInt("SkillCount", skillCount); // Yeni skill sayýsýný kaydet
        PlayerPrefs.Save();
        noSkillPanel.SetActive(false);      // Paneli kapat
    }

    // Paneldeki "Paneli Kapat" butonu için fonksiyon
    public void ClosePanel()
    {
        noSkillPanel.SetActive(false);
    }

    // Skill sayýsýný UI'ye yansýtýr
    private void UpdateSkillText()
    {
        if (skillCountText != null)
        {
            skillCountText.text = skillCount > 0 ? skillCount.ToString() : "+"; // Skill sayýsýný veya "+" simgesini göster
        }
    }
}
