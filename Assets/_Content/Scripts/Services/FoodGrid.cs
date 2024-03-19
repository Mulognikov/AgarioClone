using System.Collections.Generic;
using UnityEngine;

namespace AgarioClone
{
	public class FoodGrid
	{
		private List<FoodChunk> _chunks = new ();
		private GameConfig _gameConfig;
		
		public FoodGrid(GameConfig gameConfig)
		{
			_gameConfig = gameConfig;
			
			float halfSize = _gameConfig.ChunkSize / 2f;
			
			for (int x = 0; x < _gameConfig.WorldWidth; x++)
			for (int y = 0; y < _gameConfig.WorldHeight; y++)
			{
				float xPos = x * _gameConfig.ChunkSize + halfSize;
				float yPos = y * _gameConfig.ChunkSize + halfSize;
				
				Vector3 chunkPosition = new(xPos, yPos);
				_chunks.Add(new FoodChunk(chunkPosition, _gameConfig));
			}
		}

		public FoodChunk[] GetChunksWherePlayerIs(Vector3 playerPosition, float playerSize)
		{
			List<FoodChunk> playerChunks = new();

			float halfSize = playerSize * 0.5f;

			int startX = Mathf.FloorToInt((playerPosition.x - halfSize) / _gameConfig.ChunkSize);
			int startY = Mathf.FloorToInt((playerPosition.y - halfSize) /_gameConfig.ChunkSize);
			
			int endX = Mathf.FloorToInt((playerPosition.x + halfSize) / _gameConfig.ChunkSize);
			int endY = Mathf.FloorToInt((playerPosition.y + halfSize) / _gameConfig.ChunkSize);

			startX = Mathf.Clamp(startX, 0, _gameConfig.WorldWidth - 1);
			startY = Mathf.Clamp(startY, 0, _gameConfig.WorldHeight - 1);
			endX = Mathf.Clamp(endX, 0, _gameConfig.WorldWidth - 1);
			endY = Mathf.Clamp(endY, 0, _gameConfig.WorldHeight - 1);

			for (int i = startX; i <= endX; i++)
			{
				for (int j = startY; j <= endY; j++)
				{
					playerChunks.Add(_chunks[i * _gameConfig.WorldWidth + j]);
				}
			}
			
			return playerChunks.ToArray();
		}

		public FoodChunk[] GetChunksInCamera(Camera camera)
		{
			List<FoodChunk> cameraChunks = new();
			
			float ySize = camera.orthographicSize;
			float xSize = Screen.width / (float)Screen.height * ySize;
			
			int x1 = Mathf.FloorToInt((camera.transform.position.x - xSize) / _gameConfig.ChunkSize);
			int x2 = Mathf.FloorToInt((camera.transform.position.x + xSize) / _gameConfig.ChunkSize);
			
			int y1 = Mathf.FloorToInt((camera.transform.position.y - ySize) / _gameConfig.ChunkSize);
			int y2 = Mathf.FloorToInt((camera.transform.position.y + ySize) / _gameConfig.ChunkSize);
			
			x1 = Mathf.Clamp(x1, 0, _gameConfig.WorldWidth - 1);
			x2 = Mathf.Clamp(x2, 0, _gameConfig.WorldWidth - 1);
			
			y1 = Mathf.Clamp(y1, 0, _gameConfig.WorldHeight - 1);
			y2 = Mathf.Clamp(y2, 0, _gameConfig.WorldHeight - 1);
			
			for (int x = x1; x <= x2; x++)
			for (int y = y1; y <= y2; y++)
			{
				cameraChunks.Add(_chunks[x * _gameConfig.WorldWidth + y]);
			}
			
			return cameraChunks.ToArray();
		}

		public void RemoveFoodAndCreateRandom(FoodChunk chunk, int index)
		{
			chunk.FoodPositions.RemoveAt(index);
			_chunks[Random.Range(0, _chunks.Count)].CreateFood();
		}
	}
}