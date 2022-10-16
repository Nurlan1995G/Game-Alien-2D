using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touches : MonoBehaviour
{
   
    void Update()   //передвижение персонажа с помощью касания на экран
    {
        if (Input.touchCount > 0)   //количество касаний
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position); //мировые координаты
            if (touchPos.x > Camera.main.transform.position.x)
                transform.position = new Vector3(5f, 0f, 0f);
            else
                transform.position = new Vector3(-5f,0f,0f);
        }   
    }
}
