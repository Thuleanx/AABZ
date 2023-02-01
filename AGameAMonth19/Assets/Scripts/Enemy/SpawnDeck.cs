using UnityEngine;
using System.Collections.Generic;
using NaughtyAttributes;

namespace Modules.Enemy
{
	[CreateAssetMenu(fileName = "SpawnDeck", menuName = "~/Spawn Deck", order = 0)]
	public class SpawnDeck : ScriptableObject
	{
		int totalWeight;
		[SerializeField] List<SpawnCard> spawnCards = new List<SpawnCard>();
		[field:SerializeField, MinMaxSlider(1,100)] public Vector2Int SpawnNumbers {get; private set; }
		[field:SerializeField, MinMaxSlider(1,16)] public Vector2Int SpawnInterval {get; private set; }

		public SpawnCard getRandomCard() {
			return spawnCards[0];
		}

		void OnValidate() {
			totalWeight = 0;
			foreach (SpawnCard card in spawnCards)
				totalWeight += card.Weight;
		}
	}
}