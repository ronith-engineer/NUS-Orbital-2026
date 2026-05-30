using CodeMonkey.Utils;
using UnityEngine;

public class PlayerAimWeapon : MonoBehaviour
{
    private Transform aimTransform;
    private Vector3 aimLocalScale;
    private Vector3 aimCurrentLocalScale;
    private Entity player;
    public Vector3 aimDirection;



    private void Awake()
    {
        aimTransform = transform.Find("Aim");
        aimLocalScale = aimTransform.localScale;
        player = GetComponent<Entity>();
    }

    private void Update()
    {
        Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();
        aimDirection = (mousePosition - aimTransform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        
        aimCurrentLocalScale = aimLocalScale;

        if (aimDirection.x < 0)
        {
            aimCurrentLocalScale.y = -aimCurrentLocalScale.y;
            aimTransform.localEulerAngles = new Vector3(0, 0,180 - angle);
        }
        else
        {
            aimCurrentLocalScale.y = -aimCurrentLocalScale.y;
            aimTransform.localEulerAngles = new Vector3(0, 0, angle);
        }
        aimLocalScale = aimCurrentLocalScale;

        Debug.DrawRay(aimTransform.position, aimTransform.right * 2f, Color.red);

    }

}