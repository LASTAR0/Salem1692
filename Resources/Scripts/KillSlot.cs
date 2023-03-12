
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using System;

public class KillSlot : UdonSharpBehaviour
{
    /*
    [Header("PlayerManager")]
    public PlayerManager playerManager;
    public GameObject List;
    public KillButton[] buttons;
    public Material[] cardImage;
    */
    public Card[] cards;
    public string type = "Kill";
    public bool isOpenList = false;

    private const int NUM_CARDS = 15;

    /*
    public override void Interact() {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, "Execute");
    }
    public void Execute() {
        if (!isOpenList) {
            isOpenList = true;
            Player[] players = playerManager.players;
            foreach(Player p in players) {
                if (Networking.GetOwner(p.gameObject) == Networking.LocalPlayer && p.IsJoined && p.isWitch) {
                    List.SetActive(true);
                    ShowRemainList();
                    

                }
            }
        }
    }

    public void ShowRemainList() {
        Player[] players = playerManager.players;
        for (int i = 0; i < players.Length; ++i) {
            if (players[i].IsJoined) {
                buttons[i].gameObject.SetActive(true);
                
                //ChangeMaterials(buttons[i]);
            }
        }
    }
    */
    public void OnTriggerEnter(Collider other) {
        if (isCard(other)) {
            Card card = other.GetComponent<Card>();
            if (card == null) {
                return;
            }
            if (card.type != type || card.isUsed) {
                return;
            }
            card.isUsed = true;
            VRC_Pickup pickup = other.GetComponent<VRC_Pickup>();
            pickup.Drop();

            // move card
            for (int i = 0; i < cards.Length; ++i) {
                if (cards[i] == null) {
                    cards[i] = card;
                    Vector3 pos = transform.position;
                    pos.y += i * 0.001f;
                    card.transform.position = pos;
                    card.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 180f);
                    return;
                }
            }
        }
    }
    public void OnTriggerExit(Collider other) {
        if (isCard(other)) {
            Card card = other.GetComponent<Card>();
            if (card == null) {
                return;
            }
            if (card.type != type || !card.isUsed) {
                return;
            }
            card.isUsed = false;
            for (int i = 0; i < cards.Length; ++i) {
                if (cards[i] == card) {
                    cards[i] = null;
                    return;
                }
            }
        }
    }
    private bool isCard(Collider other) {
        return other.name.Contains("Card");
    }
    
    private void Start() {
        cards = new Card[NUM_CARDS];
    }
}
