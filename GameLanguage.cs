using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLanguage : MonoBehaviour
{

	public static GameLanguage gl;
	public string currentLanguage = "tr";

	Dictionary<string, string> langID;

	// Start is called before the first frame update
	void Start()
	{
		gl = this;






		if (PlayerPrefs.HasKey("GameLanguage"))
		{
			currentLanguage = PlayerPrefs.GetString("GameLanguage");
		}
		else
		{
			ResetLanguage();
		}



		WordDefine();
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void Setlanguage(string langCode)
	{

		PlayerPrefs.SetString("GameLanguage", langCode);
		currentLanguage = langCode;
	}





	public void ResetLanguage()
	{
		Setlanguage("tr");

	}

	public string Say(string text)
	{

		switch (currentLanguage)
		{
			case "id":
				return FindInDict(langID, text);
			default:
				return text;
		}
	}

	public string FindInDict(Dictionary<string, string> selectedLang, string text)
	{
		if (selectedLang.ContainsKey(text))
			return selectedLang[text];
		else
			return "Untranslated";
	}

	public void WordDefine()
	{

		//Bahasa Indonesia (id)
		// ingilizce
		langID = new Dictionary<string, string>()
		{
			{"OYNA", "PLAY"},
			{"Kapat", "Close"},
			{"Enerji Al", "Take Energy"},
			{"ENERJ� B�TT�", "ENERGY IS OVER"},
			{"Ama merak etme test s�recinde s�n�rs�z enerji yenileyebilirsin", "But dont worry you can add unlimited energy"},
			{"Alt�nlar� Topla", "Tap to claim"},
			{"2x Alt�n (Reklam)", "2X GOLD (Ad)"},
			{"M�zik", "MUSIC"},
			{"Ses", "SOUND"},
			{"Titre�im", "VIBRATION"},
			{"Dil", "LANGUAGE"},
			{"Sakin ol..", "Calm down.."},
			{"S�re", "Timer"},
			{"Hedef", "Goals"},
			{"Tekrar Oyna", "Try Again"},
			{"kaybettin!", "GAME OVER"},
			{"S�radaki Seviye", "Next Level"},
			{"KAZANDIN!", "LEVEL WIN"},
			{"Men�ye D�n", "Back Menu"},
			{"Devam Et", "Continue"},
			{"Mola", "Pause"},
			{"Ileri", "Next"},
			{"Ba�la!", "Start!"},
			{"Oyun icerisinde s�reye dikkat etmelisin. �lk seviyeler kolaydir fakat sonralari fazlasiyla zor..", "You should pay attention to the time in the game. The first levels are easy, but the later ones are extremely difficult.."},
			{"Birbirinden farkli ozel yetenekler ile oyunu kolaylastirabilirsin", "You can make the game easier with different special abilities."},
			{"Se�ilen objeler alt kisimda siralanir ve ayni nesneden 3 tane oldugunda birlesip yokolur.", "The selected objects are listed at the bottom and when there are 3 of the same object, they merge and disappear."},
			{"Eger 7 adet nesne icerisinde 3l� eslesme olmaz ise oyun kaybedilir", "If there is no match-3 among 7 objects, the game is lost."},
			{"Gorevin olan nesneleri ekrandan tiklayip secmen gerekiyor.", "You need to click and select the objects that are your task on the screen."},
			{"Buradan g�revin olan nesneleri g�rebilirsin.", "You need to click and select the goal objects on the screen."},
			{"Yetenek Al (Reklam)", "Get Skill (Ad)"},
			{"Yetenek kullan�mlar�n bitti.", "Your skill usage is over."},
			{"Reklam izleyerek 3 adet yetenek alabilirsin", "You can get more with a short ad"},
			{"Yak�nda f�r�n�n kap�s� a��lacak ve alt�nlar�n� harcamak i�in bolca vaktin olacak!", "Soon the bakery door will open and you will have plenty of time to spend your gold!"}
		};
	}
}
