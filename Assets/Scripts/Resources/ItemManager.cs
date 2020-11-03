using System;
using UnityEngine;

namespace Resources
{
    [CreateAssetMenu(menuName = "Game Resource/Item Manager")]
    public class ItemManager : ScriptableObject
    {
        private const string AssetPath = "ScriptableObjects/ItemData";
        private static ItemManager instance = null;
        
        [SerializeField] private ItemInfoStorage itemData = null;
        
        public static ItemManager GetInstance()
        {
            if (instance == null)
                instance = UnityEngine.Resources.Load<ItemManager>(AssetPath);
            return instance;
        }

        public Sprite GetIcon(int id)
        {
            if(!itemData.ContainsKey(id)) 
                throw new IndexOutOfRangeException($"try to access item data with id {id}");
            return itemData[id].Icon;
        }
    }
}