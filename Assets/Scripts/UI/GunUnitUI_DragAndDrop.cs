using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class GunUnitUI_DragAndDrop : MonoBehaviour
{
    public float triggerDistance;
    public bool isDragging;
    private BulletInfo[] bulletInfos;
    private Vector3 offset = Vector2.down * 400; // magic offset, i currently don't know why

    // Start is called before the first frame update
    void Start()
    {
        this.bulletInfos = FindObjectsOfType<BulletInfo>();
        var trigger = GetComponent<ObservableEventTrigger>();
        trigger.OnPointerClickAsObservable()
            .Subscribe(_ => Debug.Log($"clicked! {gameObject.name}"))
            .AddTo(this);
        // trigger.OnDragAsObservable()
        //     .Do(_ =>
        //     {
        //         foreach(var bulletInfo in this.bulletInfos)
        //         {
        //             Debug.Log($"{transform.position} {bulletInfo.transform.position}");
        //             if(Vector3.Distance(transform.position + this.offset, bulletInfo.transform.position) < this.triggerDistance)
        //             {
        //                 // highlight it
        //                 Debug.Log(bulletInfo.name);
        //                 break;
        //             }
        //         }
        //     })
        //     .Subscribe()
        //     .AddTo(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Triggered.");
        Debug.Log($"eq? {other.transform == this.bulletInfos[0].transform}");
        Debug.Log($"{other.transform.position} {this.bulletInfos[0].transform.position}");
        if(other.CompareTag("BulletInfo"))
        {
            Debug.Log($"Got you! {other.name}");
        }
    }
}