using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class UpdateMetodos : MonoBehaviour //Aqui se encuentra lo relacionado con la ejecucion por cada frame del juego
{
    //Objetos que se asigan directamente del inspector para actualizarlos constantemente.
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Player 1
    //Contadores de ataque
    public GameObject attackCountMeleeP1;
    public GameObject attackCountRangeP1;
    public GameObject attackCountSiegeP1;
    public GameObject attackCountTotalP1;
    //Secciones del tablero
    public GameObject meleeSectionP1;
    public GameObject rangeSectionP1;
    public GameObject siegeSectionP1;
    //Listas de las secciones del tablero
    public static List<GameObject> ListMeleeP1; 
    public static List<GameObject> ListRangeP1; 
    public static List<GameObject> ListSiegeP1; 
    //Secciones de cuernos
    public GameObject hornSectionMeleeP1;
    public GameObject hornSectionRangeP1;
    public GameObject hornSectionSiegeP1;
    //Secciones de cuerno (CartaBaseSola)
    public static CartaBaseSola hornCardMP1;
    public static CartaBaseSola hornCardRP1;
    public static CartaBaseSola hornCardSP1;
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Player 2
    //Contadores de ataque
    public GameObject attackCountMeleeP2;
    public GameObject attackCountRangeP2;
    public GameObject attackCountSiegeP2;
    public GameObject attackCountTotalP2;
    //Secciones del tablero
    public GameObject meleeSectionP2;
    public GameObject rangeSectionP2;
    public GameObject siegeSectionP2;
    //Listas de las secciones del tablero
    public static List<GameObject> ListMeleeP2; 
    public static List<GameObject> ListRangeP2; 
    public static List<GameObject> ListSiegeP2; 
    //Secciones de cuernos
    public GameObject hornSectionMeleeP2;
    public GameObject hornSectionRangeP2;
    public GameObject hornSectionSiegeP2;
    //Secciones de cuerno (CartaBaseSola)
    public static CartaBaseSola hornCardMP2;
    public static CartaBaseSola hornCardRP2;
    public static CartaBaseSola hornCardSP2;
    //Secciones de mano de cada Jugador
    public GameObject HandP1;
    public GameObject HandP2;
    public static List<GameObject> ListHandP1;
    public static List<GameObject> ListHandP2;
    //Secciones de los deck de cada Jugador
    public GameObject GraveyardP1;
    public GameObject GraveyardP2;
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Metodo que se encarga de extraer las listas correspondientes de cada GameObject de seccion y las coloca en variables estaticas.(tambien se encarga de hacer los mismo con las secciones de cuerno)
    public void InitializeListsAndHornCards()
    {
        ListMeleeP1 = meleeSectionP1.GetComponent<ListaBaseConGObject>().cards;
        ListRangeP1 = rangeSectionP1.GetComponent<ListaBaseConGObject>().cards;
        ListSiegeP1 = siegeSectionP1.GetComponent<ListaBaseConGObject>().cards;

        ListMeleeP2 = meleeSectionP2.GetComponent<ListaBaseConGObject>().cards;
        ListRangeP2 = rangeSectionP2.GetComponent<ListaBaseConGObject>().cards;
        ListSiegeP2 = siegeSectionP2.GetComponent<ListaBaseConGObject>().cards;

        ListHandP1 = HandP1.GetComponent<ListaBaseConGObject>().cards;
        ListHandP2 = HandP2.GetComponent<ListaBaseConGObject>().cards;

        hornCardMP1 = hornSectionMeleeP1.GetComponent<CartaBaseSola>();
        hornCardRP1 = hornSectionRangeP1.GetComponent<CartaBaseSola>();
        hornCardSP1 = hornSectionSiegeP1.GetComponent<CartaBaseSola>();

        hornCardMP2 = hornSectionMeleeP2.GetComponent<CartaBaseSola>();
        hornCardRP2 = hornSectionRangeP2.GetComponent<CartaBaseSola>();
        hornCardSP2 = hornSectionSiegeP2.GetComponent<CartaBaseSola>();        
    }

    //Metodo que se encarga de asignar el valor de ataque a su seccion correspondiente mediante su contador a la izquierda del tablero 
    public void SetAttackNumber(GameObject count, List<GameObject> section)
    {
        int totalAttack = 0;

        foreach (GameObject card in section)
        {
            totalAttack += card.GetComponent<PrefabUnitCard>().Attack;
        }

        count.GetComponent<TextMeshProUGUI>().text = totalAttack.ToString();
    }

    //Metodo que asigna el valor total de ataque de cada jugador mediante los valores de sus ataques por seccion
    public void SetAttackNumberTotal()
    {
        int m1 = int.Parse(attackCountMeleeP1.GetComponent<TextMeshProUGUI>().text);
        int r1 = int.Parse(attackCountRangeP1.GetComponent<TextMeshProUGUI>().text);
        int s1 = int.Parse(attackCountSiegeP1.GetComponent<TextMeshProUGUI>().text);

        int m2 = int.Parse(attackCountMeleeP2.GetComponent<TextMeshProUGUI>().text);
        int r2 = int.Parse(attackCountRangeP2.GetComponent<TextMeshProUGUI>().text);
        int s2 = int.Parse(attackCountSiegeP2.GetComponent<TextMeshProUGUI>().text);

        int total1 = m1 + r1 + s1;
        int total2 = m2 + r2 + s2;

        attackCountTotalP1.GetComponent<TextMeshProUGUI>().text = total1.ToString();
        attackCountTotalP2.GetComponent<TextMeshProUGUI>().text = total2.ToString(); 
    }


    void Update()
    {
        if (!GameManager.arranque) return; //Si aún no ha arrancado el juego, simplemente el metodo retorna.

        //Esta seccion verifica antes de que se actualice los ataques, si hay un cuerno de guerra en alguna seccion. Si lo hay aplica el efecto.
        if (hornCardMP1.card != null) Effects.HornEffect(meleeSectionP1);
        if (hornCardRP1.card != null) Effects.HornEffect(rangeSectionP1);
        if (hornCardSP1.card != null) Effects.HornEffect(siegeSectionP1);

        if (hornCardMP2.card != null) Effects.HornEffect(meleeSectionP2);
        if (hornCardRP2.card != null) Effects.HornEffect(rangeSectionP2);
        if (hornCardSP2.card != null) Effects.HornEffect(siegeSectionP2);
        //Esta seccion verifica si existe algun clima activo. De ser asi, aplica el metodo correspondiente
        if (GameManager.frostEffect) Effects.WeatherEffect(meleeSectionP1,meleeSectionP2);
        if (GameManager.fogEffect) Effects.WeatherEffect(rangeSectionP1,rangeSectionP2);
        if (GameManager.rainEffect) Effects.WeatherEffect(siegeSectionP1,siegeSectionP2);
        //Esta seccion de metodos actualiza por cada frame el ataque de cada jugador.
        SetAttackNumber(attackCountMeleeP1, ListMeleeP1);
        SetAttackNumber(attackCountRangeP1, ListRangeP1);
        SetAttackNumber(attackCountSiegeP1, ListSiegeP1);

        SetAttackNumber(attackCountMeleeP2, ListMeleeP2);
        SetAttackNumber(attackCountRangeP2, ListRangeP2);
        SetAttackNumber(attackCountSiegeP2, ListSiegeP2);

        SetAttackNumberTotal();
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Si la mano de algun jugador llega a tener más de 10 cartas, se remueve una carta (por cada frame hasta tener 10)
        if (ListHandP1.Count > 10) MetodosUtilesUnity.AddToListOneCard(ListHandP1[ListHandP1.Count-1], HandP1, GraveyardP1);
        if (ListHandP2.Count > 10) MetodosUtilesUnity.AddToListOneCard(ListHandP2[ListHandP2.Count-1], HandP2, GraveyardP2);
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        


    }
}
