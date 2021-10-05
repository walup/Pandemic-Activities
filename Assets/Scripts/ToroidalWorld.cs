using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToroidalWorld : MonoBehaviour
{

    private Vector2 anchorPoint;
    private float worldWidth;
    private float worldHeight;

    // Start is called before the first frame update
    void Awake()
    {

        Camera camera = Camera.main;
        float completeWidth = 2 * camera.aspect * camera.orthographicSize;
        float completeHeight = 2 * camera.orthographicSize;
        RectTransform uiRect = GameObject.Find("GraphPanel").GetComponent<RectTransform>();
        anchorPoint = Camera.main.ScreenToWorldPoint(new Vector2(uiRect.rect.width, uiRect.rect.height));
        anchorPoint.y = -anchorPoint.y;

        worldWidth = completeWidth / 2 + Mathf.Abs(anchorPoint.x);
        worldHeight = 2 * camera.orthographicSize;

        //Debuggeamos

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float nfmod(float a, float b)
    {
        return (float)(a - b * Mathf.Floor(a / b));
    }

    public Vector2 getToroidalPosition(Vector2 position)
    {
        Vector2 toroidalPosition = new Vector2();

        toroidalPosition.x = anchorPoint.x + nfmod(position.x - anchorPoint.x, worldWidth);
        toroidalPosition.y = anchorPoint.y + nfmod(position.y - anchorPoint.y, worldHeight);

        return toroidalPosition;

    }


    public float getToroidalDistance(Vector2 position1, Vector2 position2)
    {
        float distance = 0;
        float dx = Mathf.Abs(position1.x - position2.x);
        float dy = Mathf.Abs(position1.y - position2.y);

        if(dx > worldWidth / 2)
        {
            dx = worldWidth - dx;
        }
        if(dy > worldHeight / 2)
        {
            dy = worldHeight - dy;
        }

        Vector2 differential = new Vector2(dx, dy);
        distance = differential.magnitude;
        return distance;
    }

    public Vector2 getAnchorPoint()
    {
        return this.anchorPoint;
    }

    public float getWorldWidth()
    {
        return this.worldWidth;
    }

    public float getWorldHeight()
    {
        return this.worldHeight;
    }


}
