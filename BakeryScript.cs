using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BakeryScript : MonoBehaviour
{
    public GameObject panel;       // Açýlacak panel objesi
    public Button openButton;      // Paneli açan buton
    public Button closeButton;     // Paneli kapatan buton

    private Color startColor = new Color(1f, 1f, 1f, 0.7f);       // Baþlangýç rengi (Beyaz, %70 þeffaf)
    private Color targetColor = new Color(0.83f, 0.83f, 0.59f, 0.50f); // Hedef renk (D4D397, %70 þeffaf)
    private bool isFadingToTarget = true;                          // Renk geçiþ yönü

    void Start()
    {
        // Baþlangýçta paneli kapalý tut
        panel.SetActive(false);

        // Buton týklama olaylarýný baðla
        openButton.onClick.AddListener(OpenPanel);
        closeButton.onClick.AddListener(ClosePanel);

        // Renk geçiþi baþlat
        StartCoroutine(AnimateButtonColor());
    }

    void OpenPanel()
    {
        panel.SetActive(true);
        StartCoroutine(AnimateButtonColor()); // Panel açýldýðýnda renk geçiþine baþla
    }

    void ClosePanel()
    {
        panel.SetActive(false);
        StopCoroutine(AnimateButtonColor()); // Panel kapandýðýnda renk geçiþini durdur
    }

    IEnumerator AnimateButtonColor()
    {
        Image buttonImage = openButton.GetComponent<Image>(); // Butonun görüntü bileþeni

        while (true) // Sonsuz döngü, renk geçiþi devam eder
        {
            // Renkleri deðiþtir
            float duration = 1f;
            float elapsed = 0f;

            Color fromColor = isFadingToTarget ? startColor : targetColor;
            Color toColor = isFadingToTarget ? targetColor : startColor;

            while (elapsed < duration)
            {
                // Sadece renk tonunu deðiþtirirken alfa (þeffaflýk) sabit tutulur
                Color currentColor = Color.Lerp(fromColor, toColor, elapsed / duration);
                currentColor.a = 0.50f; // Alfa deðerini %70 olarak sabit tut
                buttonImage.color = currentColor;

                elapsed += Time.deltaTime;
                yield return null;
            }

            buttonImage.color = toColor;
            isFadingToTarget = !isFadingToTarget; // Yönü ters çevir
        }
    }
}
