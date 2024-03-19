using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace AgarioClone
{
	public class PlayerScaleSystem : IEcsRunSystem, IEcsInitSystem
	{
		private EcsCustomInject<GameConfig> _gameConfig;
		
		private EcsFilterInject<Inc<PlayerComponent>> _playerFilter;
		
		private EcsPoolInject<TransformComponent> _transformPool;
		private EcsPoolInject<PlayerComponent> _playerPool;

		private float _scaleDiameterRatio;
		
		public void Init(IEcsSystems systems)
		{
			float diameter = Mathf.Sqrt(_gameConfig.Value.PlayerStartScore / Mathf.PI) * 2;
			_scaleDiameterRatio = _gameConfig.Value.PlayerStartSize / diameter;
		}
		
		public void Run(IEcsSystems systems)
		{
			foreach (var player in _playerFilter.Value)
			{
				var playerTransform = _transformPool.Value.Get(player).Transform;
				ref var playerComponent = ref _playerPool.Value.Get(player);
				
				var scale = Vector3.one * Mathf.Sqrt(playerComponent.Score / Mathf.PI) * 2f * _scaleDiameterRatio;
				playerTransform.localScale = Vector3.Lerp(playerTransform.localScale, scale, Time.deltaTime * 5f);
				
				playerComponent.PlayerView.SetLayer(playerComponent.Score);
			}
		}

	}
}