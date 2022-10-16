using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Btn : MonoBehaviour   // это скрпит на кнопку для того чтобы открыть какое нибудб припяствие
{
    public GameObject[] block;
    public Sprite btnDown;     //означает когда кнопка нажата




    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "MarkBox")
        {
            GetComponent<SpriteRenderer>().sprite = btnDown;
            GetComponent<CircleCollider2D>().enabled = false;  // коллайдер делаем неактивным
            foreach (GameObject obj in block)
            {
                Destroy(obj);
            }
        }
    }
}
