using UnityEngine;
using UnityEngine.UI;
public class ShotgunUI : MonoBehaviour
{
    private Image bullet1;
    private Image bullet2;
    private Image bullet3;
    private Image bullet4;

    private float currentAmmo;

    [SerializeField] private Shotgun shotgun;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitializeBullets(); // Initialize bullet references
        ResetBulletsToGray(); // Set all bullets to gray at the start
    }
    void Update()
    {
        currentAmmo = shotgun.currentAmmo;
        UpdateBulletDisplay();
    }

    private void UpdateBulletDisplay()
    {
        if (currentAmmo > 0)
        {
            bullet1.color = new Color32(255, 255, 255, 255);
            if (currentAmmo > 1)
            {
                bullet2.color = new Color32(255, 255, 255, 255);
                if (currentAmmo > 2)
                {
                    bullet3.color = new Color32(255, 255, 255, 255);
                    if (currentAmmo > 3)
                    {
                        bullet4.color = new Color32(255, 255, 255, 255);
                   
                    }
                    else
                    {
                        bullet4.color = new Color32(125, 125, 125, 255);
                    }

                }
                else
                {
                    bullet3.color = new Color32(125, 125, 125, 255);
                }
            }
            else
            {
                bullet2.color = new Color32(125, 125, 125, 255);
            }
        }
        else
        {
            bullet1.color = new Color32(125, 125, 125, 255);
        }
    }

    private void InitializeBullets()
    {
        bullet1 = transform.Find("Bullet1").GetComponent<Image>();
        bullet2 = transform.Find("Bullet2").GetComponent<Image>();
        bullet3 = transform.Find("Bullet3").GetComponent<Image>();
        bullet4 = transform.Find("Bullet4").GetComponent<Image>();
    }
    private void ResetBulletsToGray()
    {
        bullet1.color = new Color32(125, 125, 125, 255);
        bullet2.color = new Color32(125, 125, 125, 255);
        bullet3.color = new Color32(125, 125, 125, 255);
        bullet4.color = new Color32(125, 125, 125, 255);
    }


}
