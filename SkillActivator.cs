using UnityEngine;
using System.Collections;
using TMPro;

public class SkillActivator : MonoBehaviour
{
    public GameObject skillObject;           // Skill objesi referans�
    public float rotationSpeed = 100f;       // Y ekseninde d�n�� h�z�
    public float targetXPosition = -0.23f;   // Gidece�i x pozisyon
    public float movementDuration = 2f;      // Hareket s�resi (2 saniye)
    public int skillCount = 1;               // Ba�lang�� skill say�s�
    public TextMeshProUGUI skillCountText;   // Skill say�s�n� g�steren UI text

    public GameObject noSkillPanel;          // Skill say�s� 0 oldu�unda t�klamayla a��lacak panel

    private Vector3 startPosition;
    private bool isMoving = false;           // Hareketin devam edip etmedi�i kontrol�

    void Start()
    {
        skillObject.SetActive(false);        // Oyun ba�lad���nda skill objesi gizli
        startPosition = new Vector3(0.4f, skillObject.transform.position.y, skillObject.transform.position.z);

        // Skill say�s�n� PlayerPrefs'ten y�kle
        skillCount = PlayerPrefs.GetInt("SkillCount_Activator", skillCount);
        UpdateSkillText();                   // Ba�lang��ta skill say�s�n� UI'ye yans�t
        noSkillPanel.SetActive(false);       // Panel ba�lang��ta gizli
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

    // Buton taraf�ndan �a�r�lacak metod
    public void ActivateSkill()
    {
        if (skillCount > 0 && !isMoving)
        {
            skillCount--;                    // Skill say�s�n� azalt
            UpdateSkillText();               // UI'yi g�ncelle
            PlayerPrefs.SetInt("SkillCount_Activator", skillCount);  // G�ncel skill say�s�n� kaydet
            PlayerPrefs.Save();
            skillObject.transform.position = startPosition; // Ba�lang�� pozisyonuna geri al
            skillObject.SetActive(true);     // Objeyi aktif hale getir
            isMoving = true;                 // Hareketi ba�lat
        }
        else if (skillCount <= 0)
        {
            noSkillPanel.SetActive(true);    // Skill say�s� 0 ise paneli g�ster
        }
    }

    // Paneldeki "3 Skill Ekle" butonu i�in fonksiyon
    public void AddSkills()
    {
        skillCount += 3;                     // Skill say�s�n� 3 art�r
        UpdateSkillText();                   // UI'yi g�ncelle
        PlayerPrefs.SetInt("SkillCount_Activator", skillCount);  // Yeni skill say�s�n� kaydet
        PlayerPrefs.Save();
        noSkillPanel.SetActive(false);       // Paneli kapat
    }

    // Paneldeki "Paneli Kapat" butonu i�in fonksiyon
    public void ClosePanel()
    {
        noSkillPanel.SetActive(false);
    }

    // Objeyi belirtilen s�re sonra deaktive etme
    private IEnumerator DeactivateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        skillObject.SetActive(false);
    }

    // Skill say�s�n� UI'ye yans�t�r
    private void UpdateSkillText()
    {
        if (skillCountText != null)
        {
            skillCountText.text = skillCount > 0 ? skillCount.ToString() : "+";
        }
    }
}
