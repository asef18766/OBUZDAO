using Bolt;
using UnityEngine;

namespace Entity
{
    public class Bullet : EntityBehaviour<IBullet>
    {
        public Vector2 shootDir;
        public int shooter;
        public int dmg;
        
        [SerializeField] private float moveSpeed = 0.4f;
        public override void Attached()
        {
            state.SetTransforms(state.Transform,transform);
        }

        private void Update()
        {
            transform.Translate(shootDir.normalized * (BoltNetwork.FrameDeltaTime * moveSpeed));
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            IDestroyable target;
            switch (other.tag)
            {
                case "Player":
                    var player = other.gameObject.GetComponent<Player>();
                    if (player == null || player.playerId == shooter) return;
                    target = player;
                    break;
                case "Terrain":
                    target = other.gameObject.GetComponent<IDestroyable>();
                    break;
                default:
                    return;
            }

            if (target == null)
            {
                BoltLog.Warn("return with hitting an null object");
                return;
            }
            target.OnHurt(dmg);
            BoltNetwork.Destroy(gameObject);
        }

        public static void SpawnBullet(int playerId, Vector2 atkDir, Vector3 startPos)
        {
            var bullet = BoltNetwork.Instantiate(BoltPrefabs.BulletPref, startPos, Quaternion.identity).GetComponent<Bullet>();
            bullet.shooter = playerId;
            bullet.shootDir = atkDir;
            bullet.dmg = 20;
        }
    }
}