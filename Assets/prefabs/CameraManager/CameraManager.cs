using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] Transform cameraFollowTransform;
    [SerializeField] float RotateSpeed = 20.0f;
    [SerializeField] [Range(0,1)] float MovementDamping = 0.5f;
    [SerializeField] [Range(0,1)] float RotateDamping = 0.5f;
    private Camera _mainCamera;
    private float _time = 0;

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
            transform.Rotate(Vector3.up , RotateSpeed * Time.deltaTime * playerMoveInput.x);
        }
        //makes camera follow
        //use lerping to add damping

        Vector3 previousPos = _mainCamera.transform.position;
        Quaternion previousRot = _mainCamera.transform.rotation;


         _mainCamera.transform.position = Vector3.Lerp(previousPos, cameraFollowTransform.position, MovementDamping*Time.deltaTime);
         _mainCamera.transform.rotation = Quaternion.Lerp(previousRot, cameraFollowTransform.rotation, RotateDamping* Time.deltaTime);


        //_mainCamera.transform.position = cameraFollowTransform.position;
        //_mainCamera.transform.rotation = cameraFollowTransform.rotation;
    }
}
