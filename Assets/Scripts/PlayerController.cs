using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Mouvement Settings")]
    public float mouseSensitivity = 10f;
    public float mouvementSpeed = 10f;

    [Header("RigidBody")]
    [SerializeField]
    private Rigidbody playerBody;

    [Header("Player Camera")]
    [SerializeField]
    private GameObject playerCamera;

    // Mouvement Vector
    private Vector3 inputVector;

    // Look Variable
    private float xRotation;

    // Start is called before the first frame update
    void Start()
    {
        playerBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float xMouvementInput = Input.GetAxis("Horizontal") * mouvementSpeed;
        float zMouvementInput = Input.GetAxis("Vertical") * mouvementSpeed;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        inputVector = new Vector3(xMouvementInput, playerBody.velocity.y, zMouvementInput);

        //transform.LookAt(transform.position + );
    }

    private void FixedUpdate()
    {
        playerBody.velocity = inputVector;

        
    }
}
