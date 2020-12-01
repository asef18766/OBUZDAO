using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class BulletInfo : MonoBehaviour
{
    [SerializeField]
    private Transform content;

    void Start()
    {
        var trigger = GetComponent<ObservableEventTrigger>();
        trigger.OnPointerClickAsObservable()
            // a gun unit is selected
            .Where(_ => GunUnitUI.selected.Value)
            .Do(_ => Debug.Log($"clicked! {gameObject.name}"))
            .Subscribe(_ => GunUnitUI.selected.Value.AttatchToBulletInfo(this))
            .AddTo(this);
    }

    public void AddGunUnit(GunUnitUI gunUnitUI)
    {
        gunUnitUI.transform.SetParent(this.content);
        // remove from this when clicked
        gunUnitUI.GetComponent<ObservableEventTrigger>()
            .OnPointerClickAsObservable()
            .Where(_ => !GunUnitUI.selected.Value)
            .TakeWhile(_ => gunUnitUI.transform.parent == this.content.transform)
            .Subscribe(_ => gunUnitUI.UnLoad());
    }
}