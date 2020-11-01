using System;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

namespace Resources
{
    [Serializable]
    public struct ItemMetaDataCollection
    {
        public Sprite Icon;
        public GameObject Usage;
    }
    [Serializable]
    public class ItemInfoStorage:SerializableDictionaryBase<int, ItemMetaDataCollection> { }
}