using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{
    public float m_speed;
    public float m_camRayLength = 100;

    private Vector3 movement;
    private Rigidbody m_Rigidbody;
    private Ray m_camRay;
    private int m_floorMask;
    private Vector3 m_desirdPos;
    private RaycastHit m_floorHit;


    // Use this for initialization
    void Start ()
    {
        movement = new Vector3(0f,0f,0f);
        m_Rigidbody = GetComponent<Rigidbody>();

        // Create a layer mask for the floor layer.
        m_floorMask = LayerMask.GetMask("Ground");

        //
        m_desirdPos = transform.position;
    }
	
    //
    void Awake()
    {
        m_desirdPos = transform.position;
    }


	// Update is called once per frame
	void Update ()
    {
        GetInput();
    }

    //
    private void FixedUpdate()
    {
        Move();
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
                m_desirdPos = m_floorHit.point;
            }
        }
    }

    //
    private void Move()
    {
        movement.Normalize();
        movement = movement * m_speed * Time.deltaTime;

        if(m_desirdPos != transform.position)
        {
            m_Rigidbody.MovePosition(m_desirdPos);
        }
    }
}
