using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AgarioClone
{
	public class MenuUI : MonoBehaviour
	{
		[SerializeField] private Button _startButton;
		[SerializeField] private Button _settingsButton;
		[SerializeField] private SettingsUI _settings;

		private void Awake()
		{
			_startButton.onClick.AddListener(StartGame);
			_settingsButton.onClick.AddListener(() =>
			{
				_settings.gameObject.SetActive(true);
			});
		}

		private void StartGame()
		{
			SceneManager.LoadScene("Game");
		}
	}
}

