using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class OnMouseEnterScriptTrigger : MonoBehaviour
{
    public void OnMouseEnterNew() //Se activa al pasar el cursor por encima de una carta.
    {
        OnMouseEnterTriggers();
    }
    private void OnMouseEnterTriggers()  //Muestra la carta y el efecto.
    {
        GameObject icon = GameObject.Find("CardIcon");
        GameObject text = GameObject.Find("EfectDescriptionIcon");
        GameObject card = this.gameObject;
        UnityEngine.UI.Image image = icon.GetComponent<UnityEngine.UI.Image>();
        if (card.GetComponent<PrefabCard>() != null) //Entra si la carta es unidad, especial o clima
        {
            image.sprite = card.GetComponent<UnityEngine.UI.Image>().sprite;
            text.GetComponent<TextMeshProUGUI>().text = card.GetComponent<PrefabCard>().Effect;
        }
        else if (card.GetComponent<CartaBaseSola>() != null) //entra si la carta es lider o esta en la seccion de aumento (cuerno de guerra activo)
        {
            CartaBaseSola cartaBaseSola = card.GetComponent<CartaBaseSola>();
            if (cartaBaseSola.card == null) //funciona si no hay carta en la seccion de cuerno
            {
                return;
            }
            UnityEngine.UI.Image imageCard = cartaBaseSola.card.GetComponent<UnityEngine.UI.Image>();
            image.sprite = imageCard.sprite;
            text.GetComponent<TextMeshProUGUI>().text = cartaBaseSola.card.GetComponent<PrefabCard>().Effect;
        }
    }

    //Este metodo se activa al pulsar click sobre cualquier carta
    public void OnMouseClickTriggers() 
    {
        GameObject card = this.gameObject;

        if (!GameManager.arranque) //Entra para cambiar cartas (arrancada en false)
        {
            if (!GameManager.arranque)
            {
                MetodosUtilesUnity.SwitchCard(card);
            }
        }
        else //Entra cuando el arranque da positivo, envia la carta al metodo AddToSection para que sea colocada.
        {
            GameObject player;
            if (card.transform.IsChildOf(GameObject.Find("Player1").transform))
            {
                player = GameObject.Find("Player1");
            }
            else
            {
                player = GameObject.Find("Player2");
            }

            GameObject hand = player.transform.Find("Hand").gameObject;
            ClaseJugador claseJugador = player.GetComponent<ClaseJugador>();

            if (card.GetComponent<CartaBaseSola>() != null && card.transform.IsChildOf(player.transform) && !player.GetComponent<ClaseJugador>().ActivateLeader) //Entra si la carta seleccionada es la lider
            {
                GameObject realCard = card.GetComponent<CartaBaseSola>().card;
                Effects.CardEffect(realCard);
                player.GetComponent<ClaseJugador>().ActivateLeader = true;
                GameManager.ChangeTurn();
                return;
            }

            if (GameManager.EnableDecoy && card.GetComponent<PrefabUnitCard>().UnitType != Unit_Card.EType.Gold.ToString()) //Entra si se está en un proceso de intercambio con señuelo
            {
                Effects.DecoyEffect(card, hand);
                GameManager.EnableDecoy = false;
                GameManager.ChangeTurn();
                return;
            }
            
            if (card.transform.IsChildOf(hand.transform) && claseJugador.PlayerTurn) //Entra si la carta esta en su mano y es su turno
            {
                if (claseJugador.HasPassed) //entra si el jugador a pasado su turno
                {
                    return;
                }
                Effects.CardEffect(card);
                MetodosUtilesUnity.AddToSection(card, player, hand);
            }
        }
    }
}
