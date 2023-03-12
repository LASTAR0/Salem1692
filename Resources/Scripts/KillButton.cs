
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class KillButton : UdonSharpBehaviour
{
    public KillSlot killSlot;

    public Material material;

    public override void Interact() {

    }

    private void Start() {
        material = GetComponent<Material>();
    }
}
