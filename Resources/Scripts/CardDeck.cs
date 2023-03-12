
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class CardDeck : UdonSharpBehaviour
{
    public CardManager cardManager;
    public Transform trSpawn;


    public override void Interact() {
        if (cardManager.manager.isPlaying) {
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, "Draw");
        }
    }

    public void Draw() {
        Card card = cardManager.DrawSalemCard();
        if (card == null) {
            Debug.Log("Cant Draw SalemCard, SaleCard is Empty");
            return;
        }
        card.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "EnableCover");
        card.MoveToCard(trSpawn);
    }
}
