using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    bool isHit = false;
    public GameObject drop;   //для того чтобы зеленый червяк, при его убийстве, давали гем скорости зеленый
    private void OnCollisionEnter2D(Collision2D collision)  //переводится как столкновение двух объектов
    {
        if (collision.gameObject.tag == "Player" && !isHit)
        {
            collision.gameObject.GetComponent<Player>().RecountHp(-1);
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * 8f, ForceMode2D.Impulse);  //это импуль для точка,
                                                                                                                //подходит
                                                                                                                //как для стражении
                                                                                                                //врагов
        }
    }

    public IEnumerator Death()
    {
        if(drop != null)                                // условие получения гема
        {
            Instantiate(drop, transform.position, Quaternion.identity);
        }
        isHit = true;
        GetComponent<Animator>().SetBool("dead", true);
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;  // когда мы будем убивать нашего врага он будет падать вниз
        GetComponent<Collider2D>().enabled = false;                     // это для того чтобы когда умер враг, у него отключился
                                                                        // коллайдер
        transform.GetChild(0).GetComponent<Collider2D>().enabled = false;   //теперь мы должны обратиться к компоненты нашего ребенка
                                                                            //к нашему
                                                                            
                                                                            //коллайдеру,которого мы ударяем - и удаляем его
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    public void startDeath()
    {
        StartCoroutine(Death());
    }

}
