using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] Transform cameraFollowTransform;
    [SerializeField] float RotateCameraSpeed = 20.0f;
    [SerializeField] float MovementCameraSpeed = 20.0f;
    [SerializeField] [Range(0,1)] float MovementDamping = 0.5f;
    [SerializeField] [Range(0,1)] float RotateDamping = 0.5f;
    private Camera _mainCamera;

    void Start()
    {
        _mainCamera = Camera.main;
    }

    //WIP FIX DAMPING
    public void UpdateCamera(Vector3 playerPosition,Vector2 playerMoveInput, bool lockCamera)
    {
        transform.position = playerPosition;
        if(!lockCamera)
        {
            transform.Rotate(Vector3.up , RotateCameraSpeed * Time.deltaTime * playerMoveInput.x);
        }

        //Damping
        _mainCamera.transform.position = Vector3.Lerp(_mainCamera.transform.position, cameraFollowTransform.position, (1.01f-MovementDamping)*(MovementCameraSpeed * Time.deltaTime));
        _mainCamera.transform.rotation = Quaternion.Lerp(_mainCamera.transform.rotation, cameraFollowTransform.rotation, (1.01f-RotateDamping)*(RotateCameraSpeed * Time.deltaTime));

    }
}
