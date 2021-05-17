using UnityEngine;

public class MyCamera : MonoBehaviour
{
    private Camera _camera;
    public float resolution = 1;
    public float sizeMod = 0;

    CA ca;

    private void Start()
    {
        _camera = Camera.main;
        ca = GetComponent<CA>();
    }

    public void ResetCamera()
    {
        float x = ca.arraySize / 2 - 0.5f;
        float y = ca.maxGenerations / 2 - 0.5f;
        _camera.transform.position = new Vector2(x, -y);
        _camera.orthographicSize = ca.maxGenerations * 0.5f + sizeMod; //fits camera vertically
    }

    private void Update()
    {
        ResetCamera();
    }
}
