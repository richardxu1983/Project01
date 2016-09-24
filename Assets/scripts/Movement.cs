using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{
    public float m_camRayLength = 100;

    private Ray m_camRay;
    private int m_floorMask;
    private RaycastHit m_floorHit;
    private NavigationAgentComponent m_navigationAgent;

    // Use this for initialization
    void Start ()
    {
        m_navigationAgent = GetComponent<NavigationAgentComponent>();
      
        // Create a layer mask for the floor layer.
        m_floorMask = LayerMask.GetMask("Ground");
    }


	// Update is called once per frame
	void Update ()
    {
        GetInput();
    }
    
    //
    private void GetInput()
    {
        //movement.Set(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));

        if (Input.GetMouseButtonDown(1))
        {
            // Create a ray from the mouse cursor on screen in the direction of the camera.
            m_camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Perform the raycast and if it hits something on the floor layer...
            if (Physics.Raycast(m_camRay, out m_floorHit, m_camRayLength, m_floorMask))
            {
                // Create a vector from the player to the point on the floor the raycast from the mouse hit.
                if (null != m_navigationAgent)
                {
                    m_navigationAgent.MoveToPosition(m_floorHit.point, 0.2f);
                }
            }
        }
    }

}
