using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //Declaração das variaveis que serão utilizadas
    public static List<GameObject> HeroisT = new List<GameObject>();
    public Button BotaoPassar;
    public GameObject Campeao;
    public Transform Loja;
    public Camera Cam;
    public Text TextoTurno;
    public static GameObject[,] CasaOcupada = new GameObject[9, 9];
    private Vector3 PosAtual;
    private int TurnoTotal;
    public static int Turno;
    //private Quaternion AnguloDestino;
    //private Quaternion AnguloAtual; -> FUNCIONA
    private float AnguloDestino;
    private float AnguloAtual;
    private float TempoIniciou;
    public float TempoRotacao;
    private bool GirandoCam = false;
    private float CamVelo = 0.0f;


    void Awake()
    {
        Cam = Camera.main;
        AnguloAtual = Cam.transform.eulerAngles.z;
        AnguloDestino = AnguloAtual;

        TurnoTotal = 1;
        Turno = Mathf.RoundToInt(Random.Range(1, 3)); //Gera turno aleatório e chama função que exibe msg
        TextoTurno.enabled = false;
        if(Turno == 2) { RodarCamera(); }
        StartCoroutine(MensagemTurno());

        for (int x = 0; x < 9; x++)
        {
            CasaOcupada[x, x] = null;
        }

        MontaLoja();
    }

    void Update()
    {
        if (GirandoCam)
        {
            //float TempoPercorrido = Time.time - TempoIniciou;
            //float PorcentCompleta = TempoPercorrido / TempoRotacao; -> FUNCIONA
            //float AnguloInterpol = Mathf.Lerp(AnguloAtual, AnguloDestino, Time.deltaTime * VelocidadeCam);
            float AnguloInterpol = Mathf.SmoothDampAngle(Cam.transform.eulerAngles.z, AnguloDestino, ref CamVelo, TempoRotacao);
            //Cam.transform.rotation = Quaternion.Slerp(AnguloAtual, AnguloDestino, PorcentCompleta); -> FUNCIONA
            //float AnguloInterpol = Mathf.LerpAngle(AnguloAtual, AnguloDestino, PorcentCompleta);
            Cam.transform.rotation = Quaternion.Euler(0.0f, 0.0f, AnguloInterpol);
            if (Mathf.Abs(Cam.transform.eulerAngles.z) == 180.0f || Cam.transform.eulerAngles.z == 0.0f) { GirandoCam = false; }

            //if (PorcentCompleta >= 1.0f)
            //{
            //    GirandoCam = false;
            //}
        }        
    }

    void MontaLoja() //Função que monta uma loja de heróis
    {
        for (int i=0; i<=2; i++)
        {
            var CampLoja = Instantiate<GameObject>(Campeao, new Vector3(12 + i, 0 + i), Quaternion.identity, Loja);
            var CampLoja2 = Instantiate<GameObject>(Campeao, new Vector3(-7 + i, 0 + i), Quaternion.Euler(0,0,180), Loja);
            CampLoja.transform.SetParent(Loja, false);
            CampLoja2.transform.SetParent(Loja, false);
        }
    }

    private void RodarCamera()
    {
        //TempoIniciou = Time.time;
        //AnguloAtual = Cam.transform.rotation;
        //AnguloDestino = AnguloAtual * Quaternion.Euler(0,0,180); -> FUNCIONA
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

        if (Turno == 1) { Turno = 2; }
        else { Turno = 1; }

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

