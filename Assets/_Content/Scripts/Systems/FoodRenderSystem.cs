using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace AgarioClone
{
	public class FoodRenderSystem : IEcsInitSystem, IEcsRunSystem
	{
		private EcsCustomInject<FoodGrid> _grid;
		private EcsCustomInject<GameConfig> _gameConfig;
		private EcsCustomInject<SceneData> _sceneData;

		private Mesh _mesh;
		
		public void Init(IEcsSystems systems)
		{
			_mesh = QuadCreator.CreateQuad(_gameConfig.Value.FoodSize);
		}
		
		public void Run(IEcsSystems systems)
		{
			var chunks = _grid.Value.GetChunksInCamera(_sceneData.Value.MainCamera);
			foreach (var chunk in chunks)
			{
				 // Matrix4x4[] array = new Matrix4x4[chunk.FoodPositions.Count];
				 //
				 // for (int i = 0; i < chunk.FoodPositions.Count; i++)
				 // {
				 // 	array[i] = Matrix4x4.TRS(chunk.FoodPositions[i], Quaternion.identity, Vector3.one);
				 // }
				
				//Graphics.DrawMeshInstanced(_mesh, 0, _gameConfig.Value.FoodMaterial, array);
				Graphics.DrawMeshInstanced(_mesh, 0, _gameConfig.Value.FoodMaterial, chunk.FoodPositions);
			}
		}
	}
}