using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public float speed;
    public int startIndex;
    public int endIndex;
    public Transform[] sprites;
    
    float viewHeight;
    
    void Awake()
    {
        viewHeight = Camera.main.orthographicSize * 2;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Scrolling();
        
    }

    private void Move()
    {
           Vector3 curPos = transform.position;
                Vector3 nextPos = Vector3.down * speed * Time.deltaTime;
                transform.position = curPos + nextPos;
    }

    private void Scrolling()
    {
        if (sprites[endIndex].position.y < viewHeight * (-1)) 
        {
            // #. Sprite ReUse
            Vector3 backSpritePos = sprites[startIndex].localPosition;
            Vector3 frontSpritePos = sprites[endIndex].localPosition;
            sprites[endIndex].transform.localPosition = backSpritePos + Vector3.up * viewHeight;
            
            // #. Cursor Index Change
            int startIndexSave = startIndex;
            startIndex = endIndex;
            endIndex = (startIndexSave - 1 == -1) ? sprites.Length - 1 : startIndexSave - 1;
        }
    }
}
