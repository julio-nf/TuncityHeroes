using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //Declaração das variaveis que serão utilizadas
    public static List<GameObject> HeroisT = new List<GameObject>();
    public GameObject[] heroisPrefabs;
    public Button BotaoPassar;
    public Transform Loja;
    private Camera Cam;
    public Text TextoTurno, vidaPlayer1, vidaPlayer2, dinheiro, turnoAtual, precoCamp;
    public static GameObject[,] CasaOcupada = new GameObject[9, 9];
    private Vector3 PosAtual;
    private int TurnoTotal;
    public static int Turno, moneyP1 = 50, moneyP2 = 50, hpPlayer1 = 5, hpPlayer2 = 5;
    private float AnguloDestino, AnguloAtual;
    public float TempoRotacao;
    private bool GirandoCam = false;
    private float CamVelo = 0.0f;

    void Awake()
    {
        TextoTurno.enabled = false;
        precoCamp.enabled = false;

        TurnoTotal = 1;
        Turno = Mathf.RoundToInt(Random.Range(1, 3)); //Gera turno aleatório e chama função que exibe msg
        StartCoroutine(MensagemTurno());
    }

    private void Start()
    {
        Cam = Camera.main;
        AnguloAtual = Cam.transform.eulerAngles.z;
        AnguloDestino = AnguloAtual;

        for (int x = 0; x < 9; x++) {
            CasaOcupada[x, x] = null;
        }

        turnoAtual.text = "Turno Player " + Turno;
        if (Turno == 2) { RodarCamera(); }

        MontaLoja();
    }

    void Update()
    {
        if (GirandoCam)
        {
            float AnguloInterpol = Mathf.SmoothDampAngle(Cam.transform.eulerAngles.z, AnguloDestino, ref CamVelo, TempoRotacao);
            Cam.transform.rotation = Quaternion.Euler(0.0f, 0.0f, AnguloInterpol);
            if (Mathf.Abs(Cam.transform.eulerAngles.z) == 180.0f || Cam.transform.eulerAngles.z == 0.0f) { GirandoCam = false; }
        }
    }

    void LateUpdate()
    {
        vidaPlayer1.text = "Vida Player 1: " + hpPlayer1;
        vidaPlayer2.text = "Vida Player 2: " + hpPlayer2;

        if (Turno == 1) { dinheiro.text = "$ " + moneyP1; }
        else { dinheiro.text = "$ " + moneyP2; }

        if (hpPlayer1 <= 0) { TextoTurno.text = "Vitória do Player 2"; Application.Quit(); }
        else if (hpPlayer2 <= 0) { TextoTurno.text = "Vitória do Player 1"; Application.Quit(); }
    }

    void MontaLoja() //Função que monta uma loja de heróis
    {
        for (int i=0; i<=2; i++)
        {
            for (int j=0; j<=2; j++)
            {
                var CampLoja = Instantiate<GameObject>(heroisPrefabs[Random.Range(0, heroisPrefabs.Length)], new Vector3(12 + i, -2 + j), Quaternion.identity);
                var CampLoja2 = Instantiate<GameObject>(heroisPrefabs[Random.Range(0, heroisPrefabs.Length)], new Vector3(-7 + i, 7 + j), Quaternion.Euler(0, 0, 180));
                CampLoja.tag = "Loja";
                CampLoja.tag = "Loja";
            } 
        }
    }

    private void RodarCamera()
    {
        AnguloAtual = Mathf.RoundToInt(Cam.transform.eulerAngles.z);
        AnguloDestino = ArredondaAngulo(AnguloAtual + 180.0f);
        GirandoCam = true;

        foreach (GameObject herois in CasaOcupada) {
            if (herois != null) {
                IHerois Script = (IHerois)herois.GetComponent(typeof(IHerois));
                Script.AlteraSprite();
            }
        }
    }

    private float ArredondaAngulo(float ang)
    {
        if (ang >= 360.0f) { return 0.0f ; }
        else { return 180.0f; }
    }

    private void PassarTurno() //Função de passar o turno ao apertar o botão
    {
        StartCoroutine(PassagemTurno());
    }

    IEnumerator PassagemTurno()
    {
        TurnoTotal++;
        MoverPersonagens();

        yield return new WaitForSeconds(2);
        
        if(Turno == 1) { Turno = 2; }
        else { Turno = 1; }

        turnoAtual.text = "Turno Player " + Turno;
        RodarCamera();
        StartCoroutine(MensagemTurno());
    }

    private void MoverPersonagens() //Função que move os personagens dependendo do turno
    {
        if (Turno == 1) {
            GameObject[] HeroisP1 = GameObject.FindGameObjectsWithTag("Player1");
            foreach (GameObject herois in HeroisP1) {
                IHerois Script = (IHerois)herois.GetComponent(typeof(IHerois));
                Script.Movimento();
            }
        } else {
            GameObject[] HeroisP2 = GameObject.FindGameObjectsWithTag("Player2");
            foreach (GameObject herois in HeroisP2) {
                IHerois Script = (IHerois)herois.GetComponent(typeof(IHerois));
                Script.Movimento();
            }
        }
    }

    IEnumerator MensagemTurno() //Exibe a mensagem de quem é o turno por 5 segundos
    {
        TextoTurno.text = "Turno do Jogador " + Turno.ToString();
        BotaoPassar.interactable = false;
        TextoTurno.enabled = true;
        yield return new WaitForSeconds(3);
        TextoTurno.enabled = false;
        BotaoPassar.interactable = true;
    }
}

