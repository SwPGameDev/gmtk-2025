using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject target;
    public float xOffset = 0;
    public float yOffset = 0;
    public float zOffset = 0;

    public float xAngle = -45;
    public float yAngle = 30;
    public float zAngle = 0;

    private void Start()
    {
        transform.SetPositionAndRotation(new Vector3(
            target.transform.position.x + xOffset,
            target.transform.position.y + yOffset,
            target.transform.position.z + zOffset
            ), Quaternion.Euler(xAngle, yAngle, zAngle));
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(
        target.transform.position.x + xOffset,
        target.transform.position.y + yOffset,
        target.transform.position.z + zOffset
        );
    }
}