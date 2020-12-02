// ref: https://qiita.com/tricrow/items/45b0695f168aca6be11a
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace UI
{
    public class DragAndDrop : MonoBehaviour
    {
        void Start()
        {
            var gunUnitUI = GetComponent<GunUnitUI>();
            // 親CanvasのRectTransformを基準に利用する
            var rectTransform = transform.root.gameObject.GetComponent<RectTransform>();
            // RectTransformUtility.ScreenPointToLocalPointInRectangle用使い捨て変数
            var tmpPosition = Vector2.zero;
            // get the position of parent's top-left corner in world space
            // i guess it only work when canvas use Screen space - Overlay mode
            var selfRect = GetComponent<RectTransform>();
            var parentRect = transform.parent.GetComponent<RectTransform>();
            // FIXME: i don't know why all the rect's property will be 0...
            // var anchorPosition = parentRect.TransformPoint(0, parentRect.rect.height, 0);
            var anchorPosition = parentRect.TransformPoint(0, 400, 0);
            Debug.Log(anchorPosition);
            // ObservableEventTriggerをオブジェクトに追加
            ObservableEventTrigger trigger = GetComponent<ObservableEventTrigger>();
            // Dragイベントの登録
            trigger.OnDragAsObservable()
                // 発生するたびにマウスポジションを取得する。 
                .Select(_ => Input.mousePosition - anchorPosition)
                // ただしInput.mousePositionはWorld座標なのでそのままでは使えない。親CanvasのUI座標に変換する。
                // .Select(mousePosition =>
                // {
                //     // ※注: 親CanvasのRender ModeをScreen Space Cameraにしている場合はこれでよいが、Screen Space Overlayの場合はまた別の記述になる。
                //     //   http://tsubakit1.hateblo.jp/entry/2016/03/01/020510 を参照。
                //     // RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, mousePosition, Camera.main, out tmpPosition);
                //     // return tmpPosition
                //     return mousePosition - new Vector3(188, 508); // anchor position
                // })
                // ついでにログを取ってみる
                .Do(position => Debug.Log(string.Format("[Drag]Input.mousePosition.x: {0}, Input.mousePosition.y: {1}, Canvas.LocalPosition.x: {2}, Canvas.LocalPosition.y: {3}",
                    Input.mousePosition.x, Input.mousePosition.y, position.x, position.y)))
                // 自分の位置をマウスのUI座標に。 
                // .Subscribe(position => transform.localPosition = position)
                .Subscribe(position => selfRect.anchoredPosition = position)
                // 自分が破壊されたらイベントも終了。
                .AddTo(this);

            // Drag終了イベント(つまりDrop)の登録
            trigger.OnEndDragAsObservable()
                // 発生したらマウスポジションを取得する。 
                .Select(_ => Input.mousePosition)
                .Select(mousePosition =>
                {
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, mousePosition, Camera.main, out tmpPosition);
                    return tmpPosition;
                })
                // ドロップされたらログを追加
                .Subscribe(position => Debug.Log(string.Format("[Drop]Input.mousePosition.x: {0}, Input.mousePosition.y: {1}, Canvas.LocalPosition.x: {2}, Canvas.LocalPosition.y: {3}",
                    Input.mousePosition.x, Input.mousePosition.y, position.x, position.y)))
                // 自分が破壊されたらイベントも終了。
                .AddTo(this);

        }
    }
}