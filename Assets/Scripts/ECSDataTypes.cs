using Unity.Mathematics;

public struct TransformData
{
    public quaternion rotation;
    public float3 position;
    public float3 scale;
    public TransformData(Transform t) => 
}

struct ProjectileData
{
    public TransformData transform;

    public float3 direction;

    public int ID;
    public int HP;

    public float dmg;
    public float speed;
    public float lifetime;

    public bool active;
}

// TransFormData ConvertTransform(Transform t)
// {
//     TransFormData data = new TransformData
//     {
//         position = t.position;
//         rotation = t.rotation;
//         scale = t.scale;
//     }

//     return data;

TransFormData ConvertTransform(Transform t)
{
    return new TransformData
    {
        position = t.position;
        rotation = t.rotation;
        scale = t.scale;
    }
}