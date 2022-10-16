using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed;
    public float jumpHeight;
    public Transform groundCheck;
    bool isGrounded;
    Animator anim; // анимация персонажа
    int curHp;    // текущее здоровье
    int maxHp = 3;
    bool isHit = false;  // в ударе
    public Main main;
    public bool key = false;    // есть ли у нас улюч или нет
    bool canTP = true;   // это про телепорт от двери к двери
    public bool inWater = false;  //мы не на воде
    bool isClimbing = false;  // лестница,анимация 
    int coins = 0;    // монеты
    bool canHit = true;  // могут ли бить нас врагам или нет - Враги могут нам с бить
    public GameObject blueGem, greenGem;   // это кристалы гемы - их отображение
    int gemCount = 0;   // бонусы персонажа
    float hitTimer = 0f;    //таймер удара, это для лавы
    public Image PlayerCountdown;
    float insideTimer = -1f;    //для кнопки, засекаем время и проходим припяствие
    public float insideTimeUp = 10f;   //для оптимизации,засекает время нажатии кнопки припятствии
    public Image insideCountdown;   //само название кнопки
    public Inventory inventory;   //Инветарь
    public Soundeffector soundeffector;  //звуки клавиш, прыжка и тд

    //public Joystick joystick;  //управление джостиком 


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        curHp = maxHp;
    }

    void Update()
    {
        if (inWater && !isClimbing)
        {
            anim.SetInteger("State", 4); 
            isGrounded = true;
            if(Input.GetAxis("Horizontal") != 0)
                // if (joystick.Horizontal >= 0.3f || joystick.Horizontal <= -0.3f)
             Flip();
        }
        else
        {
            CheckGround();
            if(Input.GetAxis("Horizontal") == 0 && (isGrounded) && !isClimbing)
            //if (joystick.Horizontal < 0.3f && joystick.Horizontal > -0.3f && (isGrounded) && !isClimbing)      // тут вся анимация персонажа,от
                                                                                                                 // ходьбы до прыжка
            {
                anim.SetInteger("State", 1);  //анимация покоя
            }
            else
            {
                Flip();
                if (isGrounded && !isClimbing)
                    anim.SetInteger("State", 2); //ходьбы
            }
            FixedUpdate();
        }
        
        if (insideTimer >= 0f)      // работа проводится, как только мы соприкосаемся с обьектом тега TimerButtonStart
        {
            insideTimer += Time.deltaTime;
            if (insideTimer >= insideTimeUp)
            {
                insideTimer = 0f;
                RecountHp(-1);
            }
            else
                insideCountdown.fillAmount = 1 - (insideTimer / insideTimeUp);
        }
    }

    void FixedUpdate()  // движение игрока
    {
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed,rb.velocity.y);

       // if (joystick.Horizontal >= 0.3f)
         //   rb.velocity = new Vector2(speed, rb.velocity.y);
        //else if (joystick.Horizontal <= -0.3f)
          //  rb.velocity = new Vector2(-speed, rb.velocity.y);
        //else
          //  rb.velocity = new Vector2(0f, rb.velocity.y);
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(transform.up * jumpHeight, ForceMode2D.Impulse);
            soundeffector.PlayJumpSound();
        }
    }

    //public void Jump()
   // {
     //   if (isGrounded)
      //  {
       //     rb.AddForce(transform.up * jumpHeight, ForceMode2D.Impulse);   //тутосуществляется прышок игрока с помощью кнопки
       //     soundeffector.PlayJumpSound();
       // }
    //}

    void Flip()    // движение игрока и поворот игрока
    {
        if (Input.GetAxis("Horizontal") > 0)
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        if (Input.GetAxis("Horizontal") < 0)
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        //if (joystick.Horizontal >= 0.3f)
         //   transform.localRotation = Quaternion.Euler(0, 0, 0);
        //if (joystick.Horizontal <= -0.3f)
          //  transform.localRotation = Quaternion.Euler(0, 180, 0);
    }

    void CheckGround()  // чек что игрок стоит на земле
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.2f);
        isGrounded = colliders.Length > 1;
        if (!isGrounded && !isClimbing)                 // если игрок не на земле, то проигрывается анимация прыжка
            anim.SetInteger("State", 3);  //прыжок
    }

    public void RecountHp(int deltaHp)   // это метод убавление жизни или удар 
    {
        curHp = curHp + deltaHp;
        if (deltaHp < 0 && canHit)
        {
           
            StopCoroutine(OnHit());
            canHit = false;
            isHit = true;
            StartCoroutine(OnHit());
        }
        else if (curHp > maxHp)
        {
            curHp = curHp + deltaHp;
            curHp = maxHp;
        }
            
        if (curHp <= 0)
        {
            GetComponent<CapsuleCollider2D>().enabled = false;
            Invoke("Lose", 1.5f);
        }
    }

    IEnumerator OnHit()   // корутина- онхит- при ударе переводится   здесь при ударе персонажа краснеет
    {
        if(isHit)
            GetComponent<SpriteRenderer>().color = new Color(1f, GetComponent<SpriteRenderer>().color.g - 0.04f, GetComponent<SpriteRenderer>().color.b - 0.04f);
        else
            GetComponent<SpriteRenderer>().color = new Color(1f, GetComponent<SpriteRenderer>().color.g + 0.04f, GetComponent<SpriteRenderer>().color.b + 0.04f);

        if (GetComponent<SpriteRenderer>().color.g - 0.02f == 1f)
        {
            StopCoroutine(OnHit());
            canHit = true;
        }

        if (GetComponent<SpriteRenderer>().color.g - 0.02f <= 0)
            isHit = false;

        yield return new WaitForSeconds(0.02f);
        StartCoroutine(OnHit());
    }

    void Lose()   //терять
    {
        main.GetComponent<Main>().Lose();
    }

    private void OnTriggerEnter2D(Collider2D collision) // это про дверь, тоесть если ключ подобрал,то дверь открывается и телепоритрует
                                                        // в другую дверь
    {
        if (collision.gameObject.tag == "Key")      // тут берем ключ
        {
            Destroy(collision.gameObject);
            key = true;
            inventory.Add_key();      //вызов метода Inventory
        }

        if(collision.gameObject.tag == "Door")   // тут открываем дверь и срабатывает телепорт
        {
            if (collision.gameObject.GetComponent<Door>().isOpen && canTP)
            {
                collision.gameObject.GetComponent<Door>().Teleport(gameObject);
                canTP = false;
                StartCoroutine(TPwait());
            }
            else if (key)
                collision.gameObject.GetComponent<Door>().Unlock();
        }

        if (collision.gameObject.tag == "Coin")      // тут берем монетку
        {
            Destroy(collision.gameObject);
            coins++;
            soundeffector.PlayCoinSound();
        }

        if (collision.gameObject.tag == "Heart")      // тут берем сердечко и она повышает здоровье через Recounthp
        {
            Destroy(collision.gameObject);
            //RecountHp(1);
            inventory.Add_hp();
        }


        if (collision.gameObject.tag == "Mushroom")      // тут берем уже гриб красный, который травит -1
        {
            Destroy(collision.gameObject);
            RecountHp(-1);
        }

        if (collision.gameObject.tag == "BlueGem")      // тут берем алмаз синий который дает неуязвимость
        {
            Destroy(collision.gameObject);
            //StartCoroutine(NoHit());
            inventory.Add_bg();
        }

        if (collision.gameObject.tag == "GreenGem")      // тут берем алмаз зеленый который увеличивает скорость 
        {
            Destroy(collision.gameObject);
            //StartCoroutine(SpeedBonus());
            inventory.Add_gg();
        }

        if (collision.gameObject.tag == "TimerButtonStart")  // припятствие кнопка, дается определенное время
        {
            insideTimer = 0f;
        }

        if (collision.gameObject.tag == "TimerButtonStop")  // припятствие кнопка,завершается припятствие
        {
            insideTimer = -1f;
            insideCountdown.fillAmount = 0f; 
        }

    }

    IEnumerator TPwait()
    {
        yield return new WaitForSeconds(1f);
        canTP = true;
    }

    private void OnTriggerStay2D(Collider2D collision)     // это для того чтобы персонаж забирался по лестнице // действие метода при
                                                           // касании тригера
    {
        if (collision.gameObject.tag == "Ladder")
        {
            isClimbing = true;
            rb.bodyType = RigidbodyType2D.Kinematic;      // это для того чтобы персонаж смог подняться по лестнице, отключить
                                                          // гравитацию
            if (Input.GetAxis("Vertical") == 0)
            {
                anim.SetInteger("State", 5);
            }
            else
            {
                anim.SetInteger("State", 6);
                transform.Translate(Vector3.up * Input.GetAxis("Vertical") * speed * 0.5f * Time.deltaTime);
            }
        }

        if (collision.gameObject.tag == "Icy")   // для скольжении персонажа
        {
            if (rb.gravityScale == 1f)
            {
                rb.gravityScale = 8f;
                speed *= 0.25f;
            }

        }
        if (collision.gameObject.tag == "Lava")   // Лава
        {
            hitTimer += Time.deltaTime;
            if (hitTimer >= 3f)
            {
                hitTimer = 0f;
                PlayerCountdown.fillAmount = 1f;
                RecountHp(-1);
            }
            else
                PlayerCountdown.fillAmount = 1 - (hitTimer / 3f);

        }


    }
    private void OnTriggerExit2D(Collider2D collision)   // этот метод работает при окончании касании объекта с тригером
    {
        if (collision.gameObject.tag == "Ladder")
        {
            isClimbing = false;
            rb.bodyType = RigidbodyType2D.Dynamic;  // а это для того чтобы как персонаж вышел с лестнице включить гравитацию и придать
                                                    // давление вниз
        }

        if (collision.gameObject.tag == "Icy")   // для окончании скольжении персонажа
        {
            if (rb.gravityScale == 8f)
            {
                rb.gravityScale = 1f;
                speed *= 4f;
            }
        }
        if (collision.gameObject.tag == "Lava")
        {
            hitTimer = 0f;
            PlayerCountdown.fillAmount = 0f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)  // это для того чтобы срабатывала анимация батута
    {
        if (collision.gameObject.tag == "Trampoline")    // обращаемся к батуту
            StartCoroutine(TrampolineAnim(collision.gameObject.GetComponentInParent<Animator>()));  // стар корутины батута
        if (collision.gameObject.tag == "Quicksand")   // мы обращаемся к грайнд для того чтобы сделать зыбучие пески, сделать
                                                       // тяжелее и убрать прыгучесть
        {
            speed *= 0.25f;
            rb.mass *= 100f;
        }
    }

    IEnumerator TrampolineAnim(Animator an)  // тут идет корутина батута
    {
        an.SetBool("isJump", true);
        yield return new WaitForSeconds(0.5f);
        an.SetBool("isJump", false);
    }

    IEnumerator NoHit()    // нет удара по персонажу, неуязвимость
    {
        gemCount++;
        blueGem.SetActive(true);  //делаем активным неуязвимость.
        CheckGems(blueGem);

        canHit = false;  
        blueGem.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);  // вначале цвет гема равен 1
        yield return new WaitForSeconds(4f);   // тут мы ждем
        StartCoroutine(Invis(blueGem.GetComponent<SpriteRenderer>(), 0.02f));  // корутина плавного изчезновения гема, частота 50кадров в мин
        yield return new WaitForSeconds(1f);  // ждем
        canHit = true;   

        gemCount--;
        blueGem.SetActive(false);
        CheckGems(greenGem);
    }

    IEnumerator SpeedBonus()
    {
        gemCount++;
        greenGem.SetActive(true);
        CheckGems(greenGem);

        speed = speed * 2;
        greenGem.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        yield return new WaitForSeconds(4f);
        StartCoroutine(Invis(greenGem.GetComponent<SpriteRenderer>(), 0.02f));
        yield return new WaitForSeconds(1f);
        speed = speed / 2;

        gemCount--;
        greenGem.SetActive(false);
        CheckGems(blueGem);
    }

    void CheckGems (GameObject obj)  // этот метод проверяет в какой позиции должны стоять наши гемы алмазы
    {
        if (gemCount == 1)
            obj.gameObject.transform.localPosition = new Vector3(0f, 0.6f, obj.transform.localPosition.z);
        else if (gemCount == 2)
        {
            blueGem.gameObject.transform.localPosition = new Vector3(-0.5f, 0.5f, blueGem.transform.localPosition.z);
            greenGem.gameObject.transform.localPosition = new Vector3(0.5f, 0.5f, greenGem.transform.localPosition.z);
        }

    }

    IEnumerator Invis(SpriteRenderer spr, float time)    //корутина которая будет делать наши гемы постепенно невидимыми, сколько по
                                                         //времени
                                                         //наш гем будет переходить в прозрачное
    {
        spr.color = new Color(1f, 1f, 1f, spr.color.a - time * 2);
        yield return new WaitForSeconds(time);
        if (spr.color.a > 0)
            StartCoroutine(Invis(spr, time));
    }

    private void OnCollisionExit2D(Collision2D collision)  // этот метод работает,когда касание прекращается
    {
        if (collision.gameObject.tag == "Quicksand")   // мы обращаемся к грайнд для того чтобы его прыгучесть вернуть в обратное
                                                       // состояние
        {
            speed *= 4f;
            rb.mass *= 0.01f;
        }

    }

    public int GetCoins()   //возвращаем монеты Main
    {
        return coins;
    }
    public int GetHP()   //здоровье Main
    {
        return curHp;
    }

    public void BlueGem()   //метод вызывает корутину неуязвимости, чтобы мы могли вызывать из других классов
    {
        StartCoroutine(NoHit());
    }
    public void GreenGem()   //метод вызывает корутину неуязвимости, чтобы мы могли вызывать из других классов
    {
        StartCoroutine(SpeedBonus());
    }

}
