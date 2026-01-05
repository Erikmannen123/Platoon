using UnityEngine;
using TMPro;

public class TicketCounter : MonoBehaviour
{
    [SerializeField] int blueTickets = 20; 
    [SerializeField] int redTickets = 20;

    [SerializeField] private TextMeshProUGUI blueTicketCounter;
    [SerializeField] private TextMeshProUGUI redTicketCounter;

    private void Awake()
    {
        blueTicketCounter.text = blueTickets.ToString();
        redTicketCounter.text = redTickets.ToString();
    }

    public void ModifyTicketCount(string Tag, int Ammount)
    {
        if(Tag == "Blue")
        {
            blueTickets += Ammount;
            blueTicketCounter.text = blueTickets.ToString();
        }
        else if (Tag == "Red")
        {
            redTickets += Ammount;
            redTicketCounter.text = redTickets.ToString();
        }

        if (blueTickets <= 0)
        {
            Debug.Log("Red Won");
        }

        if (redTickets <= 0)
        {
            Debug.Log("Blue Won");
        }
    }
}
