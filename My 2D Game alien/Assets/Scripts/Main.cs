using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public Player player;
    public Text cointext;      // это все отобращение пользовательского интерфейса
    public Image[] hearts;
    public Sprite isLife, nonLife;    //вкюченная жизнь и отключеная жизнь
    public GameObject PauseScreen;    //это к двум методом для того чтобы при нажатии паузы все работало
    
    public GameObject WinScreen;   //Для финиша 
    public GameObject LoseScreen;   //Для перезапуска после смерти 
    float timer = 0f;       //тимер
    public Text timeText;
    public TimeWork timeWork;  //это по сути список, где можно выбирать - работа времени
    public float coundown;  //время исчесления
    public GameObject inventoryPan;   //содержит панель инвентаря
    public Soundeffector soundeffector;
    public AudioSource musicSource, soundSource; //чтобы показания в меню настроек звука совподали с игровыми уровнями
    

    public void ReloadLvl()   // в переводе проигрыш.Для того чтобы перезапустить уровень
    {
        Time.timeScale = 1f;     // с помощью timeScale мы можем замедлять и ускорять время
        player.enabled = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Start()
    {
        musicSource.volume = (float)PlayerPrefs.GetInt("MusicVolume") / 10;  //чтобы показания в меню настроек звука совподали с игровыми уровнями
        soundSource.volume = (float)PlayerPrefs.GetInt("SoundVolume") / 10;

        if ((int)timeWork == 2)
            timer = coundown;
    }

    private void Update()
    {
        cointext.text = player.GetCoins().ToString();  //через ToString мы преобразовываем в строковый тип данных

        for (int i = 0; i < hearts.Length; i++)   // цикл for для того чтобы сердечки отображались
        {
            if (player.GetHP() > i)
                hearts[i].sprite = isLife;
            else
                hearts[i].sprite = nonLife;
        }

        if ((int)timeWork == 1)  
        {
            timer += Time.deltaTime;
            timeText.text = timer.ToString("F2").Replace(",", ":"); // в этом обозначении определяется время и переводится на текстово-цифровой
                                                                    // формат, F2 - это после запятой 2 цифры, а Replace - это замена , на :
        }
        else if ((int)timeWork == 2)  
        {
            timer -= Time.deltaTime;
            //timeText.text = timer.ToString("F2").Replace(",", ":");
            timeText.text = ((int)timer / 60).ToString() + ":" + ((int)timer - ((int)timer / 60) * 60).ToString("D2");   //для того чтобы 100 секунд
                                                                                                                     //читалось как 1мин 40сек
                                                                                                                     //D2 это специальный компонент
                                                                                                                     //tostring две цифры
            if (timer <= 0)
                Lose();
        }
        else
            timeText.gameObject.SetActive(false);
    }

    public void PauseOn()
    {
        Time.timeScale = 0f;    //время прировнять к нулю
        player.enabled = false;   // диактивировать игрока
        PauseScreen.SetActive(true);  // Паузы панель сделать активной
    }

    public void PauseOff()
    {
        Time.timeScale = 1f;    
        player.enabled = true;   
        PauseScreen.SetActive(false);  
    }

    public void Win()  // метод для финиша, продолжение в Finish
    {
        soundeffector.PlayWinSound();
        Time.timeScale = 0f;
        player.enabled = false;
        WinScreen.SetActive(true);

        if (!PlayerPrefs.HasKey("Lvl") || PlayerPrefs.GetInt("Lvl") < SceneManager.GetActiveScene().buildIndex)
            PlayerPrefs.SetInt("Lvl", SceneManager.GetActiveScene().buildIndex);   // сохранение уровня с помощью PlayerPrefs 
        if (PlayerPrefs.HasKey("coins"))
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + player.GetCoins());  // это для того чтобы сохранять монетки
        else
            PlayerPrefs.SetInt("coins", player.GetCoins());

        inventoryPan.SetActive(false);    //панель инвенторя скрывать
        GetComponent<Inventory>().RecountItems();
        
    }

    public void Lose()  // метод для перезапуска после смерти, продолжение в Finish
    {
        soundeffector.PlayLoseSound();
        Time.timeScale = 0f;
        player.enabled = false;
        LoseScreen.SetActive(true);

        inventoryPan.SetActive(false);       //для того скрывать панель инвентаря
        GetComponent<Inventory>().RecountItems();
    }

    public void MenuLvl()        // запуск меню
    {
        Time.timeScale = 1f;
        player.enabled = true;
        SceneManager.LoadScene("Menu");
    }

    public void NextLvl()    //запуск уровня в игре
    {
        Time.timeScale = 1f;
        player.enabled = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);  //текущую сцену прибавляем на один
    }
}

public enum TimeWork   //это для того чтобы мы могли проходить уровень за поставленное время . Enum это перечисление  предаставляет набор
                       //связанных констант
{
    None,  // время не считается
    Stopwatch,  // секудомер
    Timer  //таймер на время


}
