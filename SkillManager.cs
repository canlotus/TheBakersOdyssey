using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SkillManager : MonoBehaviour
{
    public Menu menuScript;               // Menu scriptine referans
    public float skillDuration = 10f;     // Skill s�resi toplam 10 saniye
    public int skillCount = 1;            // Ba�lang�� skill say�s�
    public TextMeshProUGUI skillCountText;// Kalan skill say�s�n� g�steren text
    public GameObject noSkillPanel;       // Skill say�s� 0 oldu�unda a��lan panel
    public Image skillImage;              // Skill kullan�ld���nda hareket eden g�rsel

    private bool isSkillActive = false;
    private Vector2 initialPosition = new Vector2(19, -342); // Ba�lang�� konumu
    private Vector2 targetPosition = new Vector2(-168, -209); // Hedef konumu
    private Vector3 initialScale = new Vector3(2, 2, 1);      // Ba�lang�� �l�e�i
    private Vector3 targetScale = new Vector3(1, 1, 1);       // Hedef �l�e�i
    private float moveDuration = 1.5f;                        // Animasyon s�resi (1.5 saniye)

    private void Start()
    {
        if (menuScript == null)
        {
            menuScript = FindObjectOfType<Menu>(); // Menu scriptini sahnede bul
        }

        // Skill say�s�n� PlayerPrefs'ten y�kle
        skillCount = PlayerPrefs.GetInt("SkillCount", skillCount);
        UpdateSkillText(); // Ba�lang��ta skill say�s�n� UI'ye yans�t
        noSkillPanel.SetActive(false); // Panel ba�lang��ta gizli
        skillImage.gameObject.SetActive(false); // Skill g�rseli ba�lang��ta gizli
    }

    public void ActivateTimeStopSkill()
    {
        // Skill say�s� 0'dan b�y�kse ve skill etkin de�ilse �al��t�r
        if (skillCount > 0 && !isSkillActive)
        {
            skillCount--;                   // Skill say�s�n� azalt
            UpdateSkillText();              // UI'yi g�ncelle
            PlayerPrefs.SetInt("SkillCount", skillCount); // Yeni skill say�s�n� kaydet
            PlayerPrefs.Save();
            StartCoroutine(TimeStopSkillCoroutine());
        }
        else if (skillCount <= 0)
        {
            noSkillPanel.SetActive(true);   // Skill say�s� 0 ise paneli g�ster
        }
    }

    private IEnumerator TimeStopSkillCoroutine()
    {
        isSkillActive = true;

        // Skill g�rselini etkinle�tir ve animasyonu ba�lat
        skillImage.gameObject.SetActive(true);
        StartCoroutine(AnimateSkillImage());

        // S�reyi durdur
        menuScript.isPaused = true;

        yield return new WaitForSeconds(skillDuration);  // 10 saniye bekle

        // S�reyi tekrar ba�lat
        menuScript.isPaused = false;
        isSkillActive = false;

        // Skill g�rselini ba�lang�� durumuna d�nd�r
        skillImage.gameObject.SetActive(false);
        skillImage.rectTransform.anchoredPosition = initialPosition;
        skillImage.rectTransform.localScale = initialScale;
    }

    private IEnumerator AnimateSkillImage()
    {
        float elapsedTime = 0f;

        // G�rseli 1.5 saniyede hedef konuma ve �l�e�e getir
        while (elapsedTime < moveDuration)
        {
            skillImage.rectTransform.anchoredPosition = Vector2.Lerp(initialPosition, targetPosition, elapsedTime / moveDuration);
            skillImage.rectTransform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / moveDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Hedef konuma ve �l�e�e tam olarak yerle�tir
        skillImage.rectTransform.anchoredPosition = targetPosition;
        skillImage.rectTransform.localScale = targetScale;
    }

    // Paneldeki "3 Skill Ekle" butonu i�in fonksiyon
    public void AddSkills()
    {
        skillCount += 3;                    // Skill say�s�n� 3 art�r
        UpdateSkillText();                  // UI'yi g�ncelle
        PlayerPrefs.SetInt("SkillCount", skillCount); // Yeni skill say�s�n� kaydet
        PlayerPrefs.Save();
        noSkillPanel.SetActive(false);      // Paneli kapat
    }

    // Paneldeki "Paneli Kapat" butonu i�in fonksiyon
    public void ClosePanel()
    {
        noSkillPanel.SetActive(false);
    }

    // Skill say�s�n� UI'ye yans�t�r
    private void UpdateSkillText()
    {
        if (skillCountText != null)
        {
            skillCountText.text = skillCount > 0 ? skillCount.ToString() : "+"; // Skill say�s�n� veya "+" simgesini g�ster
        }
    }
}
