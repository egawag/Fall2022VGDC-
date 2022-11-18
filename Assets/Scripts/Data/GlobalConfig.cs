using UnityEngine;

[CreateAssetMenu(fileName = "NewGlobalConfig", menuName = "Configs/Global Config")]
[System.Serializable]
public class GlobalConfig : ScriptableObject
{
	public GameObject PauseMenuPrefab;
	public GameObject GameOverMenuPrefab;
	public GameObject SceneTransitionOverlayPrefab;
	[Expandable]
	public GameplayTuningValues PrimaryGameplayTuningValues;
}
