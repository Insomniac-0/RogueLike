using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Burst;

namespace Roguelike
{
	public struct CharacterMoveDirection : IComponentData
	{
		public float2 value;
	}

	public struct CharacterMoveSpeed : IComponentData
	{
		public float value;
	}

	public class CharacterAuthoring : MonoBehaviour
	{
		public float MoveSpeed;

		private class Baker : Baker<CharacterAuthoring>
		{
			public override void Bake(CharacterAuthoring authoring)
			{
				var entity = GetEntity(TransformUsageFlags.Dynamic);
				AddComponent<CharacterMoveDirection>(entity);
				AddComponent(entity, new CharacterMoveSpeed { value = authoring.MoveSpeed });
			}
		}
	}

	public partial struct CharacterMoveSystem : ISystem
	{
		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			foreach (var (velocity, direction, speed) in SystemAPI.Query<RefRW<PhysicsVelocity>, CharacterMoveDirection, CharacterMoveSpeed>())
			{
				var movement = direction.value * speed.value;
				velocity.ValueRW.Linear = new float3(movement, 0f);
			}
		}
	}
}
