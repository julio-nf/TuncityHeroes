using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static int Turno;
    public static List<GameObject> HeroisP1 = new List<GameObject>();
    public static List<GameObject> HeroisP2 = new List<GameObject>();
    public Text TextoTurno;
    private bool[] CasaOcupada;
    private Vector3 PosAtual;
    public Button BotaoPassar;
    public GameObject Campeao;
    public Transform Loja;

    void Awake()
    {
        Turno = Random.Range(1, 3);
        TextoTurno.enabled = false;
        StartCoroutine(MensagemTurno());

        CasaOcupada = new bool[CriadorTabuleiro.ListaPosicoes.Count];
        for (int x = 0; x < CasaOcupada.Length; x++)
        {
            CasaOcupada[x] = false;
        }

        MontaLoja();
    }

    void MontaLoja() //Função que monta uma loja genérica de heróis, será aleatorizada posteriormente
    {
        for (int i=0; i<=2; i++)
        {
            var CampLoja = Instantiate<GameObject>(Campeao, new Vector3(12 + i, 0 + i), Quaternion.identity, Loja);
            var CampLoja2 = Instantiate<GameObject>(Campeao, new Vector3(-7 + i, 0 + i), Quaternion.identity, Loja);
            CampLoja.transform.SetParent(Loja, false);
            CampLoja2.transform.SetParent(Loja, false);
        }
    }

    public void PassarTurno() //Função de passar o turno ao apertar o botão
    {
        if (Turno == 1)
        {
            MoverPersonagens();
            Turno = 2;
            StartCoroutine(MensagemTurno());
        } else
        {
            MoverPersonagens();
            Turno = 1;
            StartCoroutine(MensagemTurno());
        }
    }

    void MoverPersonagens()
    {
        if(Turno == 1)
        {
            foreach (GameObject Herois in HeroisP1)
            {
                Herois.transform.position = Herois.transform.position + Vector3.right;
            }
        }
        else
        {
            foreach (GameObject Herois in HeroisP2)
            {
                Herois.transform.position = Herois.transform.position + Vector3.left;
            }
        }        
    }

    IEnumerator MensagemTurno() //Exibe a mensagem de quem é o turno por 5 segundos
    {
        TextoTurno.text = "Turno do Jogador " + Turno.ToString();
        BotaoPassar.interactable = false;
        TextoTurno.enabled = true;
        yield return new WaitForSeconds(5);
        TextoTurno.enabled = false;
        BotaoPassar.interactable = true;
    }
}

