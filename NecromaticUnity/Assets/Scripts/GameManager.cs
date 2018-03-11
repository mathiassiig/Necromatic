using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Utility;
using Necromatic.Character;
using UniRx;

namespace Necromatic
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private MotherPool _pool;
		[SerializeField] private CharacterInstance _king;
		[SerializeField] private CharacterInstance _player;

		void Start()
        {
			_king.Death.Dead.Subscribe(dead =>
			{
				if(dead)
					GameWon();
			});

			_player.Death.Dead.Subscribe(dead =>
			{
				if(dead)
					GameOver();
			});
        }

		void GameOver()
		{
			Debug.Log("You lost...");
		}

		void GameWon()
		{
			Debug.Log("You won!");
		}

    }
}