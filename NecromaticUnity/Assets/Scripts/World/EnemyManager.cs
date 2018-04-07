using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Necromatic.Utility;
using Necromatic.Character;
using Necromatic.Character.NPC;
using Necromatic.Character.NPC.Strategies;
using Necromatic.Character.NPC.Strategies.Results;

namespace Necromatic.World
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField] private DayNightManager _dayNightManager;
		private IDisposable _enemySpawner;
		private float _spawnTime = 8f;
		private float _spawnRandomness = 0.1f; // in +- percentage
		private bool _spawn = false;
		private bool _spawnEnemyChunkNow = true;
		private float _spawnCircleRadius = 20f;
		private MotherPool _motherPool;
		private Necromancer _player;

        void Start()
        {
			_motherPool = FindObjectOfType<MotherPool>();
			_player = FindObjectOfType<Necromancer>();
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
			var type = UnityEngine.Random.Range(0, 2) == 0 ? CharacterType.Human : CharacterType.HumanRanged;
			var randomDir = UnityEngine.Random.Range(0f, 2*Mathf.PI);
			var randomLocation = MathUtils.CirclePoint(Vector2.zero, _spawnCircleRadius, randomDir);
			var hooman = _motherPool.GetCharacterPrefab(type);
			var hoomanInstance = Instantiate(hooman, new Vector3(randomLocation.x, 0, randomLocation.y), Quaternion.identity);
			var ai = hoomanInstance.GetComponent<ArtificialIntelligence>();
			var killPlayer = new EnemySpottedResult(_player);
			killPlayer.Priority = 4;
			ai.AddTask(killPlayer);
		}
    }
}