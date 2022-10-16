using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyPlatform : MonoBehaviour   // скрипт для летящей дорожки, когда мы ее касаемся она движется
{
    public Transform[] points;
    public float speed = 1f;
    int i = 1;

    
    void Start()
    {
        transform.position = new Vector3(points[0].position.x, points[0].position.y, transform.position.z);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
             float posX = transform.position.x;      // это для того чтобы стоя на платформе, персонаж не падал
             float posY = transform.position.y;      //

            transform.position = Vector3.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);  // чтобы двигалась платф

            collision.gameObject.transform.position = new Vector3(collision.gameObject.transform.position.x + transform.position.x - posX,
                collision.gameObject.transform.position.y + transform.position.y - posY, collision.gameObject.transform.position.z); // не падает

            if (transform.position == points[i].position)   // движение к точке
            {
                if (i < points.Length - 1)
                    i++;
                else
                    i = 0;
            }
        }
    }
}
