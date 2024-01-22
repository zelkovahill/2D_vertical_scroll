using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject[] enemyObjs;
    public Transform[] spawnPoints;
    
    public GameObject player;
    public Text scoreText;
    public Image[] lifeImage;
    public GameObject gameOverSet;
    
    public float maxSpawnDelay;
    public float curSpawnDelay;

    void Update()
    {
        curSpawnDelay += Time.deltaTime;

        if (curSpawnDelay > maxSpawnDelay)
        {
            SpawnEnemy();
            maxSpawnDelay = Random.Range(0.5f, 3f);
            curSpawnDelay = 0;
        }
        
        // #. UI Score Update
        Player playerLogic = player.GetComponent<Player>();
        scoreText.text = string.Format("{0:n0}",playerLogic.score);
    }

    void SpawnEnemy()
    {
        int ranEnemy = Random.Range(0, 3);
        int ranPoint = Random.Range(0, 11);
        GameObject enemy = Instantiate(enemyObjs[ranEnemy], spawnPoints[ranPoint].position,
            spawnPoints[ranPoint].rotation);

        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
        Enemy enemyLogic = enemy.GetComponent<Enemy>();
        enemyLogic.player = player;

        if (ranPoint == 7 || ranPoint == 8) // # Right spawn
        {
            enemy.transform.Rotate(Vector3.back * 90);
            rb.velocity = new Vector2(enemyLogic.speed * (-1), -1);
        }
        else if (ranPoint == 9 || ranPoint == 10) // # Left spawn
        {
            enemy.transform.Rotate(Vector3.forward * 90);
            rb.velocity = new Vector2(enemyLogic.speed , -1);
        }
        else // # Top spawn
        {
            rb.velocity = new Vector2(0, enemyLogic.speed * (-1));
        }
    }
    
    public void UpdateLifeIcon(int life)
    {
        for (int i = 0; i < 3; i++)
        {
            // #. UI Life Disable
            lifeImage[i].color = new Color(1, 1, 1, 0);
        }
        
            // #. UI Life Active
        for (int i = 0; i < life; i++)
        {
            lifeImage[i].color = new Color(1, 1, 1, 1);
        }
    }

    public void RespawnPlayer()
    {
        Invoke(nameof(RespawnPlayerExe), 2f);
    }
    
    private void RespawnPlayerExe()
    {
        player.transform.position = Vector3.down * 3.5f;
        player.SetActive(true);
        
        Player playerLogic = player.GetComponent<Player>();
        playerLogic.isHit = false;
    }

    public void GameOver()
    {
        gameOverSet.SetActive(true);
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }
}
