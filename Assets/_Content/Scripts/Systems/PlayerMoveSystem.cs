using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace AgarioClone
{
	public class PlayerMoveSystem : IEcsRunSystem, IEcsInitSystem
	{
		private EcsCustomInject<GameConfig> _gameConfig;
		private EcsPoolInject<PlayerMoveComponent> _movePool;
		private EcsPoolInject<TransformComponent> _transformPool;
		private EcsPoolInject<PlayerComponent> _playerPool;
		private EcsFilterInject<Inc<PlayerComponent>> _playerFilter;

		private float maxX;
		private float maxY;
		
		public void Init(IEcsSystems systems)
		{
			maxX = _gameConfig.Value.WorldWidth * _gameConfig.Value.ChunkSize;
			maxY = _gameConfig.Value.WorldHeight * _gameConfig.Value.ChunkSize;
		}
		
		public void Run(IEcsSystems systems)
		{
			foreach (var player in _playerFilter.Value)
			{
				var transform = _transformPool.Value.Get(player).Transform;
				var moveDirection = _movePool.Value.Get(player).MoveDirection;
				var score = _playerPool.Value.Get(player).Score;

				Move(transform, moveDirection, score);
			}
		}

		private void Move(Transform transform, Vector3 moveDirection, int score)
		{
			float speed = (float)_gameConfig.Value.PlayerStartScore / score * _gameConfig.Value.PlayerSpeed;
			speed = Mathf.Log10(score) * -0.5f + _gameConfig.Value.PlayerSpeed;
			Vector3 newPosition = transform.position + moveDirection * speed * Time.deltaTime;

			float x = newPosition.x;
			float y = newPosition.y;
			float halfScale = transform.localScale.x * 0.5f;
			
			if (newPosition.x <= halfScale) x = halfScale;
			if (newPosition.y <= halfScale) y = halfScale;
			if (newPosition.x >= maxX - halfScale) x = maxX - halfScale;
			if (newPosition.y >= maxY - halfScale) y = maxY - halfScale;

			transform.position = new Vector3(x, y);
		}

	}
}