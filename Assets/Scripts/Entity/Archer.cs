using System.Collections;
using UnityEngine;

namespace Entity
{
    public class Archer : MonoBehaviour
    {
        public Vector2 AtkDir = Vector2.up;
        public float DelaySec = 1.0f;
        public int Dmg = 10;
        public int Owner = -1;
        private IEnumerator AtkRoutine()
        {
            while (true)
            {
                var bullet = BoltNetwork.Instantiate(BoltPrefabs.BulletPref, transform.position, Quaternion.identity)
                    .GetComponent<Bullet>();
                bullet.dmg = Dmg;
                bullet.shooter = Owner;
                bullet.shootDir = AtkDir;
                yield return new WaitForSeconds(DelaySec);
            }
        }
        private IEnumerator Start()
        {
            if (BoltNetwork.IsServer)
                StartCoroutine(AtkRoutine());
            
            yield break;
        }
    }
}