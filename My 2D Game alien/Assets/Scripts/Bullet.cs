using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float speed = 3f;
    float TimeToDisable = 10f;

    void Start()
    {
        StartCoroutine(SetDisabled());
    }

    void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);
    }

    IEnumerator SetDisabled()
    {
        yield return new WaitForSeconds(TimeToDisable);
        gameObject.SetActive(false);   //это метод скрывает бомбы
    }

    private void OnCollisionEnter2D(Collision2D collision)    //этот метод для того чтобы которые попали на припятствие пропадали
    {
        StopCoroutine(SetDisabled());
        gameObject.SetActive(false);
    }
}
