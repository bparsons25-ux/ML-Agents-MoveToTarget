using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera topDownCamera;
    public Camera standViewCamera;
    public Camera finishLineCamera;
    public Camera followCamera;

    private void Start()
    {
        // top down camera is default camera on start - this 
        topDownCamera.enabled = true; 
        followCamera.enabled = false; 
        standViewCamera.enabled = false;
        followCamera.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && (topDownCamera.enabled == true || standViewCamera.enabled == true || finishLineCamera.enabled == true))
        {
            // if C is pressed then it switches to the follow view
            topDownCamera.enabled = false;
            standViewCamera.enabled = false;
            finishLineCamera.enabled = false;

            followCamera.enabled = true;
        }

        else if (Input.GetKeyDown(KeyCode.X) && followCamera.enabled == true || standViewCamera.enabled == true || finishLineCamera.enabled == true)
        {
            // If X is Pressed then it switches to the top down view
            standViewCamera.enabled = false;
            finishLineCamera.enabled = false;
            followCamera.enabled = false;

            topDownCamera.enabled = true;
        }

        else if (Input.GetKeyDown(KeyCode.V)  &&  topDownCamera.enabled == true || followCamera.enabled == true || finishLineCamera.enabled == true)
        {
            // If V is pressed - then it switches to stand view 
            topDownCamera.enabled = false;
            followCamera.enabled = false;
            finishLineCamera.enabled = false;

            standViewCamera.enabled = true;
        }

        else if (Input.GetKeyDown(KeyCode.B) && topDownCamera.enabled == true || followCamera.enabled == true || standViewCamera.enabled == true)
        {
            // If B is pressed - then it switches to finish line view
            topDownCamera.enabled = false;
            followCamera.enabled = false;
            standViewCamera.enabled = false;

            finishLineCamera.enabled = true;
        }

        
    }


}
