using System;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AgarioClone
{
	public class PlayerAISystem : IEcsRunSystem
	{
		private EcsCustomInject<FoodGrid> _grid;
		
		private EcsFilterInject<Inc<BotComponent>, Exc<ControlledByPlayerComponent>> _botFilter;
		private EcsFilterInject<Inc<PlayerComponent>> _playerFilter;

		private EcsPoolInject<PlayerComponent> _playerPool;
		private EcsPoolInject<BotComponent> _botPool;
		private EcsPoolInject<PlayerMoveComponent> _movePool;
		private EcsPoolInject<TransformComponent> _transformPool;

		private float _huntDistance = 10f;
		private float _avoidDistance = 3f;
		
		public void Run(IEcsSystems systems)
		{
			// bad performance
			foreach (var bot in _botFilter.Value)
			{
				ref var playerBotComponent = ref _playerPool.Value.Get(bot);
				ref var botComponent = ref _botPool.Value.Get(bot);
				var botTransform = _transformPool.Value.Get(bot).Transform;
				ref var moveComponent = ref _movePool.Value.Get(bot);
				
				foreach (var player in _playerFilter.Value)
				{
					if (bot == player) continue;
					
					ref var playerComponent = ref _playerPool.Value.Get(player);
					var playerTransform = _transformPool.Value.Get(player).Transform;
					
					if (playerBotComponent.IsDead || playerComponent.IsDead) continue;
					
					float distance = Vector3.Distance(botTransform.position, playerTransform.position);
				
					if (playerComponent.Score > playerBotComponent.Score && distance - playerTransform.localScale.x < _avoidDistance)
					{
						Vector3 direction = botTransform.position - playerTransform.position;
						moveComponent.MoveDirection = direction.normalized;
						botComponent.Avoid = true;
						botComponent.Hunt = false;
					}
					else if (playerBotComponent.Score > playerComponent.Score && distance < _huntDistance + botTransform.localScale.x)
					{
						Vector3 direction = playerTransform.position - botTransform.position;
						moveComponent.MoveDirection = direction.normalized;
						botComponent.Avoid = false;
						botComponent.Hunt = true;
					}
				}
				
				if (botComponent.Avoid || botComponent.Hunt) continue;
				
				if (botComponent.FoodTarget != Vector3.zero)
				{
					Vector3 direction = botComponent.FoodTarget - botTransform.position;
					moveComponent.MoveDirection = direction.normalized;

					if (Vector3.Distance(botTransform.position, botComponent.FoodTarget) < botTransform.localScale.x * 0.5f)
					{
						botComponent.FoodTarget = Vector3.zero;
					}
						
					continue;
				}
					
				var chunks = _grid.Value.GetChunksWherePlayerIs(botTransform.position, botTransform.localScale.x);
				if (chunks.Length > 0)
				{
					float minDistance = float.MaxValue;
					int randomChunk = Random.Range(0, chunks.Length);
					for (int i = 0; i < chunks[randomChunk].FoodPositions.Count; i++)
					{
						float foodDistance = Vector3.Distance(botTransform.position, chunks[randomChunk].FoodPositions[i].GetPosition());
						if (foodDistance < minDistance)
						{
							minDistance = foodDistance;
							botComponent.FoodTarget = chunks[0].FoodPositions[i].GetPosition();
						}
					}
				}
			}
		}
	}
}