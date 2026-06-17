using UnityEngine;
using UnityEngine.UI;  
public class PistolUI : MonoBehaviour
{
    private Image bullet1;
    private Image bullet2;
    private Image bullet3;
    private Image bullet4;
    private Image bullet5;
    private Image bullet6;
    private int currentAmmo;

    [SerializeField] private Pistol pistol;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitializeBullets(); // Initialize bullet references
        ResetBulletsToGray(); // Set all bullets to gray at the start
    }
    void Update()
    {
        currentAmmo = pistol.currentAmmo;
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
                        if (currentAmmo > 4)
                        {
                            bullet5.color = new Color32(255, 255, 255, 255);
                            if (currentAmmo > 5)
                            {
                                bullet6.color = new Color32(255, 255, 255, 255);
                            }
                            else
                            {
                                bullet6.color = new Color32(125, 125, 125, 255);
                            }
                        }
                        else
                        {
                            bullet5.color = new Color32(125, 125, 125, 255);
                        }
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
        bullet1 = GetComponentsInChildren<Image>()[1];
        bullet2 = GetComponentsInChildren<Image>()[2];
        bullet3 = GetComponentsInChildren<Image>()[3];
        bullet4 = GetComponentsInChildren<Image>()[4];
        bullet5 = GetComponentsInChildren<Image>()[5];
        bullet6 = GetComponentsInChildren<Image>()[6];
    }
    private void ResetBulletsToGray()
    {
        bullet1.color = new Color32(125, 125, 125, 255);
        bullet2.color = new Color32(125, 125, 125, 255);
        bullet3.color = new Color32(125, 125, 125, 255);
        bullet4.color = new Color32(125, 125, 125, 255);
        bullet5.color = new Color32(125, 125, 125, 255);
        bullet6.color = new Color32(125, 125, 125, 255);
    }


}
