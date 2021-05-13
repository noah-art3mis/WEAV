using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private float pixelDistance = 0.01f;
    [SerializeField] private Sprite image;
    
    public static int maxY = 100;
    public static int maxX = 100;
    public static Sprite[][] spriteArray = new Sprite[][];

    public void Create()
    {
        Vector2 position;
        for (int j = 0; j < maxY; j++)
        {
            for (int i = 0; i < maxX; i++)
            {
                position.x = i * pixelDistance;
                position.y = -j * pixelDistance;
                Sprite cell = Instantiate(image, position, Quaternion.identity, this.transform);
                spriteArray[j][i] = cell;
            }
        }
    }
}