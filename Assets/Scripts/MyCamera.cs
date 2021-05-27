using UnityEngine;

public class MyCamera : MonoBehaviour
{
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }

    public void SetCamera(Vector2 size)
    {
        SetCameraPosition(size);
        SetCameraOrthographicSize(size);
    }

    private void SetCameraPosition(Vector2 size)
    {
        float x = size.x / 2;
        float y = size.y / 2;
        _camera.transform.position = new Vector2(x, -y);
    }

    private void SetCameraOrthographicSize(Vector2 size)
    {
        _camera.orthographicSize = size.y / 2;
    }
}
