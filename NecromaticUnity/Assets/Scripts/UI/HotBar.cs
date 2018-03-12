using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Necromatic.Character.Abilities;
using UnityEngine.UI;

namespace Necromatic.UI
{
    public class HotBar : MonoBehaviour
    {
		[SerializeField] private Image _currentHighlight;
		void Awake()
		{
			var gm = FindObjectOfType<GameManager>();
			gm.ResearchBank.BankLoaded.Subscribe(loaded =>
			{
				if(loaded)
				{
					VisualizeAbilities(gm.ResearchBank.Abilities);
				}
			});
		}

		private void VisualizeAbilities(List<Ability> abilities)
		{
			for(int i = 0; i < abilities.Count; i++)
			{
				var sprite = Resources.Load<Sprite>(abilities[i].GetIconPath());
				transform.GetChild(i).GetComponent<Image>().sprite = sprite;
			}
		}

		public void SwitchTo(int i)
		{
			var newParent = transform.GetChild(i);
			_currentHighlight.rectTransform.SetParent(newParent);
			_currentHighlight.rectTransform.offsetMax = Vector2.zero;
			_currentHighlight.rectTransform.offsetMin = Vector2.zero;
		}
    }
}