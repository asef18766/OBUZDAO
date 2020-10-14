using Bolt;
using UnityEngine;

namespace Entity
{
    public class Player : EntityBehaviour<IPlayer>
    {
        private bool _up;
        private bool _down;
        private bool _left;
        private bool _right;
        private PlayerAppearance _playerAppearance;

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
            BoltLog.Warn("input receive");
            entity.QueueInput(input);
        }

        public override void ExecuteCommand(Command command, bool resetState)
        {
            BoltLog.Warn("executing command");
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
            }
        }
    }
}