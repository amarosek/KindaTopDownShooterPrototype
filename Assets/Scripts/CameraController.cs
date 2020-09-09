using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] GameObject player;
    [SerializeField] Camera mainCamera;
    [SerializeField] float smoothTime = 0.3f;

    private Vector3 velocity = Vector3.zero;

    Vector3 offset = new Vector3(0, 20, -10);

    void Start()
    {
        SetAngle();
    }
    void Update()
    {
        FollowPlayer();
    }

    public void FollowPlayer()
    {
        mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, player.transform.position + offset, ref velocity, smoothTime);
    }

    public void SetAngle()
    {
        mainCamera.transform.eulerAngles = new Vector3(mainCamera.transform.eulerAngles.x + 60,
                                                       mainCamera.transform.eulerAngles.y,
                                                       mainCamera.transform.eulerAngles.z);
    }
}
