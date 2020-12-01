using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

public class GunUnitUI : MonoBehaviour
{
    [SerializeField]
    private Image image;
    public static ReactiveProperty<GunUnitUI> selected { get; private set; } = new ReactiveProperty<GunUnitUI>(null);
    private bool loaded; // under bullet info

    void Start()
    {
        var trigger = GetComponent<ObservableEventTrigger>();
        trigger.OnPointerClickAsObservable()
            .Do(_ => Debug.Log($"clicked! {gameObject.name}"))
            .Do(_ => GunUnitUI.selected.Value?.OnUnSelected())
            .Subscribe(_ => this.OnSelected())
            .AddTo(this);
        // GunUnitUI.selected
        //     .Do(unit => Debug.Log($"selected {unit?.name}"))
        //     .Subscribe(unit => this.image.color = unit == this ? Color.red : Color.white)
        //     .AddTo(this);
        // select a used unit
        GunUnitUI.selected
            .Where(unit => this.loaded && unit == this)
            .Subscribe(unit => { })
            .AddTo(this);
    }

    public void AttatchToBulletInfo(BulletInfo info)
    {
        info.AddGunUnit(GunUnitUI.selected.Value);
        this.OnUnSelected();
    }

    public void OnUnSelected()
    {
        GunUnitUI.selected.Value = null;
        this.image.color = Color.white;
    }

    public void OnSelected()
    {
        GunUnitUI.selected.Value = this;
        this.image.color = Color.red;
    }
}