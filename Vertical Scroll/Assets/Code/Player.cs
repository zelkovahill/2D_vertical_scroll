using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Player : MonoBehaviour
{
    public int life;
    public int score;
    public float speed;
    public int power;
    public int maxPower;
    public int boom;
    public int maxBoom;
    public float maxShotDelay;
    public float curShotDelay;
    
    public bool isTouchTop;
    public bool isTouchBottom;
    public bool isTouchRight;
    public bool isTouchLeft;
    public bool isHit;
    public bool isBoomTime;
    
    public GameObject bulletObjA;
    public GameObject bulletObjB;
    public GameObject BoomEffect;

    public GameManager manager;

    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        Move();
        Fire();
        Boom();
        Reload();

    }

    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        if ((h == 1 && isTouchRight) || (h == -1 && isTouchLeft)) 
            h = 0;
        
        
        float v = Input.GetAxisRaw("Vertical");
        if((v==1 && isTouchTop) || (v == -1 && isTouchBottom))
            v = 0;
      
        Vector3 curPos = transform.position;
        Vector3 nextPos = new Vector3(h, v, 0) * speed * Time.deltaTime;

        transform.position = curPos + nextPos;


        if(Input.GetButtonDown("Horizontal") || Input.GetButtonUp("Horizontal"))
            anim.SetInteger("Input", (int)h);
    }

    void Fire()
    {
        if(!Input.GetButton("Fire1"))
            return;
        
        if(curShotDelay < maxShotDelay)
            return;

        switch (power)
        {
            case 1:
                 GameObject bullet = Instantiate(bulletObjA,transform.position,transform.rotation);
                        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                        rb.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
            case 2:
                GameObject bulletR = Instantiate(bulletObjA,transform.position+Vector3.right*0.1f,transform.rotation);
                GameObject bulletL = Instantiate(bulletObjA,transform.position+Vector3.left*0.1f,transform.rotation);
                Rigidbody2D rbR = bulletR.GetComponent<Rigidbody2D>();
                Rigidbody2D rbL = bulletL.GetComponent<Rigidbody2D>();
                rbR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rbL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
            case 3:
                GameObject bulletRR = Instantiate(bulletObjA,transform.position+Vector3.right*0.35f,transform.rotation);
                GameObject bulletCC = Instantiate(bulletObjB,transform.position,transform.rotation);
                GameObject bulletLL = Instantiate(bulletObjA,transform.position+Vector3.left*0.35f,transform.rotation);
                Rigidbody2D rbRR = bulletRR.GetComponent<Rigidbody2D>();
                Rigidbody2D rbCC = bulletCC.GetComponent<Rigidbody2D>();
                Rigidbody2D rbLL = bulletLL.GetComponent<Rigidbody2D>();
                rbRR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rbCC.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rbLL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
            
        }
        
        curShotDelay = 0;
    }

    void Reload()
    {
        curShotDelay+= Time.deltaTime;
    }

    void Boom()
    {
        if(!Input.GetButton("Fire2"))
            return;

        if (isBoomTime)
            return;
        
        if(boom==0)
            return;

        boom--;
        isBoomTime = true;
        manager.UpdateBoomIcon(boom);
        
        
            // # 1 . Effect visible
            BoomEffect.SetActive(true);
            Invoke(nameof(OffBoomEffect), 4f);
                    
            // # 2 . Remove Enemy
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            for (int i = 0; i < enemies.Length; i++)
            {
                Enemy enemyLogic = enemies[i].GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }
                    
            // # 3 . Remove Enemy Bullet
            GameObject[] bullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
            for (int i = 0; i < bullets.Length; i++)
            {
                Destroy(bullets[i]);
            }
        
            
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Border"))
        {
            switch (other.gameObject.name)
            {
                case "Top":
                    isTouchTop = true;
                    break;
                case "Bottom":
                    isTouchBottom = true;
                    break;
                case "Right":
                    isTouchRight = true;
                    break;
                case "Left":
                    isTouchLeft = true;
                    break;
            }
        }
        else if(other.gameObject.CompareTag("Enemy")||other.gameObject.CompareTag("EnemyBullet"))
        {
            if (isHit)
            {
                return;
            }

            isHit = true;
            life--;
            manager.UpdateLifeIcon(life);

            if (life == 0)
            {
                manager.GameOver();
            }
            else
            {
                manager.RespawnPlayer();
            }
            manager.RespawnPlayer();
            gameObject.SetActive(false);
            Destroy(other.gameObject);

        }
        else if (other.gameObject.CompareTag("Item"))
        {
            Item item = other.gameObject.GetComponent<Item>();
            switch (item.type)
            {
                case "Coin":
                    score += 1000;
                    break;
                
                case "Power":
                    if (maxPower == power)
                    {
                        score += 500;
                    }
                    else
                    {
                        power++;
                    }
                    
                    break;
                
                case "Boom":
                    if (boom == maxBoom)
                        score += 500;
                    else
                    {
                        boom++;
                        manager.UpdateBoomIcon(boom);
                    }
                   
                    break;
            }

            Destroy(other.gameObject);
        }
    }
    
    void OffBoomEffect()
    {
        BoomEffect.SetActive(false);
        isBoomTime = false;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Border"))
        {
            switch (other.gameObject.name)
            {
                case "Top":
                    isTouchTop = false;
                    break;
                case "Bottom":
                    isTouchBottom = false;
                    break;
                case "Right":
                    isTouchRight = false;
                    break;
                case "Left":
                    isTouchLeft = false;
                    break;
            }
        }
    }
}
