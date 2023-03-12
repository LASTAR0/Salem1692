
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Player : UdonSharpBehaviour
{
    [UdonSynced] public bool IsJoined = false;
    [UdonSynced] public bool IsAlive = false;

    public GameObject joinButton;
    public GameObject board;
    public HandBall Hand;
    public SlotTownHall slotTownhall;
    public Slot[] slotTryal;

    public bool isWitch = false;
    public bool isConstable = false;

    [UdonSynced] public bool isReset = false;

    public void OnJoinPlayer() {
        OnTakeOwnership();
        IsJoined = true;
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "ShowBoard");
        RequestSerialization();
    }
    public void ShowBoard() {
        joinButton.SetActive(false);
        board.SetActive(true);
        Hand.gameObject.SetActive(true);
    }
    public void HideBoard() {
        joinButton.SetActive(false);
        board.SetActive(false);
        Hand.gameObject.SetActive(false);
    }
    public void ResetBoard() {
        Debug.Log("resetboard");
        joinButton.SetActive(true);
        board.SetActive(false);
        Hand.gameObject.SetActive(false);
    }
    public void MoveTownhallCard(Card card) {
        card.transform.position = slotTownhall.transform.position;
        card.transform.rotation = slotTownhall.transform.rotation;
    }
    public void MoveTryalCard(Card card , int num) {
        card.transform.position = slotTryal[num].transform.position;
        card.transform.rotation = slotTryal[num].transform.rotation;
    }
    public void MoveSalemCard(Card card) {
        card.transform.position = Hand.transform.position;
        card.transform.rotation = Hand.transform.rotation;
    }
    public void EnableWitch() {
        Debug.Log("당신은 마녀입니다");
        isWitch = true; 
    }
    public void DisableWitch() { isWitch = false; }
    public void EnableConstable() {
        Debug.Log("당신은 경찰입니다");
        isConstable = true; 
    }
    public void DisableConstable() {
        Debug.Log("이제 경찰이 아닙니다");
        isConstable = false; 
    }
    public void OnTakeOwnership() {
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        Networking.SetOwner(Networking.LocalPlayer, Hand.gameObject);
    }
    public void OnReset() {
        IsJoined = false;
        ResetBoard();
        DisableWitch();
        DisableConstable();
    }

    public override void OnDeserialization() {
        if (IsJoined) {
            ShowBoard();
        }
    }
    public override void OnPreSerialization() {
        if (IsJoined) {
            ShowBoard();
        }
    }
}
