using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveToTargetAgent : Agent
{
    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private Color winColour = Color.green;
    [SerializeField] private Color loseColour = Color.red;
    [SerializeField] private Color idleColour = Color.grey;
    [SerializeField] private Color closeColour = Color.yellow;
    [SerializeField] private Transform spawn;
    public Renderer[] floors;
    public TMPro.TextMeshProUGUI winCounter;
    public TMPro.TextMeshProUGUI loseCounter;
    private int wincount;
    private int losecount;



    public override void OnEpisodeBegin() //Resets the Enviroment
    {
        transform.localPosition = spawn.transform.localPosition;
        //transform.localPosition = new Vector3(Random.Range(-1, 1f), 0.5f, Random.Range(-1f, 0f)); // picks a random x position between -4 and 4, while also picking a random z position between -4 and 0 
        //target.localPosition = new Vector3(Random.Range(-4f, 4f), 0.5f, Random.Range(1f, 4f)); // picks a random x position between -4 and 4, while also picking a random z position between 1 and 4 
    }

    public override void CollectObservations(VectorSensor sensor) // Obsreves what it knows
    {
        sensor.AddObservation(transform.localPosition); // 3
        sensor.AddObservation(target.localPosition);    // 3
    }

    public void Update()
    {
        loseCounter.text = "losses:" + losecount.ToString();
        winCounter.text = "Wins:" + wincount.ToString();
    }

    public override void OnActionReceived(ActionBuffers actions) // Acts on what the agent does
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        Vector3 move = new Vector3(moveX, 0f, moveZ);
        transform.localPosition += move * moveSpeed * Time.deltaTime;

        // Small penalty every step so it learns to hurry
        //if (MaxStep > 0) AddReward(-1f / MaxStep);
    }

    public override void Heuristic(in ActionBuffers actionsOut) // human test drives this
    {
        var continuous = actionsOut.ContinuousActions;
        var kb = Keyboard.current;
        if (kb == null) return;

        continuous[0] = (kb.dKey.isPressed ? 1f : 0f)
                      - (kb.aKey.isPressed ? 1f : 0f);
        continuous[1] = (kb.wKey.isPressed ? 1f : 0f)
                      - (kb.sKey.isPressed ? 1f : 0f);

    }

    public override void Initialize()   // runs once at startup
    {
        //floors[1].material.color = idleColour;
        foreach (Renderer floorRenderer in floors)
        {
            floorRenderer.material.color = idleColour;
            wincount = 0;
            losecount = 0;
            loseCounter.text = "losses:" + losecount.ToString();
            winCounter.text = "Wins:" + wincount.ToString(); 
        }
    }

    private void OnTriggerEnter(Collider other) // rewards (sees wether it's good or bad?)
    {
        if (other.CompareTag("Target"))
        {
            SetReward(10f);
            foreach (Renderer floorRenderer in floors)
            {
                floorRenderer.material.color = winColour;
            }
            wincount += 10;
            EndEpisode();
        }
        else if (other.CompareTag("Wall"))
        {
            SetReward(-2f);
            foreach (Renderer floorRenderer in floors)
            {
                floorRenderer.material.color = loseColour;
            }
            losecount++;
            //EndEpisode();
        }

        if (other.CompareTag("RewardBarrier"))
        {
            SetReward(2f);
            foreach (Renderer floorRenderer in floors)
            {
                floorRenderer.material.color = closeColour;
            }
            wincount++;
            other.gameObject.SetActive(false);
           
        }

        if (other.CompareTag("StartBarrier"))
        {
            SetReward(-15f);
            foreach (Renderer floorRenderer in floors)
            {
                floorRenderer.material.color = loseColour;
            }
            losecount++;
            EndEpisode();
        }

    }

}
