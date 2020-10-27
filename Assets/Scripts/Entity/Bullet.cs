using Bolt;
using UnityEngine;

namespace Entity
{
    public class Bullet : EntityBehaviour<IBullet>
    {
        public Vector2 shootDir;
        public int shooter;
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
            if (!other.CompareTag("Player")) return;
            
            var player = other.gameObject.GetComponent<Player>();
            if (player != null && player.playerId != shooter)
                player.OnHurt();
        }

        public static void SpawnBullet(int playerId, Vector2 atkDir, Vector3 startPos)
        {
            var bullet = BoltNetwork.Instantiate(BoltPrefabs.BulletPref, startPos, Quaternion.identity).GetComponent<Bullet>();
            bullet.shooter = playerId;
            bullet.shootDir = atkDir;
        }
    }
}