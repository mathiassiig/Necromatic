using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Necromatic.UI.Research
{
	public class ResearchData
	{
		public string Name;
		public int Cost;
		public string IconPath;
		public UnityAction OnPurchase;

		public ResearchData(string name, int cost, string iconPath, UnityAction onPurchase)
		{
			Name = name;
			Cost = cost;
			IconPath = iconPath;
			OnPurchase = onPurchase;
		}
	}

    public class ResearchManager : MonoBehaviour
    {
		[SerializeField] private AbilityUI _abilityUIPrefab;
		[SerializeField] private RectTransform _abilitiesLocation;

		private static string _iconLocation = "Images/UI/Icons/";

		private List<ResearchData> _researchAbilities = new List<ResearchData>()
		{
			new ResearchData("Sacrifice", 1, $"{_iconLocation}{"icon_sacrifice"}", () => print("Bought Sacrifice"))
		};

		void Awake()
		{
			foreach(var a in _researchAbilities)
			{
				var instance = Instantiate(_abilityUIPrefab);
				instance.transform.SetParent(_abilitiesLocation);
				instance.Init(a);
			}
		}

		
    }
}