using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class CriadorTabuleiro : MonoBehaviour
{
    public static List<Vector3> ListaPosicoes = new List<Vector3>();

    public int IndexPos(Vector3 pos)
    {
        for (int x = 0; x < ListaPosicoes.Count; x++)
        {
            if (ListaPosicoes[x] == pos)
            {
                return x;
            }
        }
        return -1;
    }

    void Awake()
    {
        Tilemap Tabuleiro = GetComponent<Tilemap>();
        BoundsInt LimitesTabu = Tabuleiro.cellBounds;

        for (int x = 0; x < LimitesTabu.size.x; x++)
        {
            for (int y = 0; y < LimitesTabu.size.y; y++)
            {
                ListaPosicoes.Add(new Vector3(x, y, 0));
            }
        }
    }
}
