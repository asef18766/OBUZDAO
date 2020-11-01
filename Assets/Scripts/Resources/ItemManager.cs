using System;
using UnityEngine;

namespace Resources
{
    [CreateAssetMenu(menuName = "Game Resource/Item Manager")]
    public class ItemManager : ScriptableObject
    {
        private const string AssetPath = "ScriptableObjects/ItemData.asset";
        private static ItemManager _instance = null;
        
        [SerializeField] private ItemInfoStorage itemData = null;
        
        public static ItemManager GetInstance()
        {
            if (_instance == null)
                _instance = UnityEngine.Resources.Load<ItemManager>(AssetPath);
            return _instance;
        }

        public GameObject GetItem(int id)
        {
            if(!itemData.ContainsKey(id)) 
                throw new IndexOutOfRangeException($"try to access item data with id {id}");
            return itemData[id];
        }
    }
}