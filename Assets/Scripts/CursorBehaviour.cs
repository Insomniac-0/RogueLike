using System.Linq.Expressions;
using Unity.Mathematics;
using UnityEngine;

public class CursorBehaviour : MonoBehaviour
{
    [SerializeField] InputReader _input;

    void Update()
    {
        transform.position = new float3(_input.GetMousePositionWS().xy, 0f);
    }

}
