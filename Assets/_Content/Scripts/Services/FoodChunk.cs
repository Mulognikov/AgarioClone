using System.Collections.Generic;
using UnityEngine;

namespace AgarioClone
{
	public class FoodChunk
	{
		public List<Matrix4x4> FoodPositions = new();
		public Vector3 ChunkPosition => _chunkPosition;

		private Vector3 _chunkPosition;
		private GameConfig _gameConfig;
		private float _radius;

		public FoodChunk(Vector3 chunkPosition, GameConfig gameConfig)
		{
			_chunkPosition = chunkPosition;
			_gameConfig = gameConfig;
			
			_radius = Mathf.Sqrt(_gameConfig.ChunkSize * _gameConfig.ChunkSize * 2) * 0.5f;
			
			SpawnFood();
		}

		private void SpawnFood()
		{
			int foodInChunk = _gameConfig.FoodCount / (_gameConfig.WorldWidth * _gameConfig.WorldHeight);

			for (int i = 0; i < foodInChunk; i++)
			{
				CreateFood();
			}
		}

		public void CreateFood()
		{
			float x = Random.Range(_chunkPosition.x - _gameConfig.ChunkSize * 0.5f, _chunkPosition.x + _gameConfig.ChunkSize * 0.5f);
			float y = Random.Range(_chunkPosition.y - _gameConfig.ChunkSize * 0.5f, _chunkPosition.y + _gameConfig.ChunkSize * 0.5f);
			Vector3 pos = new(x, y);
				
			FoodPositions.Add(Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one));
			//FoodPositions.Add(pos);
		}
		
		public bool IsPlayerInChunk(Vector3 playerPosition, float playerSize)
		{
			return Vector3.Distance(_chunkPosition, playerPosition) < playerSize + _radius;
		}
	}
}