
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

public class Option : UdonSharpBehaviour
{
    [Header("GameManager")]
    public GameManager manager;
    public Text masterName;
    public AudioSource sndButton;

    public void ReShuffle() {
        if (Networking.GetOwner(gameObject) == Networking.LocalPlayer) {
            playSound();
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, "Execute");
        }
    }
    public void Execute() {
        CardManager cm = manager.cardManager;
        if (cm.salemDeck.Length == 0) {
            //cm.TakeOwnerFromDiscards();
            cm.CreateDeckFromDiscards();
        }
    }
    public void OnPlay() {
        if (Networking.IsOwner(gameObject)) {
            playSound();
            manager.OnPlay();
        }
    }
    public void OnRestart() {
        if (Networking.IsOwner(gameObject)) {
            playSound();
            manager.OnReStart();
        }
    }
    public void playSound() {
        sndButton.Stop();
        sndButton.Play();
    }
    private void Start()
    {
        masterName.text = Networking.GetOwner(gameObject).displayName;
    }
}
