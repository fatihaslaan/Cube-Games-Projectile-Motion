using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    float HorizontalAxis;
    float VerticalAxis;
    void Update()
    {
        HorizontalAxis = Input.GetAxis("Horizontal");
        VerticalAxis = Input.GetAxis("Vertical");
        if (Input.GetMouseButton(2))
        {
            Vector3 Rotation = new Vector3(VerticalAxis, HorizontalAxis, 0);
            transform.Rotate(Rotation);
        }
        else
            transform.Translate(new Vector3(HorizontalAxis, 0, VerticalAxis));  //or transform.Translate(new Vector3(HorizontalAxis, VerticalAxis, 0)); for only going up and down instead of going forward and backward
    }
}