using System;
using System.Collections;
using Bolt;
using UnityEngine;

namespace Entity
{
    public class Player : EntityEventListener<IPlayer>
    {
        private bool _up;
        private bool _down;
        private bool _left;
        private bool _right;
        private PlayerAppearance _playerAppearance;
        public int playerId;

        #region Shoot_Manipulation
        
        [SerializeField] private float shootDelay = 0.5f;
        private bool _canShoot = true;
        private bool _triggerShoot = false;
        
        private IEnumerator ShootIdle()
        {
            _canShoot = false;
            yield return new WaitForSeconds(shootDelay);
            _canShoot = true;
        }

        private Vector2 DetectShoot()
        {
            var mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane;
            StartCoroutine(ShootIdle());
            return (Camera.main.ScreenToWorldPoint(mousePos) - transform.position).normalized;
        }

        #endregion
        
        
        private void Awake()
        {
            _playerAppearance = GetComponent<PlayerAppearance>();
        }

        public override void Attached()
        {
            state.SetTransforms(state.Transform,transform);
        }
        private void PollKeys()
        {
            _up= Input.GetKey(KeyCode.W);
            _down = Input.GetKey(KeyCode.S);
            _left = Input.GetKey(KeyCode.A);
            _right = Input.GetKey(KeyCode.D);
            _triggerShoot = Input.GetMouseButton(0);
        }

        private void Update()
        {
            PollKeys();
        }

        public override void SimulateController()
        {
            var input = PlayerCmd.Create();
            
            input.Up = _up;
            input.Down = _down;
            input.Left = _left;
            input.Right = _right;
            input.Attack = false;
            
            if (_canShoot && _triggerShoot)
            {
                input.AtkDir = DetectShoot();
                input.Attack = true;
            }

            entity.QueueInput(input);
        }

        public override void ExecuteCommand(Command command, bool resetState)
        {
            var cmd = (PlayerCmd) command;

            if (resetState)
            {
                // we got a correction from the server, reset (this only runs on the client)
                _playerAppearance.transform.position = cmd.Result.Axes;
            }
            else
            {
                var mvDir = Vector3.zero;
                if(cmd.Input.Down) mvDir+= Vector3.down;
                if(cmd.Input.Up) mvDir+= Vector3.up;
                if(cmd.Input.Left) mvDir+= Vector3.left;
                if(cmd.Input.Right) mvDir+= Vector3.right;
                
                // apply movement (this runs on both server and client)
                _playerAppearance.Move(mvDir.normalized);
                
                // copy the motor state to the commands result (this gets sent back to the client)
                cmd.Result.Axes = _playerAppearance.transform.position;
                
                if(BoltNetwork.IsClient) return;
                if (!cmd.Input.Attack || !_canShoot) return;
                StartCoroutine(ShootIdle());
                Bullet.SpawnBullet(playerId, cmd.Input.AtkDir, transform.position);
            }
        }

        #region Game_Events
        public void OnHurt()
        {
            var dmgEvent = OnDamaged.Create(entity);
            dmgEvent.TargetID = Convert.ToInt32(playerId) ;
            dmgEvent.Send();
        }
        public override void OnEvent(OnDamaged evnt)
        {
            BoltLog.Warn(evnt.TargetID);
        }
        #endregion
    }
}