using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace AgarioClone
{
	public class FoodSpawnSystem : IEcsInitSystem
	{
		private EcsCustomInject<GameConfig> _gameConfig;
		
		public void Init(IEcsSystems systems)
		{
			EcsWorld world = systems.GetWorld();
			
			for (int i = 0; i < _gameConfig.Value.FoodCount; i++)
			{
				float x = Random.Range(_gameConfig.Value.WorldWidth * -0.5f, _gameConfig.Value.WorldWidth * 0.5f);
				float y = Random.Range(_gameConfig.Value.WorldHeight * -0.5f, _gameConfig.Value.WorldHeight * 0.5f);
				Vector3 pos = new(x, y);
				
				var food = Object.Instantiate(_gameConfig.Value.FoodPrefab, pos, Quaternion.identity);

				int foodEntity = world.NewEntity();
				world.GetPool<FoodComponent>().Add(foodEntity);
				world.GetPool<TransformComponent>().Add(foodEntity).Transform = food.transform;
			}
		}
	}
}