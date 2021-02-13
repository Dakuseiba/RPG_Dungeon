using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform cameraTransform;
    public Transform Rotate;

    [Header("Edge")]
    public bool useEdge;
    public float edgeSize;

    [Header("Speed")]
    public float normalSpeed;
    public float fastSpeed;
    public float speedRotationButton;
    public float speedRotationMouse;
    public float movementTime;

    [Header("Angle")]
    public float maxAngle;
    public float minAngle;

    [Header("Zoom")]
    public float maxZoom;
    public float minZoom;
    public Vector3 zoomAmount;

    [Header("Border for Camera")]
    [SerializeField]BorderCamera borderCamera = new BorderCamera();

    float speed;
    float RotX;
    float RotY;

    Vector3 newPoistion;
    Vector3 newZoom;

    void Start()
    {
        newPoistion = transform.position;
        newZoom = cameraTransform.localPosition;
    }

    void Update()
    {
        HandlerMovementEdge();
        HandleMovement();
    }

    void HandleMovement()
    {
        switch(Input.mouseScrollDelta.y)
        {
            case 1:
                if(newZoom.y > minZoom)
                    newZoom += zoomAmount * Input.mouseScrollDelta.y;
                break;
            case -1:
                if(newZoom.y < maxZoom)
                    newZoom += zoomAmount * Input.mouseScrollDelta.y;
                break;
        }
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = fastSpeed;
        }
        else
        {
            speed = normalSpeed;
        }
        if (Input.GetKey(KeyCode.W))
        {
            newPoistion += (transform.forward * speed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            newPoistion -= (transform.forward * speed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            newPoistion += (transform.right * speed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            newPoistion -= (transform.right * speed);
        }
        if (Input.GetKey(KeyCode.Q))
        { 
            RotY += -speedRotationButton;
        }
        if (Input.GetKey(KeyCode.E))
        {
            RotY += speedRotationButton;
        }
        if (Input.GetMouseButtonDown(2)) { Cursor.lockState = CursorLockMode.Locked; }
        if (Input.GetMouseButtonUp(2)) { Cursor.lockState = CursorLockMode.None; }
        if (Input.GetMouseButton(2))
        {
            if (Input.GetAxis("Mouse Y") > 0f)
            {
                RotX += -speedRotationMouse * Input.GetAxis("Mouse Y");
            }
            if (Input.GetAxis("Mouse Y") < 0f)
            {
                RotX += speedRotationMouse * -Input.GetAxis("Mouse Y");
            }
            if(Input.GetAxis("Mouse X") > 0f)
            {
                RotY += -speedRotationMouse * -Input.GetAxis("Mouse X");
            }
            if(Input.GetAxis("Mouse X") < 0f)
            {
                RotY += -speedRotationMouse * -Input.GetAxis("Mouse X");
            }
            
            RotX = Mathf.Clamp(RotX, minAngle, maxAngle);
        }
        
        var rotx = Quaternion.Euler(RotX, RotY, 0);
        var roty = Quaternion.Euler(0, RotY, 0);

        Rotate.rotation = Quaternion.Lerp(Rotate.rotation, rotx, Time.deltaTime * movementTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, roty, Time.deltaTime * movementTime);

        if (newPoistion.x < borderCamera.Size.x / 2 && newPoistion.x > -borderCamera.Size.x / 2 && newPoistion.z < borderCamera.Size.z / 2 && newPoistion.z > -borderCamera.Size.z / 2)
        {
            transform.position = Vector3.Lerp(transform.position, newPoistion, Time.deltaTime * movementTime);
        }
        else
        {
            newPoistion = transform.position;
        }
    }

    void HandlerMovementEdge()
    {
        if(useEdge)
        {
            if (Input.mousePosition.x > Screen.width - edgeSize)
            {
                newPoistion += (transform.right * normalSpeed);
            }
            if (Input.mousePosition.x < edgeSize)
            {
                newPoistion -= (transform.right * normalSpeed);
            }
            if (Input.mousePosition.y > Screen.height - edgeSize)
            {
                newPoistion += (transform.forward * normalSpeed);
            }
            if (Input.mousePosition.y < edgeSize)
            {
                newPoistion -= (transform.forward * normalSpeed);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(borderCamera.Center, borderCamera.Size);
    }

    [System.Serializable]
    protected class BorderCamera
    {
        public Vector3 Center;
        public Vector3 Size;
    }
}
