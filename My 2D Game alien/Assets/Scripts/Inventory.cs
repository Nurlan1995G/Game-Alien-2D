using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    int hp = 0, bg = 0, gg = 0;     
    public Sprite[] numbers;   // массив чисел у Image
    public Sprite is_hp, no_hp, is_bg, no_bg, is_gg, no_gg, is_key, no_key;   //отвечает за то что гем явный или нет
    public Image hp_img, bg_img, gg_img, key_img;      // исправляять с помощью переменных дочерних Image, все сердечки,гемы и ключ
    public Player player;


    private void Start()     //это для того чтобы сохранения в меню передавались после в уровень
    {
        if (PlayerPrefs.GetInt("hp") > 0)
        {
            hp = PlayerPrefs.GetInt("hp");
            hp_img.sprite = is_hp;
            hp_img.transform.GetChild(0).GetComponent<Image>().sprite = numbers[hp];
        }
        if (PlayerPrefs.GetInt("bg") > 0)
        {
            bg = PlayerPrefs.GetInt("bg");
            bg_img.sprite = is_bg;
            bg_img.transform.GetChild(0).GetComponent<Image>().sprite = numbers[bg];
        }
        if (PlayerPrefs.GetInt("gg") > 0)
        {
            gg = PlayerPrefs.GetInt("gg");
            gg_img.sprite = is_gg;
            gg_img.transform.GetChild(0).GetComponent<Image>().sprite = numbers[gg];
        }
    }
    public void Add_hp()
    {
        hp++;
        hp_img.sprite = is_hp;
        hp_img.transform.GetChild(0).GetComponent<Image>().sprite = numbers[hp];
    }

    public void Add_bg()
    {
        bg++;
        bg_img.sprite = is_bg;
        bg_img.transform.GetChild(0).GetComponent<Image>().sprite = numbers[bg];
    }

    public void Add_gg()
    {
        gg++;
        gg_img.sprite = is_gg;
        gg_img.transform.GetChild(0).GetComponent<Image>().sprite = numbers[gg];
    }

    public void Add_key()
    {
        key_img.sprite = is_key;   //этот метод приводится в Player OnTriggerEnter2D
    }

    public void Use_hp()   //когда мы кликаем по сердечку,чтобы взять отложенное
    {
        if (hp > 0)
        {
            hp--;
            player.RecountHp(1);
            hp_img.transform.GetChild(0).GetComponent<Image>().sprite = numbers[hp];
            if (hp == 0)
                hp_img.sprite = no_hp;
        }
    }
    public void Use_bg()   //когда мы кликаем по синему гему,чтобы взять отложенное
    {
        if (hp > 0)
        {
            bg--;
            player.BlueGem();
            bg_img.transform.GetChild(0).GetComponent<Image>().sprite = numbers[bg];
            if (bg == 0)
                bg_img.sprite = no_bg;
        }
    }

    public void Use_gg()   //когда мы кликаем по зеленому гему,чтобы взять отложенное
    {
        if (gg > 0)
        {
            gg--;
            player.GreenGem();
            gg_img.transform.GetChild(0).GetComponent<Image>().sprite = numbers[gg];
            if (gg == 0)
                gg_img.sprite = no_gg;
        }
    }

    public void RecountItems()  // создаем метод пересчет предметов,для того чтобы после смерти наши предметы удалялись
    {
        PlayerPrefs.SetInt("hp", hp);
        PlayerPrefs.SetInt("bg", bg);
        PlayerPrefs.SetInt("gg", gg);
    }     

}
