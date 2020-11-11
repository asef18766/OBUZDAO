using System;
using UnityEngine;

namespace Entity
{
    public class PlayerAppearance : MonoBehaviour
    {
        [SerializeField] private float moveScale = 0.5f;
        public void Move(Vector3 mvDir)
        {
            GetComponent<Rigidbody2D>().velocity = mvDir * moveScale;
        }
    }
}