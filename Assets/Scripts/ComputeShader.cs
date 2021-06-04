using UnityEngine;

public class ComputeShader : MonoBehaviour
{
    [SerializeField] private ComputeShader computeShader;
    ComputeBuffer positionsBuffer;
    private CA ca;

    private void OnEnable()
    {
        ca = GetComponent<CA>();
        positionsBuffer = new ComputeBuffer(ca.arraySize * ca.maxGenerations, 2 * 4);
        computeShader.setBuffer();
        computeShader.Dispatch();
    }

    private void OnDisable()
    {
        positionsBuffer.Release();
        positionsBuffer = null;
    }
}
