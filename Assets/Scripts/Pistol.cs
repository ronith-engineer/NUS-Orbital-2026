using System.Collections;
using UnityEngine;

public class Pistol : MonoBehaviour
{

    [SerializeField] private Transform firePoint;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private GameObject muzzleFlash;
    private Player player;
    private Coroutine shootCoroutine;


    void Awake()
    {
        player = GetComponent<Player>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            shootCoroutine = StartCoroutine(Shoot());
        }

    }

    IEnumerator Shoot()
    {

        RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, firePoint.right);
        Debug.Log("firePoint.right: " + firePoint.right);

        if (hitInfo)
        {
            Debug.Log("Hit: " + hitInfo.transform.name);
            Enemy enemy = hitInfo.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage();
            }
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, hitInfo.point);
        }
        else
        {
            
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, firePoint.position + firePoint.right * 100f);
        }

        lineRenderer.enabled = true;

        muzzleFlash.SetActive(true);

        yield return new WaitForSeconds(0.02f);
        //wait for a short time and then disable the line renderer
        
        lineRenderer.enabled = false;
        muzzleFlash.SetActive(false);

    }
}
