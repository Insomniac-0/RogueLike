using Unity.Mathematics;
using UnityEngine;

public class CursorBehaviour : MonoBehaviour
{
    [SerializeField] InputBehaviour input;

    void Update()
    {
        transform.position = new float3(GameData.MousePosition.xy, 0);
    }
}
