using UnityEngine;
using Unity.Cinemachine;

public class PlayerTeleporter : MonoBehaviour
{
    public static void TeleportPlayer(Transform player, Transform destination, CinemachineCamera cam)
    {
        if (player == null || destination == null) return;

        Vector3 delta = destination.position - player.position;
        player.position = destination.position;
        
       
        if (cam != null )
        {
            cam.OnTargetObjectWarped(player, delta);
        }
    }
}
