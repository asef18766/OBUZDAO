using System;
using System.Collections;
using Bolt;
using Networking;
using UnityEngine;
using UnityEngine.Serialization;

namespace Entity
{
    public class Player : EntityEventListener<IPlayer>, IDestroyable
    {
        private bool up;
        private bool down;
        private bool left;
        private bool right;
        private PlayerAppearance playerAppearance;
        public int playerId;
        private int health = 100;

        #region Shoot_Manipulation

        [SerializeField] private float shootDelay = 0.5f;
        private bool canShoot = true;
        private bool triggerShoot = false;

        private IEnumerator ShootIdle()
        {
            canShoot = false;
            yield return new WaitForSeconds(shootDelay);
            canShoot = true;
        }

        private Vector2 DetectShoot()
        {
            var mousePos = Input.mousePosition;
            if(Camera.main == null) throw new ArgumentException("can not found main camera");
            mousePos.z = Camera.main.nearClipPlane;
            StartCoroutine(ShootIdle());
            return (Camera.main.ScreenToWorldPoint(mousePos) - transform.position).normalized;
        }

        #endregion

        private void Awake()
        {
            playerAppearance = GetComponent<PlayerAppearance>();
        }

        public override void Attached()
        {
            state.SetTransforms(state.Transform, transform);
            state.SetAnimator(GetComponent<Animator>());
        }

        private void PollKeys()
        {
            up = Input.GetKey(KeyCode.W);
            down = Input.GetKey(KeyCode.S);
            left = Input.GetKey(KeyCode.A);
            right = Input.GetKey(KeyCode.D);
            triggerShoot = Input.GetMouseButton(0);
        }

        private void Update()
        {
            PollKeys();
        }

        public override void SimulateController()
        {
            if(!(up || down || left || right || (canShoot && triggerShoot)))
                return;

            var input = PlayerCmd.Create();

            input.Up = up;
            input.Down = down;
            input.Left = left;
            input.Right = right;
            input.Attack = false;

            if(canShoot && triggerShoot)
            {
                input.AtkDir = DetectShoot();
                input.Attack = true;
            }
            print("simulate controller");
            try
            {
                entity.QueueInput(input);
            }
            catch (Exception e)
            {
                BoltLog.Error(e);
                entity.ClearInputQueue();
            }

        }

        public override void MissingCommand(Command previous)
        {
            if(previous == null) return;
            ExecuteCommand(previous, true);
        }

        public override void ExecuteCommand(Command command, bool resetState)
        {
            var cmd = (PlayerCmd) command;

            if(resetState)
            {
                // we got a correction from the server, reset (this only runs on the client)
                playerAppearance.transform.position = cmd.Result.Axes;
            }
            else
            {
                var mvDir = Vector3.zero;
                if(cmd.Input.Down) mvDir += Vector3.down;
                if(cmd.Input.Up) mvDir += Vector3.up;
                if(cmd.Input.Left) mvDir += Vector3.left;
                if(cmd.Input.Right) mvDir += Vector3.right;

                // apply movement (this runs on both server and client)
                playerAppearance.Move(mvDir.normalized);

                // copy the motor state to the commands result (this gets sent back to the client)
                cmd.Result.Axes = playerAppearance.transform.position;

                if(BoltNetwork.IsClient) return;
                if(!cmd.Input.Attack || !canShoot) return;
                StartCoroutine(ShootIdle());
                Bullet.SpawnBullet(playerId, cmd.Input.AtkDir, transform.position);
            }
        }

        #region Game_Events
        public void OnHurt(int dmg)
        {
            BoltLog.Warn($"player {playerId} get hurt for {dmg}");
            health -= dmg;

            var dmgEvent = OnDamaged.Create(entity, EntityTargets.OnlyController);
            dmgEvent.TargetID = Convert.ToInt32(playerId);
            dmgEvent.Dmg = dmg;
            dmgEvent.Send();

            if(health > 0) return;
            OnPlayerDeath.Post(GlobalTargets.AllClients, playerId);
            PlayerRegistry.RemovePlayer(Convert.ToUInt32(playerId));
        }

        public override void OnEvent(OnDamaged evnt)
        {
            BoltLog.Warn($"target id {evnt.TargetID} get hurt for {evnt.Dmg}");
        }

        public void OnUpdateBag(string bagContent)
        {
            BoltLog.Warn("sending update bag event");
            var bagUpdateEvent = OnUpdateBagContent.Create(entity.Controller);
            bagUpdateEvent.BagContent = bagContent;
            bagUpdateEvent.Send();
        }
        #endregion
    }
}