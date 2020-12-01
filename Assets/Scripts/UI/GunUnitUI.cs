using System.Linq;
using UniRx;
using UniRx.Triggers;
using Unity.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GunUnitUI : MonoBehaviour
{
    [SerializeField]
    private Image image;
    [SerializeField]
    private Text text;
    public static ReactiveProperty<GunUnitUI> selected { get; private set; } = new ReactiveProperty<GunUnitUI>(null);
    private bool loaded; // under bullet info
    private Transform container;

    void Start()
    {
        this.container = transform.parent;
        this.text.text = RandomString(3);
        var trigger = GetComponent<ObservableEventTrigger>();
        gameObject.AddComponent<Text>().text = RandomString(3);
        trigger.OnPointerClickAsObservable()
            .Where(_ => !this.loaded)
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
        this.loaded = true;
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

    public void UnLoad()
    {
        var buttonTransform = this.container.transform.GetChild(this.container.transform.childCount - 1);
        buttonTransform.gameObject.MoveToBeforeSelf(transform);
    }

    private static string RandomString(int length)
    {
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(
            Enumerable.Repeat(chars, length)
            .Select(s => s[Random.Range(0, s.Length)])
            .ToArray()
        );
    }
}