using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace AgarioClone
{
	public class MouseInputSystem : IEcsRunSystem
	{
		private EcsCustomInject<GameConfig> _gameConfig;
		private EcsFilterInject<Inc<ControlledByPlayerComponent>> _playerFilter;
		private EcsPoolInject<PlayerMoveComponent> _movePool;
		
		public void Run(IEcsSystems systems)
		{
			// Player can resize window
			Vector3 screenCenter = new (Screen.width * 0.5f, Screen.height * 0.5f);
			float shortScreenSide = Screen.height < Screen.width ? Screen.height : Screen.width;
			float maxSize = shortScreenSide * _gameConfig.Value.MaxSpeedMouseRadiusNormalized * 0.5f;
			
			foreach (var player in _playerFilter.Value)
			{
				ref var moveComponent = ref _movePool.Value.Get(player);
				Vector3 direction = (Input.mousePosition - screenCenter);
				Vector3 normalized = direction.normalized * Mathf.Lerp(0f, 1f, direction.magnitude / maxSize);
				moveComponent.MoveDirection = normalized;
			}
		}
	}
}