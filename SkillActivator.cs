using UnityEngine;
using System.Collections;
using TMPro;

public class SkillActivator : MonoBehaviour
{
    public GameObject skillObject;           // Skill objesi referansý
    public float rotationSpeed = 100f;       // Y ekseninde dönüþ hýzý
    public float targetXPosition = -0.23f;   // Gideceði x pozisyon
    public float movementDuration = 2f;      // Hareket süresi (2 saniye)
    public int skillCount = 1;               // Baþlangýç skill sayýsý
    public TextMeshProUGUI skillCountText;   // Skill sayýsýný gösteren UI text

    public GameObject noSkillPanel;          // Skill sayýsý 0 olduðunda týklamayla açýlacak panel

    private Vector3 startPosition;
    private bool isMoving = false;           // Hareketin devam edip etmediði kontrolü

    void Start()
    {
        skillObject.SetActive(false);        // Oyun baþladýðýnda skill objesi gizli
        startPosition = new Vector3(0.4f, skillObject.transform.position.y, skillObject.transform.position.z);

        // Skill sayýsýný PlayerPrefs'ten yükle
        skillCount = PlayerPrefs.GetInt("SkillCount_Activator", skillCount);
        UpdateSkillText();                   // Baþlangýçta skill sayýsýný UI'ye yansýt
        noSkillPanel.SetActive(false);       // Panel baþlangýçta gizli
    }

    void Update()
    {
        if (isMoving)
        {
            skillObject.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);

            float newX = Mathf.MoveTowards(skillObject.transform.position.x, targetXPosition,
                          (0.4f - targetXPosition) / movementDuration * Time.deltaTime);
            skillObject.transform.position = new Vector3(newX, startPosition.y, startPosition.z);

            if (Mathf.Abs(skillObject.transform.position.x - targetXPosition) < 0.01f)
            {
                isMoving = false;
                StartCoroutine(DeactivateAfterDelay(0.5f)); // 0.5 saniye sonra kapatma
            }
        }
    }

    // Buton tarafýndan çaðrýlacak metod
    public void ActivateSkill()
    {
        if (skillCount > 0 && !isMoving)
        {
            skillCount--;                    // Skill sayýsýný azalt
            UpdateSkillText();               // UI'yi güncelle
            PlayerPrefs.SetInt("SkillCount_Activator", skillCount);  // Güncel skill sayýsýný kaydet
            PlayerPrefs.Save();
            skillObject.transform.position = startPosition; // Baþlangýç pozisyonuna geri al
            skillObject.SetActive(true);     // Objeyi aktif hale getir
            isMoving = true;                 // Hareketi baþlat
        }
        else if (skillCount <= 0)
        {
            noSkillPanel.SetActive(true);    // Skill sayýsý 0 ise paneli göster
        }
    }

    // Paneldeki "3 Skill Ekle" butonu için fonksiyon
    public void AddSkills()
    {
        skillCount += 3;                     // Skill sayýsýný 3 artýr
        UpdateSkillText();                   // UI'yi güncelle
        PlayerPrefs.SetInt("SkillCount_Activator", skillCount);  // Yeni skill sayýsýný kaydet
        PlayerPrefs.Save();
        noSkillPanel.SetActive(false);       // Paneli kapat
    }

    // Paneldeki "Paneli Kapat" butonu için fonksiyon
    public void ClosePanel()
    {
        noSkillPanel.SetActive(false);
    }

    // Objeyi belirtilen süre sonra deaktive etme
    private IEnumerator DeactivateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        skillObject.SetActive(false);
    }

    // Skill sayýsýný UI'ye yansýtýr
    private void UpdateSkillText()
    {
        if (skillCountText != null)
        {
            skillCountText.text = skillCount > 0 ? skillCount.ToString() : "+";
        }
    }
}
