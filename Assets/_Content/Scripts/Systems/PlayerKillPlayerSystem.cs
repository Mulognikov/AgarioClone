using System.Collections.Generic;
using DG.Tweening;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AgarioClone
{
	public class PlayerKillPlayerSystem : IEcsRunSystem, IEcsPostRunSystem
	{
		private EcsCustomInject<GameConfig> _gameConfig;
		private EcsWorldInject _world;
		private EcsFilterInject<Inc<PlayerComponent>> _playerFilter;
		private EcsPoolInject<PlayerComponent> _playerPool;
		private EcsPoolInject<TransformComponent> _transformPool;

		private List<int> _deadInThisUpdateList = new();
		
		public void Run(IEcsSystems systems)
		{
			foreach (var player1 in _playerFilter.Value)
			foreach (var player2 in _playerFilter.Value)
			{
				if (player1 == player2) continue;

				ref var player1Component = ref _playerPool.Value.Get(player1);
				ref var player2Component = ref _playerPool.Value.Get(player2);
				
				if (player1Component.IsDead) continue;
				if (player2Component.IsDead) continue;
				
				var player1Transform = _transformPool.Value.Get(player1).Transform;
				var player2Transform = _transformPool.Value.Get(player2).Transform;

				float distance = player1Transform.localScale.x > player2Transform.localScale.x
					? player1Transform.localScale.x
					: player2Transform.localScale.x;
				
				if (Vector3.Distance(player1Transform.position, player2Transform.position) < distance * 0.5f)
				{
					if (player1Component.Score > player2Component.Score)
					{
						KillPlayer(player1, player2);
					}
					
					if (player1Component.Score < player2Component.Score)
					{
						KillPlayer(player2, player1);
					}
				}

			}
		}

		private void KillPlayer(int killer, int victim)
		{
			var killerTransform = _transformPool.Value.Get(killer).Transform;
			var victimTransform = _transformPool.Value.Get(victim).Transform;
			ref var killerComponent = ref _playerPool.Value.Get(killer);
			ref var victimComponent = ref _playerPool.Value.Get(victim);
			
			killerComponent.Score += victimComponent.Score;
			victimComponent.IsDead = true;
			_deadInThisUpdateList.Add(victim);

			var tween = victimTransform.DOMove(killerTransform.position, 0.5f).OnKill(() =>
			{
				Object.Destroy(victimTransform.gameObject);
			});
			tween.OnUpdate(() =>
			{
				Vector3 target = (killerTransform.position - victimTransform.position).normalized * 3f;
				tween.ChangeEndValue(killerTransform.position + target, true);
				if (Vector3.Distance(victimTransform.position, killerTransform.position) <
				    (killerTransform.localScale.x - victimTransform.localScale.x) * 0.5f)
				{
					tween.Kill();
				}
			});
		}

		public void PostRun(IEcsSystems systems)
		{
			foreach (var player in _deadInThisUpdateList)
			{
				_world.Value.DelEntity(player);
			}
			
			_deadInThisUpdateList.Clear();
		}
	}
}