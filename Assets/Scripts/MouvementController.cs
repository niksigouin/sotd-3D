using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouvementController : MonoBehaviour
{
    [SerializeField]
    float speed = 5f;

    [SerializeField]
    private float lookSensitivity = 3f;

    [SerializeField]
    GameObject playerCam;

    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private float cameraRotation = 0f;
    private float currentCameraRotation = 0f;

    private Rigidbody rb;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    private void Update()
    {
        float _xMovement = Input.GetAxis("Horizontal");
        float _zMovement = Input.GetAxis("Vertical");

        Vector3 _mouvementHorizontal = transform.right * _xMovement;
        Vector3 _mouvementVertical = transform.forward * _zMovement;

        // Final Mouvement Vel
        Vector3 _movementVelocity = (_mouvementHorizontal + _mouvementVertical).normalized * speed;

        //Apply movement
        Move(_movementVelocity);

        //Calculate rotation
        float _yRotation = Input.GetAxis("Mouse X");
        Vector3 _rotationVector = new Vector3(0f, _yRotation, 0f) * lookSensitivity;

        //Apply rotation
        Rotate(_rotationVector);

        //Calculate Camera rotation as 
        float _cameraRotation = Input.GetAxis("Mouse Y") * lookSensitivity;

        // Apply rotation
        RotateCamera(_cameraRotation);
    }

    private void FixedUpdate()
    {
        if (velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position+velocity*Time.fixedDeltaTime);
        }

        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));

        if (playerCam != null)
        {
            currentCameraRotation -= cameraRotation;
            currentCameraRotation = Mathf.Clamp(currentCameraRotation, -85f, 85f);
            playerCam.transform.localEulerAngles = new Vector3(currentCameraRotation, 0f, 0f);
        }
    }

    void Move(Vector3 movVel)
    {
        velocity = movVel;
    }

    void Rotate(Vector3 rotVec)
    {
        rotation = rotVec;
    }

    void RotateCamera(float rot)
    {
        cameraRotation = rot;
    }
}
