
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Card : UdonSharpBehaviour
{
    public GameObject cover;
    [Header("TownHall Tryal Salem")]
    public string type;
    // Used Somewhere?
    public bool isUsed = false;
    public bool isWitchCard = false;
    public bool isConstableCard = false;

    public Transform trParent;

    public override void OnPickup() {
        if (isUsed) {
            transform.SetParent(trParent);
        }
        if (cover != null) {
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "EnableCover");
            DisableCover();
        }
    }
    public override void OnDrop() {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "DisableCover");
    }
    public void MoveToCard(Transform tr) {
        transform.position = tr.position;
        transform.rotation = tr.rotation;
    }
    public void EnableObjectAll() { gameObject.SetActive(true); }
    public void DisableObjectAll () { gameObject.SetActive(false); }
    public void Use() { isUsed = true; }
    public void UnUse() { isUsed = false; }
    public void EnableObjectGuest() {
        //Debug.Log($"Enable Card {gameObject.name}");
        if (!Networking.IsOwner(gameObject)) {
            gameObject.SetActive(true);
            //Debug.Log($"Enable Card {gameObject.name}");
        }
    }
    public void DisableObjectGuest() {
        if (!Networking.IsOwner(gameObject)) {
            //Debug.Log($"Disable Card {gameObject.name}");
            gameObject.SetActive(false);
        }
    }
    public void EnableCover() {
        if (cover == null)
            return;
        cover.SetActive(true);
    }
    public void DisableCover() {
        if (cover == null)
            return;
        cover.SetActive(false);
    }
    public void Flip() {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 180f);
    }
    public void OnReset() {
        transform.parent = trParent;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "UnUse");
    }
    public void Start() {
        trParent = transform.parent;
    }
}
