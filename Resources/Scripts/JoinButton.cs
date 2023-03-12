
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class JoinButton : UdonSharpBehaviour
{
    public Player player;
    
    public override void Interact() {
        if (player == null) {
            Debug.Log("JoinButton Something wrong");
            return;
        }
        player.OnJoinPlayer();
    }
}
