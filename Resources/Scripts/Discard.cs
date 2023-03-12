
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Discard : UdonSharpBehaviour
{
    public Card[] cards;
    [Header("Salem")]
    public string type = "Salem";

    private const int NUM_SALEMCARDS = 58;

    public void OnTriggerEnter(Collider other) {
        if (!isCard(other))
            return;
        Card card = other.GetComponent<Card>();
        if (card == null)
            return;
        if (card.type != type || card.isUsed)
            return;
        VRC_Pickup pickup = other.GetComponent<VRC_Pickup>();
        if (Networking.LocalPlayer != Networking.GetOwner(gameObject)) {
            if (Networking.GetOwner(card.gameObject) == Networking.LocalPlayer) {
                pickup.Drop();
                MoveToCard(card);
            }
            return;
        }

        Networking.SetOwner(Networking.LocalPlayer, other.gameObject);
        card.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "Use");
        pickup.Drop();
        MoveToCard(card);
    }
    public void OnTriggerExit(Collider other) {
        if (!isCard(other))
            return;
        Card card = other.GetComponent<Card>();
        if (card == null) 
            return;
        if (card.type != type || !card.isUsed) 
            return;
        if (Networking.LocalPlayer != Networking.GetOwner(gameObject)) {
            RemoveFromArray(card);
            return;
        }

        card.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "UnUse");
        RemoveFromArray(card);
    }
    public void OnReset() {
        for(int i = 0; i < cards.Length; ++i) {
            cards[i] = null;
        }
    }
    public int GetSize() {
        int num = 0;
        for (int i = 0; i < cards.Length; ++i) {
            if (cards[i] != null) {
                num++;
            }
        }
        return num;
    }

    private void MoveToCard(Card card) {
        for (int i = 0; i < cards.Length; ++i) {
            if (cards[i] == null) {
                cards[i] = card;
                Vector3 pos = transform.position;
                pos.y += i * 0.001f;
                card.transform.position = pos;
                card.transform.rotation = transform.rotation;
                return;
            }
        }
    }
    private void RemoveFromArray(Card card) {
        for (int i = 0; i < cards.Length; ++i) {
            if (cards[i] == card) {
                cards[i] = null;
                return;
            }
        }
    }

    private bool isCard(Collider other) {
        return other.name.Contains("Card");
    }

    private void Start() {
        cards = new Card[NUM_SALEMCARDS];
    }
}
