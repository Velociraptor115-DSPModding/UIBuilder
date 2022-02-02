using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace DysonSphereProgram.Modding.UI
{
	public class UIWindowResize : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
	{
		private void OnEnable()
		{
			var pointerDownEventEntry = new EventTrigger.Entry();
			pointerDownEventEntry.eventID = EventTriggerType.PointerDown;
			pointerDownEventEntry.callback.AddListener(new UnityAction<BaseEventData>(OnPointerDown));
			resizeTrigger.triggers.Add(pointerDownEventEntry);
			var pointerUpEventEntry = new EventTrigger.Entry();
			pointerUpEventEntry.eventID = EventTriggerType.PointerUp;
			pointerUpEventEntry.callback.AddListener(new UnityAction<BaseEventData>(OnPointerUp));
			resizeTrigger.triggers.Add(pointerUpEventEntry);

			var pointerEnterEventEntry = new EventTrigger.Entry();
			pointerEnterEventEntry.eventID = EventTriggerType.PointerEnter;
			pointerEnterEventEntry.callback.AddListener(new UnityAction<BaseEventData>(OnPointerEnterResizeArea));
			resizeTrigger.triggers.Add(pointerEnterEventEntry);
			var pointerExitEventEntry = new EventTrigger.Entry();
			pointerExitEventEntry.eventID = EventTriggerType.PointerExit;
			pointerExitEventEntry.callback.AddListener(new UnityAction<BaseEventData>(OnPointerExitResizeArea));
			resizeTrigger.triggers.Add(pointerExitEventEntry);


			if (resizeTrans == null)
			{
				resizeTrans = transform as RectTransform;
			}
			VFAudio.Create("common-popup-3", null, Vector3.zero, true, 2, -1, -1L);
		}

		private void OnDisable()
		{
			resizeTrigger.triggers.Clear();
			mouseDown = false;
			resizeMouseBegin = Vector2.zero;
			resizeSizeDeltaBegin = Vector2.zero;
			if (resizing)
			{
				resizeTrans.sizeDelta = new Vector2(Mathf.Round(resizeSizeDeltaWanted.x), Mathf.Round(resizeSizeDeltaWanted.y));
			}
			pointerIn = false;
		}

		private void OnApplicationFocus(bool focus)
		{
			if (!focus)
			{
				pointerIn = false;
			}
		}

		private void OnPointerDown(BaseEventData eventData)
		{
			mouseDown = true;
			UIRoot.ScreenPointIntoRect(Input.mousePosition, resizeTrans, out resizeMouseBegin);
			resizeSizeDeltaBegin = resizeTrans.sizeDelta;
			resizeSizeDeltaWanted = resizeTrans.sizeDelta;
			UICursor.SetCursor(ECursor.TargetIn);
		}

		private void OnPointerUp(BaseEventData eventData)
		{
			mouseDown = false;
			resizeMouseBegin = Vector2.zero;
			resizeSizeDeltaBegin = Vector2.zero;
		}

		private void OnPointerEnterResizeArea(BaseEventData eventData)
    {
			mouseOver = true;
    }

		private void OnPointerExitResizeArea(BaseEventData eventData)
		{
			mouseOver = false;
		}

		public bool resizing { get; private set; }

		private void BringToFront()
		{
			int childCount = transform.parent.childCount;
			if (transform.GetSiblingIndex() < childCount - 1)
			{
				transform.SetAsLastSibling();
			}
		}

		private void Update()
		{
			if (mouseDown)
			{
				if (!Input.GetMouseButton(0) && !Input.GetMouseButton(1))
				{
					mouseDown = false;
				}

				UIRoot.ScreenPointIntoRect(Input.mousePosition, resizeTrans, out var newPoint);
				var mousePosDiff = newPoint - resizeMouseBegin;
				if (Mathf.Abs(mousePosDiff.x) + Mathf.Abs(mousePosDiff.y) > resizeThreshold)
				{
					resizing = true;
					resizeSizeDeltaWanted = resizeSizeDeltaBegin + new Vector2(mousePosDiff.x, -mousePosDiff.y);
					resizeTrans.sizeDelta =
						new Vector2(
							Mathf.Round(Mathf.Max(resizeSizeDeltaWanted.x, minSize.x)),
							Mathf.Round(Mathf.Max(resizeSizeDeltaWanted.y, minSize.y))
						);
				}
				UICursor.SetCursor(ECursor.TargetIn);
			}
			else
			{
				resizing = false;
				resizeMouseBegin = Vector2.zero;
				resizeSizeDeltaBegin = Vector2.zero;
				if (mouseOver)
          UICursor.SetCursor(ECursor.TargetOut);
			}
			if (pointerIn && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)))
			{
				BringToFront();
			}
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			pointerIn = true;
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			pointerIn = false;
		}

		public EventTrigger resizeTrigger;

		public RectTransform resizeTrans;

		public int resizeThreshold = 4;

		public Vector2 minSize;

		private bool mouseDown;

		private bool mouseOver;

		private Vector2 resizeMouseBegin;

		private Vector2 resizeSizeDeltaBegin;

		private Vector2 resizeSizeDeltaWanted;

		public bool pointerIn;
	}
}
