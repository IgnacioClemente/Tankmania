using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Vector3 cameraOffSet;
    [SerializeField] Transform player;
    [SerializeField] private Transform aimPivot;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.position + cameraOffSet;
        transform.rotation = aimPivot.rotation;
    }
}
