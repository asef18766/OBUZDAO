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
    }
}