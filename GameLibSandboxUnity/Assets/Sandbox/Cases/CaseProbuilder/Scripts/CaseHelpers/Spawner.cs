using System.Collections;
using DG.Tweening;
using GameLib.Random;
using ResourcesHelper;
using UnityEngine;

namespace Cases
{
    public class Spawner : MonoBehaviour
    {
        public long Seed;
        public Range Delay;
        public int Count;
        public PrefabHolder Prefabs;
        public Transform Parent;
        public bool DestroyPrev;
        public bool PlaySpawnAnimation;

        private IPseudoRandomNumberGenerator _rnd;
        private GameObject _lastSpawned;

        void Awake()
        {
            _rnd = RandomHelper.CreateRandomNumberGenerator(-1);
            Seed = _rnd.GetState().AsNumber();
            StartCoroutine(Respawn());
        }

        IEnumerator Respawn()
        {
            for (int i = 0; i < Count; ++i)
            {
                yield return new WaitForSeconds(i == 0 ? 0f : _rnd.FromRange(Delay));
                var prefab = Prefabs.Prefabs[i % Prefabs.Prefabs.Objects.Length];
                var prevSpawned = _lastSpawned;
                _lastSpawned = Instantiate(prefab, transform.position, transform.rotation);
                _lastSpawned.name = $"{prefab.name}{i}";
                _lastSpawned.transform.SetParent(Parent);
                if (PlaySpawnAnimation)
                    _lastSpawned.transform.DOPunchScale(Vector3.one * 0.3f, 0.2f, 2, 0.5f);
                yield return null;
                if (DestroyPrev)
                    Destroy(prevSpawned);
            }
        }
    }
}
