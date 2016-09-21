using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

    public float m_cameraMax = 8;
    public float m_cameraMin = 3;
    public float m_cameraMoveSpeed = 1.5f;
    public float m_cameraScrollSpeed = 1.5f;

    private float m_cameraScroll;
    private Camera m_camera;
    private float m_cameraCurSize;

    private Vector3 m_lastMousePos;
    private Vector3 m_curMousePos;

    // Use this for initialization
    void Start ()
    {
        m_camera = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        CameraScroll();
        CameraMove();
    }

    //
    void FixedUpdate()
    {

    }

    //
    void CameraMove()
    {
        if (Input.GetMouseButtonDown(2))
        {
            //Debug.Log("Pressed middle click.");
            m_lastMousePos.x = Input.mousePosition.x;
            m_lastMousePos.z = Input.mousePosition.y;
        }
        else if(Input.GetMouseButton(2))
        {
            m_curMousePos.x = Input.mousePosition.x;
            m_curMousePos.z = Input.mousePosition.y;
            transform.position -= (m_curMousePos - m_lastMousePos) * Time.deltaTime * m_cameraMoveSpeed;
            m_lastMousePos = m_curMousePos;
        }
    }

    //
    void CameraScroll()
    {
        m_cameraScroll = Input.GetAxis("Mouse ScrollWheel");
        if (m_cameraScroll != 0)
        {
            m_cameraCurSize = m_camera.orthographicSize;
            m_cameraCurSize -= m_cameraScroll * m_cameraScrollSpeed;
            m_cameraCurSize = m_cameraCurSize > m_cameraMax ? m_cameraMax : m_cameraCurSize;
            m_cameraCurSize = m_cameraCurSize < m_cameraMin ? m_cameraMin : m_cameraCurSize;
            m_camera.orthographicSize = m_cameraCurSize;
        }
    }
}
