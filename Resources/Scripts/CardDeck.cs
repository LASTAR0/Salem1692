
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class CardDeck : UdonSharpBehaviour
{
    public CardManager cardManager;
    public Transform trSpawn;

    public GameObject[] cardCount;
    public Material[] matNumber;

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
        cardManager.syncSalemdeckCount--;
        cardManager.RequestSerialization();
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "SetCount");
    }

    public void SetCount() {
        int ten = cardManager.syncSalemdeckCount / 10;
        int one = cardManager.syncSalemdeckCount % 10;
        Material matTen = cardCount[0].GetComponent<Material>();
        Material matOne = cardCount[1].GetComponent<Material>();
        matTen = matNumber[ten];
        matOne = matNumber[one];
    }

    private void Update() {
        
    }
}
