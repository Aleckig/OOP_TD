using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraPan : MonoBehaviour
{
    [SerializeField] private float panSpeed = 10f;
    private CinemachineInputProvider inputProvider;
    private CinemachineVirtualCamera virtualCamera;
    private Transform cameraTransform;

    private void Awake()
    {
        inputProvider = GetComponent<CinemachineInputProvider>();
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        cameraTransform = virtualCamera.VirtualCameraGameObject.transform;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = inputProvider.GetAxisValue(0);
        float y = inputProvider.GetAxisValue(1);
        float z = inputProvider.GetAxisValue(2);
        if (x != 0 || y != 0)
        {
            PanScreen(x, y, z);
        }
    }

    public Vector3 PanDirection(float x, float y, float z)
    {
        Vector3 direction = Vector3.zero;
        if (x >= Screen.width * 0.9f)
        {
            direction.z -= 1;
        }
        else if (x <= Screen.width * 0.1f)
        {
            direction.z += 1;
        }
        return direction;
    }

    public void PanScreen(float x, float y, float z)
    {
        Vector3 direction = PanDirection(x, y, z);
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, cameraTransform.position + direction * panSpeed, Time.deltaTime);
    }
}
