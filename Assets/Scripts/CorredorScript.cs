using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorredorScript : MonoBehaviour, IHerois
{
    private Vector3 DestInterpol;
    private bool Movimentar = false;
    public float VelociMov;

    public int tamanhoMovimento = 3; //Tamanho do movimento que o personagem fará
    public int tamanhoAtaque = 1; //Tamanho do ataque do personagem, apenas numeros impares

    public void Movimento()
    {
        if (gameObject.CompareTag("Player1") || gameObject.CompareTag("Player2")) {
            ChecaMovimentaCorr(tamanhoMovimento, tamanhoAtaque);
        }
    }

    private void ChecaMovimentaCorr(int mov, int rangeAtaque)
    {
        int posX = Mathf.RoundToInt(transform.position.x);
        int posY = Mathf.RoundToInt(transform.position.y);
        int i = 0;

        if (GameManager.Turno == 1)
        {
            while (i < mov - 1)
            {
                posY += i;

                if (posY >= 7) {
                    DestInterpol = transform.position + (Vector3.down * i);
                    Movimentar = true;
                    AtacarHP(2);
                    break;
                }
                else if (GameManager.CasaOcupada[posX, posY + 1] == null) {
                    GameManager.CasaOcupada[posX, posY] = null;
                    GameManager.CasaOcupada[posX, posY + 1] = gameObject;
                    i++;
                }
                else {
                    DestInterpol = transform.position + (Vector3.up * i);
                    Movimentar = true;
                    Atacar(posX, posY, rangeAtaque, 1, "Player2");
                    break;
                }
                DestInterpol = transform.position + (Vector3.up * i);
                Movimentar = true;
            }
        }
        else
        {
            while (i < mov - 1)
            {
                posY -= i;

                if (posY <= 0)
                {
                    DestInterpol = transform.position + (Vector3.down * i);
                    Movimentar = true;
                    AtacarHP(1);
                    break;
                } else if (GameManager.CasaOcupada[posX, posY - 1] == null) {
                    GameManager.CasaOcupada[posX, posY] = null;
                    GameManager.CasaOcupada[posX, posY - 1] = gameObject;
                    i++;
                }
                else {
                    DestInterpol = transform.position + (Vector3.down * i);
                    Movimentar = true;
                    Atacar(posX, posY, rangeAtaque, -1, "Player1");
                    break;
                }
            }
            DestInterpol = transform.position + (Vector3.down * i);
            Movimentar = true;
        }
    }

    private bool ChecaInimigo(int posX, int posY, int orientAtaque, string player)
    {
        if (GameManager.CasaOcupada[posX, posY + orientAtaque].CompareTag(player))
        {
            return true;
        }
        return false;
    }

    private void Atacar(int posX, int posY, int rangeAtaque, int orientAtaque, string player)
    {
        if(ChecaInimigo(posX, posY, orientAtaque, player))
        {
            Destroy(GameManager.CasaOcupada[posX, posY + orientAtaque]);
            GameManager.CasaOcupada[posX, posY + orientAtaque] = null;
        }        
    }

    private void AtacarHP(int player)
    {
        if (player == 1) { GameManager.hpPlayer1--; }
        else { GameManager.hpPlayer2--; }
    }

    public void AlteraSprite()
    {
        SpriteRenderer sr = transform.GetComponent<SpriteRenderer>();
        Arrastar spr = transform.GetComponent<Arrastar>();

        if (GameManager.Turno == 1)
        {
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            if (transform.CompareTag("Player1"))
            {
                sr.sprite = spr.spritesR[1];
            }
            else
            {
                sr.sprite = spr.spritesR[0];
            }
        }
        else
        {
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
            if (transform.CompareTag("Player2"))
            {
                sr.sprite = spr.spritesR[1];
            }
            else
            {
                sr.sprite = spr.spritesR[0];
            }
        }
    }

    void Update()
    {
        if (Movimentar)
        {
            transform.position = Vector3.MoveTowards(transform.position, DestInterpol, VelociMov * Time.deltaTime);
            if (transform.position == DestInterpol) { Movimentar = false; }
        }
    }
}
