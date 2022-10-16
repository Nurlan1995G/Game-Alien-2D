using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour   // Класс Финиш
{
    public Main main;
    public Sprite finSprite;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GetComponent<SpriteRenderer>().sprite = finSprite;
            main.Win();
        }
    }

}
