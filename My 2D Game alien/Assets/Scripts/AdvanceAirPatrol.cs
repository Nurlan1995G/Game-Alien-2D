using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvanceAirPatrol : MonoBehaviour
{
    public Transform[] points;
    public float speed = 2f;
    public float waitTime = 3f;  //время ожидания врага как дойдет до точки
    bool CanGo = true;   //логического типа метод, можем ли мы идти
    int i = 1;
    
    void Start()
    {
        gameObject.transform.position = new Vector3(points[0].position.x, points[0].position.y, transform.position.z);
    }


    void Update()
    {
        if (CanGo)
            transform.position = Vector3.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);

        if (transform.position == points[i].position)
        {
            if (i < points.Length - 1)
                i++;
            else
                i = 0;
            CanGo = false;
            StartCoroutine(Waiting());
        }

    }

    IEnumerator Waiting()    //корутина ожидания
    {
        yield return new WaitForSeconds(waitTime);
      
        CanGo = true;

    }
}
