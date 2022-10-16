using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour  
{
    public Button[] lvls;
    public Text coinText;
    public Slider musicSlider, soundSlider;   //это дл€ того чтобы прибавл€ть и убавл€ть музыку и громкость
    public Text musicText, soundText;   //чтобы мен€лс€ текст в ползунках

    void Start()
    {
        if (PlayerPrefs.HasKey("Lvl"))                 //это дл€ того чтобы скрывалс€ Intectable, чтобы был не доступен новый уровень без прохождени€
            for (int i = 0; i < lvls.Length; i++)
            {
                if (i <= PlayerPrefs.GetInt("Lvl"))
                        lvls[i].interactable = true;
                else 
                    lvls[i].interactable = false;
            }
        if (!PlayerPrefs.HasKey("hp"))
            PlayerPrefs.SetInt("hp", 0);
        if (!PlayerPrefs.HasKey("bg"))
            PlayerPrefs.SetInt("bg", 0);
        if (!PlayerPrefs.HasKey("gg"))
            PlayerPrefs.SetInt("gg", 0);

        if (!PlayerPrefs.HasKey("MusicVolume"))   //если ключа сохранени€ музыки нет, то мы будем записывать
            PlayerPrefs.SetInt("MusicVolume", 3); 
        if (!PlayerPrefs.HasKey("SoundVolume"))
            PlayerPrefs.SetInt("SoundVolume", 6);

        musicSlider.value = PlayerPrefs.GetInt("MusicVolume");  //сохранена€ позици€ слайдера на определнной точке поставленной раннее
        soundSlider.value = PlayerPrefs.GetInt("SoundVolume");
    }

    void Update()
    {
        PlayerPrefs.SetInt("MusicVolume", (int)musicSlider.value);  //тут мы переписываем ключи на новые, когда помен€ли ползунки
        PlayerPrefs.SetInt("SoundVolume", (int)soundSlider.value);
        musicText.text = musicSlider.value.ToString();
        soundText.text = soundSlider.value.ToString();   //дл€ того чтобы текс цифр в ползунке мен€лс€


        if (PlayerPrefs.HasKey("coins"))
            coinText.text = PlayerPrefs.GetInt("coins").ToString();
        else
            coinText.text = "0";
    }

    public void OpenScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void DelKeys()   //удал€ет все ключи
    {
        PlayerPrefs.DeleteAll();
    }

    public void Buy_hp(int cost)      //метод покупки сердечек - cost будет означать цену сердечек
    {
        if (PlayerPrefs.GetInt("coins") >= cost)
        {
            PlayerPrefs.SetInt("hp", PlayerPrefs.GetInt("hp") + 1);
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") - cost);
        }
    }

    public void Buy_bg(int cost)      //метод покупки гем - cost будет означать цену сердечек
    {
        if (PlayerPrefs.GetInt("coins") >= cost)
        {
            PlayerPrefs.SetInt("bg", PlayerPrefs.GetInt("bg") + 1);
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") - cost);
        }
    }

    public void Buy_gg(int cost)      //метод покупки гем - cost будет означать цену сердечек
    {
        if (PlayerPrefs.GetInt("coins") >= cost)
        {
            PlayerPrefs.SetInt("gg", PlayerPrefs.GetInt("gg") + 1);
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") - cost);
        }
    }
}
