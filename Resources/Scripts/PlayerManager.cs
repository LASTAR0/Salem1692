
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class PlayerManager : UdonSharpBehaviour
{
    [Header("GameManager")]
    public GameManager manager;
    public Player[] players;
    public VRCPlayerApi[] joinedPlayer;

    [HideInInspector] public int curPlayer = 0;
    private const int MAX_PLAYER = 10;

    public void SetJoinedPlayers() {
        for (int i = 0; i < players.Length; ++i) {
            VRCPlayerApi owner = Networking.GetOwner(players[i].gameObject);
            if (players[i].IsJoined) {
                curPlayer++;
                AddPlayer(owner);
            } else {
                players[i].SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "HideBoard");
            }
        }
    }
    public void DisableNonJoin() {
        for (int i = 0; i < players.Length; ++i) {
            if (!players[i].IsJoined)
                players[i].SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "DisableObject");
        }
    }
    private void AddPlayer(VRCPlayerApi player) {
        for (int i = 0; i < joinedPlayer.Length; ++i) {
            if (joinedPlayer[i] == null) {
                joinedPlayer[i] = player;
                return;
            }
        }
    }
    private void SortJoinedPlayers() {
        for (int i = 0; i < joinedPlayer.Length; ++i) {
            if (joinedPlayer[i] == null) {
                // null 칸 없애기
            }
        }
    }
    public void OnTakeOwnership() {
        foreach(Player p in players) {
            p.OnTakeOwnership();
        }
    }
    public void OnReset() {
        for (int i = 0; i < joinedPlayer.Length; ++i)
            joinedPlayer[i] = null;
        foreach(Player p in players) {
            p.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "OnReset");
        }
        curPlayer = 0;
    }
    public void Start() {
        joinedPlayer = new VRCPlayerApi[MAX_PLAYER];
    }
}
