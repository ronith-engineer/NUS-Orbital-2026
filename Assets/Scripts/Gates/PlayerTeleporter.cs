using UnityEngine;
using Unity.Cinemachine;

public class PlayerTeleporter : MonoBehaviour
{
    private const int ShadowZoneLayer = 13;

    public static void TeleportPlayer(Transform player, Transform destination, CinemachineCamera cam)
    {
        if (player == null || destination == null) return;

        Vector3 delta = destination.position - player.position;
        player.position = destination.position;

        //instant position snaps can skip shadow's ontriggerenter2d, hence this shadow check
        Player playerComponent = player.GetComponent<Player>();
        if (playerComponent != null)
        {
            int shadowMask = 1 << ShadowZoneLayer; //bitwise shift to get binary representation of shadowzonelayer
            Collider2D shadowCheck = Physics2D.OverlapPoint(player.position, shadowMask);
            Debug.Log($"[Teleport] Position: {player.position}, ShadowCheck hit: {(shadowCheck != null ? shadowCheck.name : "none")}");
            playerComponent.SetInShadow(shadowCheck != null);
        }

        if (cam != null)
        {
            cam.OnTargetObjectWarped(player, delta);
        }
    }
}