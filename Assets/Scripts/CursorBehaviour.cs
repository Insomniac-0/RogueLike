using System.Linq.Expressions;
using Unity.Mathematics;
using UnityEngine;

public class CursorBehaviour : MonoBehaviour
{

    void Update()
    {
        transform.position = new float3(InitResources.GetInputReader.GetMousePositionWS().xy, 0f);
    }

}
