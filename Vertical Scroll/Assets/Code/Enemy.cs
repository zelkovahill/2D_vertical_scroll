using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
   public float speed;
   public float health; 
   public Sprite[] sprites;
   
   private SpriteRenderer _spriteRenderer;
   private Rigidbody2D _rb;

   void Awake()
   {
      _spriteRenderer = GetComponent<SpriteRenderer>();
      _rb = GetComponent<Rigidbody2D>();
      _rb.velocity = Vector2.down * speed;
   }

   void OnHit(int dmg)
   {
      health -= dmg;
      _spriteRenderer.sprite = sprites[1];
      Invoke(nameof(ReturnSprite), 0.1f);

      if (health <= 0)
      {
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
