using UnityEngine;

public class MyCamera : MonoBehaviour
{
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }
    public void ResetCamera()
    {
        Vector2 cameraPosition = Vector2.zero;
        cameraPosition.x = CA.arraySize / 2 * CA.pixelDistance;
        cameraPosition.y = CA.maxGenerations / 2 * CA.pixelDistance - CA.pixelDistance;
        _camera.transform.position = new Vector2(cameraPosition.x, -cameraPosition.y);
        _camera.orthographicSize = CA.maxGenerations * CA.pixelDistance * 0.5f; //fits camera vertically
    }
}
