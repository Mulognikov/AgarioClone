using AgarioClone;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig")]
public class GameConfig : ScriptableObject
{
	[Space][Header("Prefabs")]
	public PlayerView PlayerPrefab;
	public FoodView FoodPrefab;

	[Space][Header("World")]
	public int ChunkSize = 10;
	public int WorldWidth  = 100;
	public int WorldHeight = 100;
	public int PlayersCount = 25;
	
	
	[Space][Header("Food")]
	public int FoodCount = 1000;
	public int FoodScore = 1;
	public float FoodSize = 0.25f;
	public Material FoodMaterial;

	[Space] [Header("Player")] 
	public float PlayerStartSize = 1f;
	public int PlayerStartScore = 100;
	public float PlayerSpeed = 5f;
	public Color[] PlayerColors;
	
	[Space][Header("Input Settings")]
	public float MaxSpeedMouseRadiusNormalized = 0.15f;
}
