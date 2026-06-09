using UnityEngine;

public class Knife : MonoBehaviour
{
    private Animator anim;
    public int knifeCurrentDurability = 3;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }
    private void Update()
    {
        if (knifeCurrentDurability <= 0) // If the knife has no durability left, disable the knife GameObject
        {
            gameObject.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && knifeCurrentDurability > 0) // If the left mouse button is pressed and the knife has durability, perform an attack

        {
            knifeCurrentDurability--;
            anim.SetTrigger("knifeAttack");
        }
    }
}
