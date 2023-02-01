using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Modules.Enemy
{
	public class WaveDirector : Director
	{
		[SerializeField] float _difficultyCoef = 2.3f;
		[SerializeField] float _creditMultiplier = 1;
		[SerializeField] SpawnDeck _testDeck;

		float _creditsPerSecond => (1+.4f*_difficultyCoef) * _creditMultiplier * 0.5f;
		float _credits;

		void OnEnable() {
			_credits = 0;
			StartCoroutine(SpawnWave(_testDeck));
		}

		void Update() {
			_credits += _creditsPerSecond * Time.deltaTime;
		}

		public IEnumerator SpawnWave(SpawnDeck deck) {
			int enemiesToSpawn = Random.Range(deck.SpawnNumbers.x, deck.SpawnNumbers.y);

			List<Coroutine> spawningCoroutines = new List<Coroutine>();

			while (enemiesToSpawn --> 0) {
				Debug.Log("Attempting to spawn a card");
				do {
					// attempt to choose a card
					SpawnCard card = deck.getRandomCard();

					// check if can afford card and not overcrowded
					bool SUCCESS = (card.Cost <= _credits && EnemyTarget.AllTargets.Count < EnemyTarget.MAXIMUM_TARGETS); 
					if (!SUCCESS) {
						yield return null;
						continue;
					}

					// wait until can afford card and not overcrowded
					// while (card.Cost > _credits || EnemyTarget.AllTargets.Count >= EnemyTarget.MAXIMUM_TARGETS) 
					// 	yield return null;

					_credits -= card.Cost;
					spawningCoroutines.Add(StartCoroutine(TimedSpawn(card))); // does not spawn immediately. Instead, calculate how to spawn on beat.
					yield return new WaitForSeconds(Random.Range(deck.SpawnInterval.x, deck.SpawnInterval.y + 1) * 60.0f / BPM); // wait for interval before attempting the next spawn
					break;

				} while (true);
			}

			// wait until everyone is spawned
			foreach (Coroutine spawningCoroutine in spawningCoroutines) 
				yield return spawningCoroutine;

			// wait till all enemies are killed
			while (EnemyTarget.AllTargets.Count > 0) yield return null;

			_credits = 0.4f * _credits;
		}

		IEnumerator TimedSpawn(SpawnCard card) {
			Debug.Log("SPAWNING : " + card);
			// all values are in beats
			float chargingTime = 1, attackConnectTime = 1, spawningTime = 1, shotCooldownTime = 1;
			{
				Devil devil = ObjectPooler.Instance.GetPooledObject(card.EnemyPoolIndex).GetComponent<Devil>();
				chargingTime = devil.ShotChargingTime;
				spawningTime = devil.SpawnTime;
			}
			shotCooldownTime = card.BeatInterval - chargingTime;
			attackConnectTime = new int[]{1,2,4}[Random.Range(0,3)]; // random between these three numbers

			float spawnTime = (card.BeatInterval - ((chargingTime + attackConnectTime + spawningTime) % card.BeatInterval) + card.BeatOffset) % card.BeatInterval;
			float BPS = BPM / 60.0f;
			spawnTime += (int) (Time.time * BPS / card.BeatInterval) * card.BeatInterval;
			if (Time.time * BPS > spawnTime) spawnTime += card.BeatInterval;

			float spawnTimeSeconds = spawnTime / BPS;
			yield return new WaitForSeconds(spawnTimeSeconds);
			Spawn(card.EnemyPoolIndex, attackConnectTime, shotCooldownTime);
		}
	}
}