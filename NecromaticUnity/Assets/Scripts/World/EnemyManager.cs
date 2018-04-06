using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

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

        void Start()
        {
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
			
		}
    }
}