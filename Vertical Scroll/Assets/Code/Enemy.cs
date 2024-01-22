using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
   public int enemyScore;
   public string Enemyname;
   public float speed;
   public float health; 
   public Sprite[] sprites;

   public float maxShotDelay;
   public float curShotDelay;
   
   public GameObject bulletObjA;
   public GameObject bulletObjB;
   public GameObject player;
   
   private SpriteRenderer _spriteRenderer;
   

   void Awake()
   {
      _spriteRenderer = GetComponent<SpriteRenderer>();
     
   }

   void Update()
   {
      Fire();
      Reload();
   }

   void Fire()
   {
      if(curShotDelay<maxShotDelay)
         return;

      if (Enemyname == "S")
      {
         GameObject bullet = Instantiate(bulletObjA, transform.position, transform.rotation);
         Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();


         Vector3 dirVec = player.transform.position - transform.position;
         rb.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);

      }
      
      else if (Enemyname == "L")
      {
         GameObject bulletL = Instantiate(bulletObjA, transform.position+Vector3.left*0.3f, transform.rotation);
         GameObject bulletR = Instantiate(bulletObjA, transform.position+Vector3.right*0.3f, transform.rotation);
         
         Rigidbody2D rbL = bulletL.GetComponent<Rigidbody2D>();
         Rigidbody2D rbR = bulletR.GetComponent<Rigidbody2D>();
         
         Vector3 dirVecL = player.transform.position - (transform.position+Vector3.left*0.3f);
         Vector3 dirVecR = player.transform.position - (transform.position+Vector3.right*0.3f);
         
         rbL.AddForce(dirVecL.normalized * 4, ForceMode2D.Impulse);
         rbR.AddForce(dirVecR.normalized * 4, ForceMode2D.Impulse);
         
      }
      
      curShotDelay = 0;
   }

   void Reload()
   {
      curShotDelay+=Time.deltaTime;
   }
   

   void OnHit(int dmg)
   {
      health -= dmg;
      _spriteRenderer.sprite = sprites[1];
      Invoke(nameof(ReturnSprite), 0.1f);

      if (health <= 0)
      {
         Player playerLogic = player.GetComponent<Player>();
         playerLogic.score += enemyScore;
         Destroy(gameObject);
      }
   }

   void ReturnSprite()
   {
      _spriteRenderer.sprite = sprites[0];
   }

   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.gameObject.CompareTag("BorderBullet"))
      {
         Destroy(gameObject);
      }
      else if (other.gameObject.CompareTag("PlayerBullet")) 
      {
         Bullet bullet = other.gameObject.GetComponent<Bullet>();
         OnHit(bullet.dmg);
         
         Destroy(other.gameObject);
      }
   }
}
