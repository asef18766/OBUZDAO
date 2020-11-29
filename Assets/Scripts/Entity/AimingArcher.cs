using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Entity
{
    public class AimingArcher : MonoBehaviour
    {
        public Vector2 AtkDir = Vector2.up;
        public float DelaySec = 1.0f;
        public int Dmg = 10;
        public int Owner = -1;
        public float attackRadius;

        private HashSet<Player> targets = new HashSet<Player>();

        private void OnTriggerEnter2D(Collider2D other)
        {
            var player = other.GetComponent<Player>();
            if(player)
            {
                Debug.Assert(!this.targets.Contains(player), $"Player {player.playerId} should not appear in targets");
                this.targets.Add(player);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var player = other.GetComponent<Player>();
            if(player)
            {
                Debug.Assert(this.targets.Contains(player), $"Player {player.playerId}  disappear in targets");
                this.targets.Remove(player);
            }
        }

        private float distance(Transform other)
        {
            return Vector3.Distance(transform.position, other.position);
        }

        private Player selectTarget()
        {
            if(this.targets.Count != 0)
                return null;
            Player ret = null;
            float minDis = float.MaxValue;
            foreach(var player in this.targets)
            {
                if(!ret || minDis > distance(player.transform))
                {
                    ret = player;
                    minDis = distance(ret.transform);
                }
            }
            return ret;
        }

        private IEnumerator AtkRoutine()
        {
            while(true)
            {
                var target = this.selectTarget();
                if(target)
                {
                    var bullet = BoltNetwork.Instantiate(
                        BoltPrefabs.BulletPref,
                        transform.position,
                        Quaternion.identity
                    ).GetComponent<Bullet>();
                    bullet.dmg = Dmg;
                    bullet.shooter = Owner;
                    bullet.shootDir = (target.transform.position - transform.position).normalized;
                    yield return new WaitForSeconds(DelaySec);
                }
                else
                {
                    yield return null;
                }
            }
        }

        private void Start()
        {
            // setup collider
            var collider = gameObject.AddComponent<CircleCollider2D>();
            collider.radius = this.attackRadius;
            collider.isTrigger = true;
            if(BoltNetwork.IsServer)
                StartCoroutine(AtkRoutine());
        }
    }
}