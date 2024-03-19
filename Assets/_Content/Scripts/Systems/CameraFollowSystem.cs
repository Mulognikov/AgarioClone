using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace AgarioClone
{
	public class CameraFollowSystem : IEcsRunSystem
	{
		private EcsCustomInject<SceneData> _sceneData;
		private EcsFilterInject<Inc<ControlledByPlayerComponent>> _playerFilter;
		private EcsPoolInject<TransformComponent> _transformPool;

		public void Run(IEcsSystems systems)
		{
			foreach (var player in _playerFilter.Value)
			{
				Transform playerTransform = _transformPool.Value.Get(player).Transform;
				_sceneData.Value.MainCamera.transform.position = playerTransform.position;
				_sceneData.Value.MainCamera.transform.position -= Vector3.forward;
				_sceneData.Value.MainCamera.orthographicSize = playerTransform.localScale.x + 5;
			}
		}
	}
}