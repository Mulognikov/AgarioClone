using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace AgarioClone
{
	public class SettingsUI : MonoBehaviour
	{
		[SerializeField] private GameConfig _gameConfig;
		[SerializeField] private Slider _musicSlider;
		[SerializeField] private Transform _playerColorsParent;
		[SerializeField] private ColorButton _colorPrefab;
		[SerializeField] private AudioMixer _audioMixer;
		[SerializeField] private Button _backButton;

		private void Awake()
		{
			for (var index = 0; index < _gameConfig.PlayerColors.Length; index++)
			{
				var color = _gameConfig.PlayerColors[index];
				Instantiate(_colorPrefab, _playerColorsParent).SetColor(color, index);
			}

			_musicSlider.onValueChanged.AddListener(OnMusicSlider);
			_backButton.onClick.AddListener(() =>
			{
				gameObject.SetActive(false);
			});
		}

		private void OnMusicSlider(float value)
		{
			_audioMixer.SetFloat("Music", value);
		}
		
	}
}

