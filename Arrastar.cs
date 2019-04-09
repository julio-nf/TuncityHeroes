using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrastar : MonoBehaviour
{
    private Vector2 initialPosition;
    private Vector2 mousePosition;
    private Vector2 gridPosition;
    private float deltaX, deltaY;
    private GameObject Hero;


    private void Start()
    {
        initialPosition = transform.position;
    }

    private void OnMouseDown()
    {

        deltaX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x;
        deltaY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - transform.position.y;

    }

    private void OnMouseDrag()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(mousePosition.x - deltaX, mousePosition.y - deltaY);
    }

    private void OnMouseUp()
    {
        float PosX = Mathf.RoundToInt(transform.position.x);
        float PosY = Mathf.RoundToInt(transform.position.y);
        if (GameController.Turno == 1)
        {            
            if (PosX >= 0 && PosX <= 1 && PosY >= 0 && PosY <= 8)
            {
                transform.position = new Vector2(PosX, PosY);
                GameController.HeroisP1.Add(gameObject);
            }

            else
            {
                transform.position = new Vector2(initialPosition.x, initialPosition.y);
            }
        }
        else
        {
            if (PosX >= 6 && PosX <= 7 && PosY >= 0 && PosY <= 8)
            {
                transform.position = new Vector2(PosX, PosY);
                GameController.HeroisP2.Add(gameObject);
            }
            else
            {
                transform.position = new Vector2(initialPosition.x, initialPosition.y);
            }
        }
    }
}