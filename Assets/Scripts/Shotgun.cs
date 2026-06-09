using System.Collections;
using UnityEngine;

public class Shotgun : MonoBehaviour
{

    [SerializeField] private Transform firePoint;
    [SerializeField] private LineRenderer lineRenderer;
    private Player player;
    private Coroutine shootCoroutine;
    public int currentAmmo = 4;
    private Animator anim;


    void Awake()
    {
        player = GetComponentInParent<Player>();
        anim = GetComponentInChildren<Animator>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && currentAmmo > 0)
        {
            shootCoroutine = StartCoroutine(Shoot());
        }

    }

    IEnumerator Shoot()
    {
        currentAmmo--;
        RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, firePoint.right);
        Debug.Log("firePoint.right: " + firePoint.right);

        if (hitInfo)
        {
            Debug.Log("Hit: " + hitInfo.transform.name);
            Enemy enemy = hitInfo.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(player.facingRight);
            }
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, hitInfo.point);
        }
        else
        {

            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, firePoint.position + firePoint.right * 100f);
        }
        anim.SetTrigger("shoot");
        lineRenderer.enabled = true;

        yield return new WaitForSeconds(0.02f);
        //wait for a short time and then disable the line renderer

        lineRenderer.enabled = false;


    }
}
