using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public static Manager Singleton;

    public float EscalaDeTiempo = 1;

    public int TasaDeHambre = 10;
    public int TasaDeFelicdad = 5;

    public int Felicidad = 100;
    public int Salud = 100;
    public int Hambre = 100;

    public int TiempoNesesarioParaSubir = 25;

    public Mascota MascotaActual;

    public Mascota[] EstadosDeMascotas;
    /*
        0- Muerto
        1- Huevo
        ... Estados Sig en orden
    */

    int TiempoTranscurrido;
    int EstadoActual = 0;
    /*
        -1 - Muerto
        0 - Huevo
        ... Estados Sig En Orden
    */
    

    public Slider SLDHambre;
    public Slider SLDSalud;
    public Slider SLDFelicidad;

    public Button BTNAlimentar;


    void Update()
    {
        RaycastHit h;
        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(r, out h))
            {
                if (h.collider.CompareTag("Player"))
                {
                    Acariciar();
                }
            }
        }
    }
    void Acariciar()
    {
        if (EstadoActual > 0)
        {
            if (Felicidad < 100)
            {
                Felicidad++;
            }
            SLDFelicidad.value = Felicidad;
        }
    }

	void Awake ()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }	
        else
        {
            Destroy(this);
        }
	}
    void Start()
    {
        BTNAlimentar.onClick.AddListener(Alimentar);
        SetUp();
        MascotaActual = Instantiate(EstadosDeMascotas[1], Vector3.zero, Quaternion.identity) as Mascota;
        InvokeRepeating("ActualizarCosas", 0, EscalaDeTiempo);
    }

    void Alimentar()
    {
        if (EstadoActual > 0)
        {

            if (Hambre < 100)
            {
                if (Felicidad > 25)
                {
                    Hambre += 20;
                }
            }
            else
            {
                Hambre = 100;
            }
            SLDHambre.value = Hambre;
        }
    }

    void SetUp()
    {
        SLDHambre.value = Hambre;
        SLDSalud.value = Salud;
        SLDFelicidad.value = Felicidad;
    }

    int TiempoHambre;
    int TiempoFelicidad;
    void ActualizarCosas()
    {
        if (EstadoActual > 0)
        {
            if (TiempoFelicidad > TasaDeFelicdad)
            {
                TiempoFelicidad = 0;
                Felicidad--;
                SLDFelicidad.value = Felicidad;
            }
            TiempoFelicidad++;

            if (TiempoHambre > TasaDeHambre)
            {
                TiempoHambre = 0;
                Hambre--;

                if (Hambre <= 40)
                {
                    Salud--;
                    SLDSalud.value = Salud;
                    if (Salud <= 0)
                    {
                        Morir();
                    }
                }
                else
                {
                    if (Salud < 100)
                    {
                        Salud++;
                        SLDSalud.value = Salud;
                    }
                }          

                SLDHambre.value = Hambre;
            }
            TiempoHambre++;
        }
        if (EstadoActual > -1)
        {
            if ((TiempoTranscurrido % TiempoNesesarioParaSubir) == 0)
            {
                CalcularCambioDeEstado();
            }
        }
        

        TiempoTranscurrido++;
    }
    void Morir()
    {
        Destroy(MascotaActual.gameObject);
        MascotaActual = Instantiate(EstadosDeMascotas[0], Vector3.zero, Quaternion.identity) as Mascota;
        EstadoActual = -1;
    }
    void CalcularCambioDeEstado()
    {
        int s = TiempoTranscurrido / TiempoNesesarioParaSubir;
        if (s == EstadoActual || EstadoActual >= EstadosDeMascotas.Length - 2)
        {
            return;
        }

        Destroy(MascotaActual.gameObject);
        MascotaActual = Instantiate(EstadosDeMascotas[s + 1], Vector3.zero, Quaternion.identity) as Mascota;
        EstadoActual = s;
    }
}
