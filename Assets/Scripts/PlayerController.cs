using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speedMultiplayer = 5f;
    [SerializeField] float movementSpeed;

    public Rigidbody rb;
    private Vector3 movementInput;
    private Vector3 movementVelocity;

    public Camera mainCamera;

    public static Vector3 lookAt;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        movementInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        movementVelocity = movementInput * movementSpeed;


        Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if(groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            Debug.DrawLine(cameraRay.origin, pointToLook, Color.blue);

            transform.LookAt(lookAt = new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }

    }

    void FixedUpdate()
    {
        rb.velocity = movementVelocity;

        float movement = Math.Abs(movementVelocity.x) + Math.Abs(movementVelocity.z);

        if(movement != 0)
        {
            GetComponent<Animator>().SetTrigger("move");
        }
        else
        {
            GetComponent<Animator>().SetTrigger("idle");
        }
    }
}
