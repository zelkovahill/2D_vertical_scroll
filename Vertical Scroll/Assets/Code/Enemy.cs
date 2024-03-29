
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
   public GameObject itemCoin;
   public GameObject itemPower;
   public GameObject itemBoom;
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
   

   public void OnHit(int dmg)
   {
      if(health<=0)
         return;
         
      health -= dmg;
      _spriteRenderer.sprite = sprites[1];
      Invoke(nameof(ReturnSprite), 0.1f);

      if (health <= 0)
      {
         Player playerLogic = player.GetComponent<Player>();
         playerLogic.score += enemyScore;
         
         // # Random Ratio Item Drop
         int ran = Random.Range(0, 10);
         if (ran < 3) // Not Item 30%
         {
            
         }
         else if (ran < 6) // Coin 30%
         {
            Instantiate(itemCoin, transform.position, itemCoin.transform.rotation);
            
         }
         
         else if (ran < 8) // Power 20%
         {
            Instantiate(itemPower, transform.position, itemPower.transform.rotation);
         }
         
         else if (ran < 10) // Boom 20%
         {
            Instantiate(itemBoom, transform.position, itemBoom.transform.rotation);
         }
       
         
         
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
