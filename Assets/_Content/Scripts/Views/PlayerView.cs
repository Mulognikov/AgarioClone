using UnityEngine;

namespace AgarioClone
{
	[RequireComponent(typeof(SpriteRenderer))]
	public class PlayerView : MonoBehaviour
	{
		private SpriteRenderer _spriteRenderer;

		public void SetColor(Color color)
		{
			if (_spriteRenderer == null)
			{
				_spriteRenderer = GetComponent<SpriteRenderer>();
			}

			_spriteRenderer.color = color;
		}

		public void SetLayer(int layer)
		{
			_spriteRenderer.sortingOrder = layer;
		}
		
	}
}

