using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrastar : MonoBehaviour
{
    public GameObject gameManager;
    private Vector2 initialPosition;
    private Vector2 mousePosition;
    private Vector2 gridPosition;
    private float deltaX, deltaY;
    private GameObject Hero;
    public Sprite[] spritesR;

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
        //GameManager GM = gameManager.GetComponent<GameManager>();

        int PosX = Mathf.RoundToInt(transform.position.x);
        int PosY = Mathf.RoundToInt(transform.position.y);
        SpriteRenderer SR = gameObject.GetComponent<SpriteRenderer>();

        if (GameManager.Turno == 1)
        {            
            if (PosX >= 0 && PosX <= 7 && PosY >= 0 && PosY <= 3) {
                transform.position = new Vector2(PosX, PosY);
                transform.tag = "Player1";
                GameManager.CasaOcupada[PosX, PosY] = gameObject; 
                SR.sprite = spritesR[1];
            }
            else {
                transform.position = new Vector2(initialPosition.x, initialPosition.y);
            }
        }
        else
        {
            if (PosX >= 0 && PosX <= 7 && PosY >= 4 && PosY <= 7) {
                transform.position = new Vector2(PosX, PosY);
                transform.tag = "Player2";
                GameManager.CasaOcupada[PosX, PosY] = gameObject;
                SR.sprite = spritesR[1];
            }
            else {
                transform.position = new Vector2(initialPosition.x, initialPosition.y);
            }
        }
    }
}