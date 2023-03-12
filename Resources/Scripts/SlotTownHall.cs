
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class SlotTownHall : UdonSharpBehaviour
{
    public Player player;
    public GameObject slotCard;
    [Header("TownHall")]
    public string type = "TownHall";

    public void OnTriggerEnter(Collider other) {
        if (!isCard(other) || slotCard != null)
            return;
        Card card = other.GetComponent<Card>();
        if (card == null)
            return;
        if (card.type != type || card.isUsed) {
            Debug.Log($"{card.name} type is not same");
            return;
        }
        VRC_Pickup pickup = other.GetComponent<VRC_Pickup>();
        if (Networking.LocalPlayer != Networking.GetOwner(gameObject)) {
            if (Networking.GetOwner(card.gameObject) == Networking.LocalPlayer) {
                pickup.Drop();
                MoveToCard(card);
            }
            return;
        }
        //Debug.Log($"{gameObject.name}'s slot is {card.name}");
        Networking.SetOwner(Networking.LocalPlayer, other.gameObject);
        pickup.Drop();

        slotCard = card.gameObject;
        card.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "Use");

        MoveToCard(card);
    }
    public void OnTriggerExit(Collider other) {
        if (Networking.LocalPlayer != Networking.GetOwner(gameObject))
            return;
        if (!isCard(other) || slotCard != other.gameObject)
            return;

        Card card = slotCard.GetComponent<Card>();
        card.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "UnUse");
        slotCard = null;
    }
    private void MoveToCard(Card card) {
        card.transform.position = transform.position;
        card.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0f);
    }
    private bool isCard(Collider other) {
        return other.name.Contains("Card");
    }
}
