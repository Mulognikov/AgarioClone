using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace AgarioClone 
{
    public sealed class EcsStartup : MonoBehaviour
    {
        [SerializeField] private GameConfig _gameConfig;
        [SerializeField] private SceneData _sceneData;

        private FoodGrid _foodGrid;
        
        private EcsWorld _world;
        private IEcsSystems _systems;

        private void Start ()
        {
            _foodGrid = new FoodGrid(_gameConfig);
            
            _world = new EcsWorld ();
            _systems = new EcsSystems (_world);
            _systems
                .Add(new GameInitSystem())
                .Add(new MouseInputSystem())
                .Add(new PlayerAISystem())
                .Add(new PlayerMoveSystem())
                .Add(new CameraFollowSystem())
                .Add(new PlayerFoodSystem())
                .Add(new PlayerScaleSystem())
                .Add(new FoodRenderSystem())
                .Add(new PlayerKillPlayerSystem())
#if UNITY_EDITOR
                .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ())
#endif
                .Inject(_gameConfig)
                .Inject(_sceneData)
                .Inject(_foodGrid)
                .Init();
        }

        private void Update () 
        {
            _systems?.Run ();
        }

        private void OnDestroy () 
        {
            _systems?.Destroy ();
            _systems = null;
            
            _world?.Destroy ();
            _world = null;
        }
    }
}