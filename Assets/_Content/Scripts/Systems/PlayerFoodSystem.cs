using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace AgarioClone
{
	public class PlayerFoodSystem : IEcsRunSystem
	{
		private EcsCustomInject<GameConfig> _gameConfig;
		private EcsCustomInject<FoodGrid> _grid;
		
		private EcsFilterInject<Inc<PlayerComponent>> _playerFilter;
		private EcsPoolInject<TransformComponent> _transformPool;
		private EcsPoolInject<PlayerComponent> _playerPool;
		
		public void Run(IEcsSystems systems)
		{
			foreach (var player in _playerFilter.Value)
			{
				Transform playerTransform = _transformPool.Value.Get(player).Transform;
				ref PlayerComponent playerComponent = ref _playerPool.Value.Get(player);

				FoodChunk[] chunks = _grid.Value.GetChunksWherePlayerIs(playerTransform.position, playerTransform.localScale.x);
				
				foreach (var chunk in chunks)
				for (var i = 0; i < chunk.FoodPositions.Count; i++)
				{
					float distance = playerTransform.localScale.x * 0.5f - _gameConfig.Value.FoodSize * 0.25f;
					//if (Vector3.Distance(chunk.FoodPositions[i], playerTransform.position) < distance)
					if (Vector3.Distance(chunk.FoodPositions[i].GetPosition(), playerTransform.position) < distance)
					{
						playerComponent.Score += _gameConfig.Value.FoodScore;
						_grid.Value.RemoveFoodAndCreateRandom(chunk, i);
					}
				}
			}	
		}
	}
}