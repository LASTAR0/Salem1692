
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ObjectManager : UdonSharpBehaviour
{
    public CardDeck cardDeck;
    public KillSlot killSlot;
    public Discard discard;
    public GameObject hammer;

    public void ShowObjects() {
    }
    public void HideObjects() {
    }
    public void ActiveHammer() { hammer.SetActive(true); }
    public void DeactiveHammer() { hammer.SetActive(false); }
    public override void OnPlayerJoined(VRCPlayerApi player) {
        if (Networking.GetOwner(gameObject) == Networking.LocalPlayer) {
        }
    }
}
