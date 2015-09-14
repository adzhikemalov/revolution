using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class AIManager : MonoBehaviour
    {
        [SerializeField] public GameObject AIPrefab;
        [SerializeField] public List<GameObject> AvailableWorkingPlaces;
        [SerializeField] public int MinSpawnTime;
        [SerializeField] public int MaxSpawnTime;

        private List<GameObject> _freeWorkingPlaces;
        private List<GameObject> _allAIObjects; 

        private int _nextSpawnTime = 0;
        public void Start()
        {
            _freeWorkingPlaces = new List<GameObject>();
            foreach (var availableWorkingPlace in AvailableWorkingPlaces)
            {
                _freeWorkingPlaces.Add(availableWorkingPlace);
            }
        }

        public void Update()
        {
            if (_freeWorkingPlaces.Count > 0)
            {
                if (Game.TimeManager.GameTime >= _nextSpawnTime)
                {
                    SpawnNewAI();
                    SelectNextSpawnTime();
                }
            }
        }

        private void SelectNextSpawnTime()
        {
            _nextSpawnTime = Game.TimeManager.GameTime + Random.Range(MinSpawnTime, MaxSpawnTime);
        }

        private void SpawnNewAI()
        {
            if (_freeWorkingPlaces.Count == 0)
                return;

            var mob = GameObject.Instantiate(AIPrefab);
            mob.transform.position = transform.position;
            var AIScript = mob.GetComponent<CharacterMovement>();
            if (AIScript == null)
            {
                Debug.LogWarning("Cant control this mob");
            }
            else
            {
                var workingPlace = GetFreeWorkingPlace();
                AIScript.TargetPosition = workingPlace.transform.position;
            }
        }

        private GameObject GetFreeWorkingPlace()
        {
            var itemId = Random.Range(0, _freeWorkingPlaces.Count-1);
            var item = _freeWorkingPlaces[itemId];
            _freeWorkingPlaces.Remove(item);
            return item;
        }
    }
}
