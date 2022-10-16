using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public Player player;
    public Text cointext;      // ��� ��� ����������� ����������������� ����������
    public Image[] hearts;
    public Sprite isLife, nonLife;    //��������� ����� � ���������� �����
    public GameObject PauseScreen;    //��� � ���� ������� ��� ���� ����� ��� ������� ����� ��� ��������
    
    public GameObject WinScreen;   //��� ������ 
    public GameObject LoseScreen;   //��� ����������� ����� ������ 
    float timer = 0f;       //�����
    public Text timeText;
    public TimeWork timeWork;  //��� �� ���� ������, ��� ����� �������� - ������ �������
    public float coundown;  //����� ����������
    public GameObject inventoryPan;   //�������� ������ ���������
    public Soundeffector soundeffector;
    public AudioSource musicSource, soundSource; //����� ��������� � ���� �������� ����� ��������� � �������� ��������
    

    public void ReloadLvl()   // � �������� ��������.��� ���� ����� ������������� �������
    {
        Time.timeScale = 1f;     // � ������� timeScale �� ����� ��������� � �������� �����
        player.enabled = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Start()
    {
        musicSource.volume = (float)PlayerPrefs.GetInt("MusicVolume") / 10;  //����� ��������� � ���� �������� ����� ��������� � �������� ��������
        soundSource.volume = (float)PlayerPrefs.GetInt("SoundVolume") / 10;

        if ((int)timeWork == 2)
            timer = coundown;
    }

    private void Update()
    {
        cointext.text = player.GetCoins().ToString();  //����� ToString �� ��������������� � ��������� ��� ������

        for (int i = 0; i < hearts.Length; i++)   // ���� for ��� ���� ����� �������� ������������
        {
            if (player.GetHP() > i)
                hearts[i].sprite = isLife;
            else
                hearts[i].sprite = nonLife;
        }

        if ((int)timeWork == 1)  
        {
            timer += Time.deltaTime;
            timeText.text = timer.ToString("F2").Replace(",", ":"); // � ���� ����������� ������������ ����� � ����������� �� ��������-��������
                                                                    // ������, F2 - ��� ����� ������� 2 �����, � Replace - ��� ������ , �� :
        }
        else if ((int)timeWork == 2)  
        {
            timer -= Time.deltaTime;
            //timeText.text = timer.ToString("F2").Replace(",", ":");
            timeText.text = ((int)timer / 60).ToString() + ":" + ((int)timer - ((int)timer / 60) * 60).ToString("D2");   //��� ���� ����� 100 ������
                                                                                                                     //�������� ��� 1��� 40���
                                                                                                                     //D2 ��� ����������� ���������
                                                                                                                     //tostring ��� �����
            if (timer <= 0)
                Lose();
        }
        else
            timeText.gameObject.SetActive(false);
    }

    public void PauseOn()
    {
        Time.timeScale = 0f;    //����� ���������� � ����
        player.enabled = false;   // �������������� ������
        PauseScreen.SetActive(true);  // ����� ������ ������� ��������
    }

    public void PauseOff()
    {
        Time.timeScale = 1f;    
        player.enabled = true;   
        PauseScreen.SetActive(false);  
    }

    public void Win()  // ����� ��� ������, ����������� � Finish
    {
        soundeffector.PlayWinSound();
        Time.timeScale = 0f;
        player.enabled = false;
        WinScreen.SetActive(true);

        if (!PlayerPrefs.HasKey("Lvl") || PlayerPrefs.GetInt("Lvl") < SceneManager.GetActiveScene().buildIndex)
            PlayerPrefs.SetInt("Lvl", SceneManager.GetActiveScene().buildIndex);   // ���������� ������ � ������� PlayerPrefs 
        if (PlayerPrefs.HasKey("coins"))
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + player.GetCoins());  // ��� ��� ���� ����� ��������� �������
        else
            PlayerPrefs.SetInt("coins", player.GetCoins());

        inventoryPan.SetActive(false);    //������ ��������� ��������
        GetComponent<Inventory>().RecountItems();
        
    }

    public void Lose()  // ����� ��� ����������� ����� ������, ����������� � Finish
    {
        soundeffector.PlayLoseSound();
        Time.timeScale = 0f;
        player.enabled = false;
        LoseScreen.SetActive(true);

        inventoryPan.SetActive(false);       //��� ���� �������� ������ ���������
        GetComponent<Inventory>().RecountItems();
    }

    public void MenuLvl()        // ������ ����
    {
        Time.timeScale = 1f;
        player.enabled = true;
        SceneManager.LoadScene("Menu");
    }

    public void NextLvl()    //������ ������ � ����
    {
        Time.timeScale = 1f;
        player.enabled = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);  //������� ����� ���������� �� ����
    }
}

public enum TimeWork   //��� ��� ���� ����� �� ����� ��������� ������� �� ������������ ����� . Enum ��� ������������  ������������� �����
                       //��������� ��������
{
    None,  // ����� �� ���������
    Stopwatch,  // ���������
    Timer  //������ �� �����


}
