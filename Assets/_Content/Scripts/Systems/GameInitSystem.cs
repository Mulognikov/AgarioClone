using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace AgarioClone
{
	public class GameInitSystem : IEcsInitSystem
	{
		private EcsCustomInject<GameConfig> _gameConfig;
		
		public void Init(IEcsSystems systems)
		{
			EcsWorld world = systems.GetWorld();

			for (int i = 0; i < _gameConfig.Value.PlayersCount; i++)
			{
				//float x = _gameConfig.Value.WorldWidth * _gameConfig.Value.ChunkSize * 0.5f;
				//float y = _gameConfig.Value.WorldHeight * _gameConfig.Value.ChunkSize * 0.5f;

				float x = Random.Range(0, _gameConfig.Value.WorldWidth * _gameConfig.Value.ChunkSize);
				float y = Random.Range(0, _gameConfig.Value.WorldHeight * _gameConfig.Value.ChunkSize);
				
				var player = Object.Instantiate(_gameConfig.Value.PlayerPrefab, new Vector3(x, y), Quaternion.identity);
				
				
				int playerEntity = world.NewEntity();

				ref var playerComponent = ref world.GetPool<PlayerComponent>().Add(playerEntity);
				playerComponent.Score = _gameConfig.Value.PlayerStartScore;
				playerComponent.PlayerView = player;
				
				world.GetPool<PlayerMoveComponent>().Add(playerEntity);
				world.GetPool<TransformComponent>().Add(playerEntity).Transform = player.transform;

				if (i == 0)
				{
					world.GetPool<ControlledByPlayerComponent>().Add(playerEntity);
					player.SetColor(_gameConfig.Value.PlayerColors[RuntimeData.PlayerColorIndex]);
				}
				else
				{
					world.GetPool<BotComponent>().Add(playerEntity);
					Color playerColor = _gameConfig.Value.PlayerColors[Random.Range(0, _gameConfig.Value.PlayerColors.Length)];
					player.SetColor(playerColor);
				}
			}
			
			// for (int i = 0; i < _gameConfig.Value.FoodCount; i++)
			// {
			// 	float x = Random.Range(_gameConfig.Value.WorldWidth * -0.5f, _gameConfig.Value.WorldWidth * 0.5f);
			// 	float y = Random.Range(_gameConfig.Value.WorldHeight * -0.5f, _gameConfig.Value.WorldHeight * 0.5f);
			// 	Vector3 pos = new(x, y);
			// 	
			// 	var food = Object.Instantiate(_gameConfig.Value.FoodPrefab, pos, Quaternion.identity);
			//
			// 	int foodEntity = world.NewEntity();
			// 	world.GetPool<FoodComponent>().Add(foodEntity);
			// 	world.GetPool<TransformComponent>().Add(foodEntity).Transform = food.transform;
			// }
		}
	}
}