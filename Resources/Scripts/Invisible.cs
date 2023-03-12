
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Invisible : UdonSharpBehaviour
{
    private const float DISTANCE = 0.2f;
    public void Update() {
        Vector3 pos = Networking.LocalPlayer.GetBonePosition(HumanBodyBones.Head);
        Quaternion rot = Networking.LocalPlayer.GetBoneRotation(HumanBodyBones.Head);
        Vector3 targetPos = pos + rot * Vector3.forward * DISTANCE;
        Vector3 forward = pos + rot * Vector3.forward * DISTANCE * 2;
        transform.position = targetPos;
        transform.LookAt(forward);
    }
}
