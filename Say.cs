using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Say : MonoBehaviour
{
    public string defaultText = "Default Text";

    private Text currentText;           // Normal Unity Text bileþeni
    private TextMeshProUGUI tmpText;     // TextMeshPro bileþeni

    void Start()
    {
        // Hem Text hem de TextMeshPro bileþenlerini kontrol et
        currentText = GetComponent<Text>();
        tmpText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        // Çevirilmiþ metni alýn
        string translatedText = GameLanguage.gl.Say(defaultText);

        // Eðer normal Text bileþeni varsa, ona çeviriyi uygula
        if (currentText != null)
        {
            currentText.text = translatedText;
        }
        // Eðer TextMeshPro bileþeni varsa, ona çeviriyi uygula
        else if (tmpText != null)
        {
            tmpText.text = translatedText;
        }
    }
}
