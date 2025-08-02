using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject target;
    public Vector3 offsetVector;

    public Vector3 angleOffset;

    public float cameraMoveSpeed = 10;

    private void Start()
    {
        transform.SetPositionAndRotation((target.transform.position + offsetVector), Quaternion.Euler(angleOffset));
    }

    private void LateUpdate()
    {
        Vector3 offsetTargetPos = target.transform.position + offsetVector;

        transform.position = Vector3.Lerp(transform.position, offsetTargetPos, cameraMoveSpeed * Time.deltaTime);


        //transform.position = offsetTargetPos;
    }
}