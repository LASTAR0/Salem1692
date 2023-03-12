
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Descriptor : UdonSharpBehaviour
{
    public float walkSpeed = 2f;
    public float runSpeed = 3f;
    public float jump = 4f;

    void Start() {
        VRCPlayerApi lp = Networking.LocalPlayer;
        lp.SetWalkSpeed(walkSpeed);
        lp.SetRunSpeed(runSpeed);
        lp.SetJumpImpulse(jump);
    }
}
