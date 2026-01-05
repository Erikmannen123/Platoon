using UnityEngine;

public class Objective : MonoBehaviour
{
    [SerializeField] private float capPoints = 0f;

    [SerializeField] private Material blueMat;
    [SerializeField] private Material redMat;
    [SerializeField] private Material nutralMat;

    [SerializeField] private MeshRenderer mesh;

    public enum ObjEnum { Blue, Red, Neutral };

    public ObjEnum Obj = ObjEnum.Neutral;

    [SerializeField] private int ticketGain = 10;

    TicketCounter ticketCounter;

    private void Awake()
    {
        if (ticketCounter == null)
        {
            ticketCounter = FindAnyObjectByType<TicketCounter>();
        }
    }

    private void FixedUpdate()
    {
        LayerMask BlueLayer = new LayerMask();
        BlueLayer += LayerMask.GetMask("Blue");

        Collider[] Blue = Physics.OverlapSphere(this.transform.position, 10 * this.transform.localScale.x, BlueLayer);

        LayerMask RedLayer = new LayerMask();
        RedLayer += LayerMask.GetMask("Red");

        Collider[] Red = Physics.OverlapSphere(this.transform.position, 10 * this.transform.localScale.x, RedLayer);


        if(Blue.Length > Red.Length && capPoints <= 1f)
        {
            capPoints = Cap(true, capPoints);
        }
        else if (Red.Length > Blue.Length && capPoints >= -1f)
        {
            capPoints = Cap(false, capPoints);
        }

        if(capPoints >= 1f)
        {
            if(Obj != ObjEnum.Blue)
            {
                Debug.Log("blue Caped");
                ticketCounter.ModifyTicketCount("Blue", ticketGain);
                Obj = ObjEnum.Blue;
                mesh.material = blueMat;
            }
        }
        else if(capPoints <= -1f)
        {
            if (Obj != ObjEnum.Red)
            {
                Debug.Log("red Caped");
                ticketCounter.ModifyTicketCount("Red", ticketGain);
                Obj = ObjEnum.Red;
                mesh.material = redMat;
            }
        }
        else
        {
            if (Obj != ObjEnum.Neutral)
            {
                Obj = ObjEnum.Neutral;
                mesh.material = nutralMat;
            }
        }
    }


    float Cap(bool BlueCaping, float capPoints)
    {
        if (BlueCaping)
        {
            capPoints += 0.1f * Time.deltaTime;
        }
        else
        {
            capPoints += -0.1f * Time.deltaTime;
        }

        return capPoints;
    }
}
