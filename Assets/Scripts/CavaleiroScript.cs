using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CavaleiroScript : MonoBehaviour, IHerois
{
    private Vector3 DestInterpol;
    private bool Movimentar = false;
    public float VelociMov;
    public Text precoCamp;
    public int preco = 5;

    public int tamanhoMovimento = 1; //Tamanho do movimento que o personagem fará
    public int tamanhoAtaque = 3; //Tamanho do ataque do personagem, apenas numeros impares

    public void Movimento()
    {
        if (gameObject.CompareTag("Player1") || gameObject.CompareTag("Player2")) {
            ChecaMovimentaCav(tamanhoMovimento, tamanhoAtaque);
        }
    }

    private void ChecaMovimentaCav(int mov, int rangeAtaque)
    {
        int posX = Mathf.RoundToInt(transform.position.x);
        int posY = Mathf.RoundToInt(transform.position.y);

        if (GameManager.Turno == 1)
        {
            if (posY >= 7) { AtacarHP(2); }
            else if (GameManager.CasaOcupada[posX, posY + 1] == null)
            {
                GameManager.CasaOcupada[posX, posY] = null;
                GameManager.CasaOcupada[posX, posY + 1] = gameObject;
                DestInterpol = transform.position + Vector3.up;
                Movimentar = true;
            }
            else
            {
                Atacar(posX, posY, rangeAtaque, 1);
            }
        }
        else
        {
            if (posY <= 0) { AtacarHP(0); }
            else if (GameManager.CasaOcupada[posX, posY - 1] == null)
            {
                GameManager.CasaOcupada[posX, posY] = null;
                GameManager.CasaOcupada[posX, posY - 1] = gameObject;
                DestInterpol = transform.position + Vector3.down;
                Movimentar = true;
            }
            else
            {
                Atacar(posX, posY, rangeAtaque, -1);
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

    private void AtacarHP(int player)
    {
        if (player == 1) { GameManager.hpPlayer1--; }
        else { GameManager.hpPlayer2--; }
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

    // void OnMouseOver()
    // {
    //     if (transform.CompareTag("Loja"))
    //     {
    //         precoCamp.enabled = true;
    //         precoCamp.text = "Valor: $" + preco;
    //     }
    // }
    // 
    // void OnMouseExit()
    // {
    //     if (transform.CompareTag("Loja"))
    //     {
    //         precoCamp.enabled = false;
    //     }
    // }
    // 
    // void OnMouseDown()
    // {
    //     if (transform.CompareTag("Loja") && GameManager.Turno == 1 && GameManager.moneyP1 >= preco)
    //     {
    //         GameManager.moneyP1 -= preco;
    //         gameObject.transform.position = new Vector3(-7, -2);
    //     } else if (transform.CompareTag("Loja") && GameManager.Turno == 2 && GameManager.moneyP2 >= preco){
    //         GameManager.moneyP2 -= preco;
    //         gameObject.transform.position = new Vector3(-5, 9);
    //     }
    // }

    void Update()
    {
        if (Movimentar)
        {
            transform.position = Vector3.MoveTowards(transform.position, DestInterpol, VelociMov * Time.deltaTime);
            if (transform.position == DestInterpol) { Movimentar = false; }
        } 
    }
}
