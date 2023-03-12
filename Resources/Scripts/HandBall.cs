
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class HandBall : UdonSharpBehaviour
{
    public float fanAngle = 200f;
    public float radius = 1.3f;
    public int maxCards = 12;
    [Header("Salem")]
    public string type;
    public GameObject[] handCard;

    public void OnTriggerEnter(Collider other) {
        Debug.Log("ontriggerenter");
        if (!isCard(other) || GetEmptySlot() == -1)
            return;
        Card card = other.GetComponent<Card>();
        if (card == null) {
            Debug.Log("card is null");
            return;
        }
        if (card.isUsed || card.type != type) 
            return;

        VRC_Pickup pickup = other.GetComponent<VRC_Pickup>();
        if (Networking.LocalPlayer != Networking.GetOwner(gameObject)) {
            if (Networking.LocalPlayer == Networking.GetOwner(card.gameObject)) {
                pickup.Drop();
            }
            return;
        }
        VRCPlayerApi owner = Networking.GetOwner(gameObject);
        Networking.SetOwner(owner, card.gameObject);
        int num = GetEmptySlot();
        handCard[num] = card.gameObject;

        Debug.Log(handCard[num]);
        
        pickup.Drop();

        card.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "Use");
        card.transform.SetParent(transform);
        card.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "EnableCover");
        card.DisableCover();
        MoveToCard(card , num);
    }
    public void OnTriggerExit(Collider other) {
        if (!isCard(other))
            return;

        if (Networking.GetOwner(gameObject) != Networking.LocalPlayer)
            return;
        Card card = other.GetComponent<Card>();
        if (card == null) {
            Debug.Log("card is null");
            return;
        }
        if (!card.isUsed)
            return;

        for (int i = 0; i < handCard.Length; ++i) {
            if (handCard[i] == card.gameObject) {
                card.transform.SetParent(card.trParent);
                card.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "UnUse");
                handCard[i] = null;
                return;
            }
        }
    }
    public int GetEmptySlot() {
        for (int i = 0; i < handCard.Length; ++i) {
            if (handCard[i] == null) {
                return i;
            }
        }
        Debug.Log("Hand is Full");
        return -1;
    }
    private void MoveToCard(Card card , int num) {
        float angle = num * fanAngle / (maxCards - 1) - fanAngle / 2;
        card.transform.localPosition = new Vector3(radius * Mathf.Sin(angle * Mathf.Deg2Rad), 0, radius * Mathf.Cos(angle * Mathf.Deg2Rad));
        card.transform.localRotation = Quaternion.Euler(0, angle, 0);
    }
    private bool isCard(Collider other) {
        return other.name.Contains("Card");
    }
    private void Start() {
        handCard = new GameObject[maxCards];
    }
}
