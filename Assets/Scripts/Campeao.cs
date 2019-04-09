using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Campeao : MonoBehaviour
{
    /*void Movimento()
    {
        var CT = FindObjectOfType<CriadorTabuleiro>();
        float t = 0;
        print("Casa 0: " + CasaOcupada[0]);
        print("Count: " + CT.ListaPosicoes.Count);
        CasaOcupada[CT.IndexPos(transform.position)] = false;
        Vector3 destino = transform.position + new Vector3(1.0f, 2.0f, 0.0f);
        print(destino);
        while (t < 1)
        {
            t += Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, destino, t);
        }            
        print("Casa 0 (2): " + CasaOcupada[0]);
        //transform.position = Vector3.MoveTowards(new Vector3(transform.position.x, transform.position.y), transform.position + new Vector3(1.0f,2.0f,0.0f), 3 * Time.deltaTime);
        
        print("Casa 11: " + CasaOcupada[10]);
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position == new Vector3(1, 2, 0))
        {
            var CT = FindObjectOfType<CriadorTabuleiro>();
            print("Transform: " + transform.position);
            print("Index: " + CT.IndexPos(transform.position));
            CasaOcupada[CT.IndexPos(transform.position)] = true;
            print("Casa 0 (Def) : " + CasaOcupada[0]);
            print("Casa 11 (Def): " + CasaOcupada[10]);
        }
    }*/
}
