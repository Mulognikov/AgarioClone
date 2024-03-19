using System;
using UnityEngine;
using UnityEngine.UI;

namespace AgarioClone
{
	[RequireComponent(typeof(Button), typeof(Image))]
	public class ColorButton : MonoBehaviour
	{
		private int _colorIndex;

		public void SetColor(Color color, int index)
		{
			_colorIndex = index;
			GetComponent<Image>().color = color;
			GetComponent<Button>().onClick.AddListener(() =>
			{
				RuntimeData.PlayerColorIndex = _colorIndex;
			});
		}
	}
}