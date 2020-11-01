using System;
using System.Collections.Generic;

namespace Resources
{
    public class BagManager
    {
        private static BagManager _instance = null;
        private Dictionary<int, Bag> _playerInv = null;
        
        public static BagManager GetInstance()
        {
            return _instance ?? (_instance = new BagManager());
        }

        private BagManager()
        {
            _playerInv = new Dictionary<int, Bag>();
        }

        public Bag GetBag(int playerId)
        {
            if (!_playerInv.ContainsKey(playerId))
                throw new IndexOutOfRangeException($"try to requesting with player id {playerId}");
            return _playerInv[playerId];
        }
        public void CreatePlayerBag(int playerId)
        {
            if (_playerInv.ContainsKey(playerId))
                throw new IndexOutOfRangeException($"try to create bag with player id {playerId}");
            _playerInv.Add(playerId, new Bag {OwnerId = Convert.ToUInt32(playerId)});
        }

        public void RemovePlayerBag(int playerId)
        {
            if (!_playerInv.ContainsKey(playerId))
                throw new IndexOutOfRangeException($"try to delete bag with player id {playerId}");
            _playerInv.Remove(playerId);
        }
    }
}