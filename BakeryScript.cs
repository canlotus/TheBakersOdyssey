using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BakeryScript : MonoBehaviour
{
    public GameObject panel;       // A��lacak panel objesi
    public Button openButton;      // Paneli a�an buton
    public Button closeButton;     // Paneli kapatan buton

    private Color startColor = new Color(1f, 1f, 1f, 0.7f);       // Ba�lang�� rengi (Beyaz, %70 �effaf)
    private Color targetColor = new Color(0.83f, 0.83f, 0.59f, 0.50f); // Hedef renk (D4D397, %70 �effaf)
    private bool isFadingToTarget = true;                          // Renk ge�i� y�n�

    void Start()
    {
        // Ba�lang��ta paneli kapal� tut
        panel.SetActive(false);

        // Buton t�klama olaylar�n� ba�la
        openButton.onClick.AddListener(OpenPanel);
        closeButton.onClick.AddListener(ClosePanel);

        // Renk ge�i�i ba�lat
        StartCoroutine(AnimateButtonColor());
    }

    void OpenPanel()
    {
        panel.SetActive(true);
        StartCoroutine(AnimateButtonColor()); // Panel a��ld���nda renk ge�i�ine ba�la
    }

    void ClosePanel()
    {
        panel.SetActive(false);
        StopCoroutine(AnimateButtonColor()); // Panel kapand���nda renk ge�i�ini durdur
    }

    IEnumerator AnimateButtonColor()
    {
        Image buttonImage = openButton.GetComponent<Image>(); // Butonun g�r�nt� bile�eni

        while (true) // Sonsuz d�ng�, renk ge�i�i devam eder
        {
            // Renkleri de�i�tir
            float duration = 1f;
            float elapsed = 0f;

            Color fromColor = isFadingToTarget ? startColor : targetColor;
            Color toColor = isFadingToTarget ? targetColor : startColor;

            while (elapsed < duration)
            {
                // Sadece renk tonunu de�i�tirirken alfa (�effafl�k) sabit tutulur
                Color currentColor = Color.Lerp(fromColor, toColor, elapsed / duration);
                currentColor.a = 0.50f; // Alfa de�erini %70 olarak sabit tut
                buttonImage.color = currentColor;

                elapsed += Time.deltaTime;
                yield return null;
            }

            buttonImage.color = toColor;
            isFadingToTarget = !isFadingToTarget; // Y�n� ters �evir
        }
    }
}
