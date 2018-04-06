using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Necromatic.Utility;
using Necromatic.Character;

namespace Necromatic.World
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField] private DayNightManager _dayNightManager;
		private IDisposable _enemySpawner;
		private float _spawnTime = 2;
		private float _spawnRandomness = 0.1f; // in +- percentage
		private bool _spawn = false;
		private bool _spawnEnemyChunkNow = true;
		private MotherPool _motherPool;

        void Start()
        {
			_motherPool = FindObjectOfType<MotherPool>();
			_dayNightManager.IsDay.Subscribe(day =>
			{
				_spawn = day;
			});
        }

		void Update()
		{
			if(_spawn)
			{
				if(_spawnEnemyChunkNow)
				{
					_spawnEnemyChunkNow = false;
					var delay = UnityEngine.Random.Range(_spawnTime - _spawnTime*_spawnRandomness, _spawnTime + _spawnTime*_spawnRandomness);
					Observable.Timer(TimeSpan.FromSeconds(delay)).TakeUntilDestroy(this).Subscribe(_ =>
					{
						_spawnEnemyChunkNow = true;
					});
					SpawnEnemyChunk();
				}
			}
		}

		void SpawnEnemyChunk()
		{
			var randomDir = UnityEngine.Random.Range(0f, 2*Mathf.PI);
			var randomLocation = MathUtils.CirclePoint(Vector2.zero, 10, randomDir);
			var hooman = _motherPool.GetCharacterPrefab(CharacterType.Human);
			Instantiate(hooman, new Vector3(randomLocation.x, 0, randomLocation.y), Quaternion.identity);
		}
    }
}