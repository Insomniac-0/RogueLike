using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Roguelike
{
	public struct PlayerTag : IComponentData { }

	public struct CameraTarget : IComponentData
	{
		public UnityObjectRef<Transform> CameraTransform;
	}

	public struct InitializeCameraTargetTag : IComponentData { }

	public class PlayerAuthoring : MonoBehaviour
	{
		private class Baker : Baker<PlayerAuthoring>
		{
			public override void Bake(PlayerAuthoring authoring)
			{
				var entity = GetEntity(TransformUsageFlags.Dynamic);
				AddComponent<PlayerTag>(entity);
				AddComponent<InitializeCameraTargetTag>(entity);
				AddComponent<CameraTarget>(entity);
			}
		}
	}
	[UpdateInGroup(typeof(InitializationSystemGroup))]
	public partial struct CameraInitializationSystem : ISystem
	{
		public void OnCreate(ref SystemState state)
		{
			state.RequireForUpdate<InitializeCameraTargetTag>();
		}

		public void OnUpdate(ref SystemState state)
		{
			if (CameraTargetSingelton.Instance == null) return;
			var camera_target_transform = CameraTargetSingelton.Instance.transform;

			foreach (var camera_target in SystemAPI.Query<RefRW<CameraTarget>>().WithAll<InitializeCameraTargetTag, PlayerTag>())
			{
				camera_target.ValueRW.CameraTransform = camera_target_transform;
			}
		}
	}

	public partial class PlayerInputSystem : SystemBase
	{
		private UserInput _input;
		protected override void OnCreate()
		{
			_input = new UserInput();
			_input.Enable();
		}
		protected override void OnUpdate()
		{
			var current_input = (float2)_input.Player.Move.ReadValue<Vector2>();
			foreach (var direction in SystemAPI.Query<RefRW<CharacterMoveDirection>>().WithAll<PlayerTag>())
			{
				direction.ValueRW.value = current_input;
			}
		}
	}
}

