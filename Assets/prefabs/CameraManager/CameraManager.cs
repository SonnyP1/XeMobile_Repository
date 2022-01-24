using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] Transform cameraFollowTransform;
    [SerializeField] float RotateSpeed = 20.0f;
    private Camera _mainCamera;

    void Start()
    {
        _mainCamera = Camera.main;
    }

    public void UpdateCamera(Vector3 playerPosition,Vector2 playerMoveInput, bool lockCamera)
    {
        transform.position = playerPosition;
        if(!lockCamera)
        {
            transform.Rotate(Vector3.up , RotateSpeed * Time.deltaTime * playerMoveInput.x);
        }
        //makes camera follow
        _mainCamera.transform.position = cameraFollowTransform.position;
        _mainCamera.transform.rotation = cameraFollowTransform.rotation;
    }
}
