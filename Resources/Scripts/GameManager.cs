
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class GameManager : UdonSharpBehaviour
{
    public CardManager cardManager;
    public PlayerManager playerManager;
    public ObjectManager objectManager;
    public NightMode nightMode;

    [UdonSynced] public bool isPlaying = false;

    public void OnPlay() {
        if (isPlaying) 
            return;
        isPlaying = true;
        RequestSerialization();
        OnCheck();
    }
    public void OnCheck() {
        HideCards();
        //objectManager.ShowObjects();
        playerManager.SetJoinedPlayers();
        SendCustomEventDelayedSeconds("OnShuffle", 0.3f, VRC.Udon.Common.Enums.EventTiming.Update);
    }

    public void OnReStart() {
        if (!isPlaying)
            return;
        isPlaying = false;
        RequestSerialization();
        //objectManager.HideObjects();
        OnTakeOwnership();
    }
    public void OnTakeOwnership() {
        cardManager.OnTakeOwnership();
        playerManager.OnTakeOwnership();
        SendCustomEventDelayedSeconds("OnReset", 0.5f, VRC.Udon.Common.Enums.EventTiming.Update);
    }
    public void OnReset() {
        cardManager.OnReset();
        playerManager.OnReset();
    }
    public void OnShuffle() {
        //Debug.Log($"{playerManager.curPlayer}");
        cardManager.OnShuffle();
        cardManager.DealCardToPlayer();
        SendCustomEventDelayedSeconds("AddShuffle", 0.5f, VRC.Udon.Common.Enums.EventTiming.Update);
    }
    public void AddShuffle() {
        cardManager.AddSpecialAndShuffle();
        SendCustomEventDelayedSeconds("ShowCards", 0.5f, VRC.Udon.Common.Enums.EventTiming.Update);
    }
    public void EnableNightWitch() {
        if (Networking.LocalPlayer == Networking.GetOwner(gameObject)) {
            nightMode.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "EnableNightWitch");
        }
    }
    public void EnableNightConstable() {
        if (Networking.LocalPlayer == Networking.GetOwner(gameObject)) {
            nightMode.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "EnableNightConstable");
        }
    }

    public void HideCards() {
        cardManager.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "HideCards");
    }
    public void ShowCards() {
        cardManager.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "ShowCards");
    }

    public override void OnPlayerJoined(VRCPlayerApi player) {
        if (Networking.LocalPlayer == Networking.GetOwner(gameObject)) {
            
        }
    }
    public override void OnPlayerLeft(VRCPlayerApi player) {
        
    }
}
