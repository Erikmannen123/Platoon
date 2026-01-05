using UnityEngine;
using UnityEngine.UIElements;

public class Health : MonoBehaviour, I_Health
{
    [SerializeField] public float health = 100f;

    public Spawn spawn;

    TicketCounter ticketCounter;

    private void Awake()
    {
        if(ticketCounter == null)
        {
            ticketCounter = FindAnyObjectByType<TicketCounter>();
        }
    }

    public void Hit(float Damage)
    {
        health -= Damage;

        if(health <= 0f)
        {
            Debug.Log("Dead " + this.gameObject.tag);
            ticketCounter.ModifyTicketCount(this.gameObject.tag, -1);
            spawn.AddToQueue(this.gameObject);
            this.gameObject.SetActive(false);
        }
    }
}
