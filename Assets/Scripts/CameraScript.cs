using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject leftSideObject;
    public GameObject topSideObject;
    public GameObject bottomSideObject;

    // Start is called before the first frame update
    void Start()
    {
        //Adjusting camera position to screen resolution
        float height = 2f * Camera.main.orthographicSize;
        float width = height * Camera.main.aspect;

        float leftSideX = leftSideObject.transform.position.x;
        float leftSideWidth = leftSideObject.GetComponent<SpriteRenderer>().bounds.size.x;
        float x = (leftSideX - leftSideWidth / 2) + width / 2;

        float topSideY = topSideObject.transform.position.y;
        float topSideHeight = topSideObject.GetComponent<SpriteRenderer>().bounds.size.y;
        float bottomSideY = bottomSideObject.transform.position.y;
        float bottomSideHeight = bottomSideObject.GetComponent<SpriteRenderer>().bounds.size.y;
        float y = ((topSideY - topSideHeight / 2) + (bottomSideY + bottomSideHeight / 2)) / 2;
        Camera.main.transform.position = new Vector3(x, y, Camera.main.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
