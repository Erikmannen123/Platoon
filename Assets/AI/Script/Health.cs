using UnityEngine;

public class Health : MonoBehaviour, I_Health
{
    [SerializeField] float health = 100f;

    public void Hit(float Damage)
    {
        health -= Damage;

        if(health <= 0f)
        {
            this.gameObject.SetActive(false);
        }
    }
}
