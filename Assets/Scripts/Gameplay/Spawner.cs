using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways]
public class Spawner : MonoBehaviour
{
	// Spawning is equally likely to occur in the entire area within the circle defined by maxRadius.
	public float SpawnRadius;

	// Spawns at least min and at most max entities
	public int MaxSpawnQuantity;
	public int MinSpawnQuantity;

	// A reference to the prefab of the object to spawn
	public GameObject spawnPrefab;

	public SpawnType spawnType;
	public SpawnerEditorPreviewData editorPreviewData;

	public Color customDiscColor;
	public float customDiscThickness;

	private void OnEnable() => SpawnManager.CurrentSpawners.Add(this);
	private void OnDisable() => SpawnManager.CurrentSpawners.Remove(this);

	public void Spawn()
	{
		Vector3 pos = gameObject.transform.position;
		for (int i = 0; i < Random.Range(MinSpawnQuantity, MaxSpawnQuantity); ++i)
		{
			float r = Mathf.Sqrt(Random.Range(0, SpawnRadius * SpawnRadius));
			float theta = Random.Range(0.0f, 2.0f * Mathf.PI);

			float relativeY = r * Mathf.Sin(theta);
			float relativeX = r * Mathf.Cos(theta);

			GameObject newObject = Instantiate(spawnPrefab);
			newObject.transform.position = new Vector3(pos.x + relativeX, pos.y, pos.z + relativeY);
		}
	}


	private void OnValidate()
	{
		ApplyCustomEditorPreviewData();
	}

	public void ApplySpawnType()
	{
		if (spawnType != SpawnType.Custom)
		{
			editorPreviewData = Resources.Load($"Data/Spawners/{spawnType.ToString()}") as SpawnerEditorPreviewData;
		}
		else
		{
			editorPreviewData = Instantiate(Resources.Load($"Data/Spawners/Neutral") as SpawnerEditorPreviewData);
		}

		customDiscColor = editorPreviewData.discColor;
		customDiscThickness = editorPreviewData.discThickness;
	}

	public void ApplyCustomEditorPreviewData()
	{
		editorPreviewData.discColor = customDiscColor;
		editorPreviewData.discThickness = customDiscThickness;
	}

#if UNITY_EDITOR
	private void OnDrawGizmosSelected()
	{
		Color colorCache = Handles.color;
		Handles.color = editorPreviewData.discColor;
		Handles.DrawWireDisc(transform.position, transform.up, SpawnRadius, editorPreviewData.discThickness);
		Handles.color = colorCache;
	}
#endif
}