using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CavaleiroScript : MonoBehaviour, IHerois
{
    private Vector3 DestInterpol;
    private bool Movimentar = false;
    public float VelociMov;

    public int tamanhoMovimento = 1; //Tamanho do movimento que o personagem fará
    public int tamanhoAtaque = 3; //Tamanho do ataque do personagem, apenas numeros impares

    public void Movimento()
    {
        int PosX = Mathf.RoundToInt(transform.position.x);
        int PosY = Mathf.RoundToInt(transform.position.y);

        ChecaMovimenta(PosX, PosY, tamanhoMovimento, tamanhoAtaque); 
    }

    private void ChecaMovimenta(int posX, int posY, int mov, int rangeAtaque)
    {
        if (GameManager.Turno == 1) {
            for (int i = 1; i <= mov; i++) {
                if (GameManager.CasaOcupada[posX, posY + i] == null) {
                    GameManager.CasaOcupada[posX, posY] = null;
                    GameManager.CasaOcupada[posX, posY + i] = gameObject;
                    DestInterpol = transform.position + Vector3.up;
                    Movimentar = true;
                } else { Atacar(posX, posY, rangeAtaque, 1); }
            }
        }
        else {
            for (int i = 1; i <= mov; i++) {
                if (GameManager.CasaOcupada[posX, posY - i] == null) {
                    GameManager.CasaOcupada[posX, posY] = null;
                    GameManager.CasaOcupada[posX, posY - i] = gameObject;
                    DestInterpol = transform.position + Vector3.down;
                    Movimentar = true;
                } else { Atacar(posX, posY, rangeAtaque, -1); }
            }
        }
    }

    private int ChecaInimigo(int posX, int posY, int rangeBusca)
    {
        if (GameManager.Turno == 1) {
            for (int i = -1; i < rangeBusca - 1; i++) {
                if (posX + i <= 0) { i = 0; }
                if (posX + i >= 7) { i = 0; }
                if (GameManager.CasaOcupada[posX + i, posY + 1] != null) {
                    if (GameManager.CasaOcupada[posX + i, posY + 1].CompareTag("Player2")) {
                        return posX + i;
                    }
                }
            }
        } else {
            for (int i = 1; i < rangeBusca - 1; i--) {
                if (posX + i <= 0) { i = 0; }
                if (posX + i >= 7) { i = 0; }
                if (GameManager.CasaOcupada[posX + i, posY - 1] != null) {
                    if (GameManager.CasaOcupada[posX + i, posY - 1].CompareTag("Player1")) {
                        return posX + i;
                    }
                }
            }
        }
        return -1;
    }

    private void Atacar(int posX, int posY, int rangeAtaque, int orientAtaque)
    {
        int i = rangeAtaque;
        while(i != 0) {
            int posInimigo = ChecaInimigo(posX, posY, rangeAtaque);
            if (posInimigo != -1) { 
                Destroy(GameManager.CasaOcupada[posInimigo, posY + orientAtaque]);
                GameManager.CasaOcupada[posInimigo, posY + orientAtaque] = null;
            }
            i--;
        }
    }

    public void AlteraSprite()
    {
        SpriteRenderer sr = transform.GetComponent<SpriteRenderer>();
        Arrastar spr = transform.GetComponent<Arrastar>();

        if (GameManager.Turno == 1) {
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            if (transform.CompareTag("Player1")) {
                sr.sprite = spr.spritesR[1];
            } else {
                sr.sprite = spr.spritesR[0];
            }
        }
        else {
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
            if (transform.CompareTag("Player2")) {
                sr.sprite = spr.spritesR[1];
            } else {
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
