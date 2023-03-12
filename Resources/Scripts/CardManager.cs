
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using System;

public class CardManager : UdonSharpBehaviour
{
    [Header("GameManager")]
    public GameManager manager;

    [Header("All Card")]
    public Card[] allCards;

    [Header("Card")]
    public Card[] Card_Salem;
    public Card[] Card_TownHall;
    public Card[] Card_NotAWitch;
    public Card[] Card_Witch;
    public Card Card_Constable;

    [Header("Special Card")]
    public Card BlackCat;
    public Card Conspyracy;
    public Card Night;

    [Header("Kill Card")]
    public Card[] Card_Kill;

    [Header("Deck")]
    public Card[] tryalDeck;
    public Card[] charDeck;
    public Card[] salemDeck;

    [Header("Discard")]
    public Discard discard;

    [Header("KillSlot")]
    public KillSlot killSlot;

    [Header("Const")]
    public int NUM_TRYAL_CARDS = 3;

    private const int NUM_SALEM_CARDS = 3;

    private int[] numNotWitch = {18, 23, 28, 32, 29, 33, 27};
    private int[] numWitch = {1, 1, 2, 2, 2, 2, 2};
    private int[] numPerPerson = {5, 5, 5, 5, 4, 4, 3};

    private bool _timer = false;
    private float _delta = 0f;
    private Player[] _players;
    private int _num = 0;
    private int _curPlayer = 0;


    public void OnShuffle() {
        SetSalem();
        SetTryal();
        SetTownHall();
    }

    public void OnTakeOwnership() {
        foreach(Card c in allCards) {
            Networking.SetOwner(Networking.LocalPlayer, c.gameObject);
        }
    }

    public void OnReset() {
        foreach(Card c in allCards) {
            c.OnReset();
        }
        salemDeck = new Card[Card_Salem.Length];
        charDeck = new Card[Card_TownHall.Length];
    }

    public void AddSpecialAndShuffle() {
        AddSpecialCard(Conspyracy);
        Shuffle(salemDeck);
        AddSpecialCard(Night);
    }

    public void CreateDeckFromDiscards() {
        Card[] cards = discard.cards;
        int size = discard.GetSize();
        salemDeck = new Card[size];
        for (int i = 0; i < size; ++i) {
            if (cards[i] != Night) {
                salemDeck[i] = cards[i];
            }
        }
        Shuffle(salemDeck);
        AddNight(ref salemDeck);
        foreach (Card c in salemDeck) {
            c.OnReset();
        }
        discard.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "OnReset");
    }
    public void TakeOwnerFromDiscards() {
        foreach(Card c in discard.cards) {
            if (c != null) {
                Networking.SetOwner(Networking.LocalPlayer, c.gameObject);
            }
        }
    }
    private void AddNight(ref Card[] deck) {
        for (int i = 0; i < deck.Length; ++i) {
            if (deck[i] == null && i < deck.Length - 1) {
                deck[i] = deck[i + 1];
                deck[i + 1] = null;
            }
        }
        deck[deck.Length - 1] = Night;
    }

    public void HideCards() {
        if (!Networking.IsOwner(gameObject)) {
            foreach (Card c in allCards) {
                c.gameObject.SetActive(false);
            }
        }
    }
    public void ShowCards() {
        if (!Networking.IsOwner(gameObject)) {
            foreach (Card c in allCards) {
                c.gameObject.SetActive(true);
            }
        }
    }

    public void DealCardToPlayer() {
        Player[] players = manager.playerManager.players;
        int curPlayer = manager.playerManager.curPlayer;
        foreach (Player p in players) {
            if (p.IsJoined) {
                DealToTownHall(p);
                DealToTryal(p, curPlayer);
                DealToSalem(p);
            }
        }
        foreach(Card c in Card_Kill) {
            c.MoveToCard(killSlot.transform);
        }
    }
    private void SetSalem() {
        Array.Copy(Card_Salem, salemDeck, Card_Salem.Length);
        Shuffle(salemDeck);
    }
    private void AddSpecialCard(Card card) {
        Card[] newDeck = new Card[salemDeck.Length + 1];
        Array.Copy(salemDeck, newDeck, salemDeck.Length);
        newDeck[newDeck.Length - 1] = card;
        salemDeck = new Card[newDeck.Length];
        salemDeck = newDeck;

    }
    private void SetTryal() {
        int numOfPlayers = manager.playerManager.curPlayer - 4;
        int total = numNotWitch[numOfPlayers] + numWitch[numOfPlayers] + 1;
        tryalDeck = new Card[total];
        for (int i = 0; i < numNotWitch[numOfPlayers]; ++i) {
            tryalDeck[i] = Card_NotAWitch[i];
        }
        int cntWitch = 0;
        for (int i = numNotWitch[numOfPlayers]; i < numNotWitch[numOfPlayers] + numWitch[numOfPlayers]; ++i) {
            tryalDeck[i] = Card_Witch[cntWitch++];
        }
        tryalDeck[tryalDeck.Length - 1] = Card_Constable;

        Shuffle(tryalDeck);
    }
    private void SetTownHall() {
        Array.Copy(Card_TownHall, charDeck, Card_TownHall.Length);
        Shuffle(charDeck);
    }

    private void DealToTownHall(Player p) {
        if (charDeck.Length > 0) {
            Card draw = charDeck[0];
            Skip(ref charDeck, 1);
            p.MoveTownhallCard(draw);
        }
    }
    private void DealToTryal(Player p , int num) {
        if (tryalDeck.Length > 0) {
            for (int i = 0; i < numPerPerson[num-4]; ++i) {
                Card draw = tryalDeck[0];
                Skip(ref tryalDeck, 1);
                p.MoveTryalCard(draw, i);
                draw.Flip();
            }
        }
    }
    private void DealToSalem(Player p) {
        for (int j = 0; j < NUM_SALEM_CARDS; ++j) {
            Card drawCard = DrawSalemCard();
            p.MoveSalemCard(drawCard);
        }
    }
    public Card DrawSalemCard() {
        Debug.Log(salemDeck.Length);
        if (salemDeck.Length > 0) {
            Card temp = salemDeck[0];
            Skip(ref salemDeck, 1);
            return temp;
        }
        Debug.Log("salemDeck is Empty");
        return null;
    }
    private void Skip(ref Card[] deck, int num) {
        Card[] newDeck = new Card[deck.Length - num];
        for (int i = 0; i < newDeck.Length; i++) {
            newDeck[i] = deck[i + num];
        }
        deck = newDeck;
    }
    private void Shuffle(Card[] deck) {
        for (int i = deck.Length - 1; i >= 1; i--) {
            int j = UnityEngine.Random.Range(0, i + 1);
            Card temp = deck[i];
            deck[i] = deck[j];
            deck[j] = temp;
        }
    }

    private void Start() {
        salemDeck = new Card[Card_Salem.Length];
        charDeck = new Card[Card_TownHall.Length];
    }

}
