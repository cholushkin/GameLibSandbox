using CGTespy.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PickleBase : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public enum BoundaryAlignment
    {
        Free,
        NearestEdge,
        LeftEdge,
        RightEdge,
        TopEdge,
        BottomEdge
    }


    public BoundaryAlignment Alignment;
    public bool Static; // Other pickles can't affect this one
    public float Speed;

    [HideInInspector]
    public RectTransform RectTransform;
    private RectTransform _canvasRect;
    private Rect _activeRect;
    private bool _useSafeArea;
    private Vector2 _normalizedPosition = Vector2.zero;
    private Vector2 _halfSize;
    private bool _isPopupBeingDragged;
    private Image _ghost;



    public void Init(RectTransform canvasRect, Rect activeRect, bool useSafeArea, Image ghost)
    {
        RectTransform = transform as RectTransform;
        _canvasRect = canvasRect;
        _activeRect = activeRect;
        _useSafeArea = useSafeArea;
        _halfSize = RectTransform.sizeDelta * 0.5f;
        _ghost = ghost;
        InitPosition();


        // Check if it's outside canvas

        //Vector3[] corners = new Vector3[4];
        //RectTransform.GetWorldCorners(corners);

        //RectTransformUtility.ScreenPointToLocalPointInRectangle(
        //    canvas.transform as RectTransform,
        //    corners[0],
        //    camera,
        //    out canvasSpaceBottomLeft
        //);
    }

    public void UpdatePosition()
    {

        RectTransform.anchoredPosition = RectTransform.anchoredPosition + Vector2.right * Time.unscaledDeltaTime * Speed;
    }


    public Vector2 CalculateTargetPosition(Rect activeArea)
    {
	    Debug.Log($"CalculateTargetPosition for {name}");

		Debug.Log($"active area {activeArea}");

		Vector2 canvasRawSize = _canvasRect.rect.size;
		Vector2 blPicklePos = canvasRawSize * 0.5f + RectTransform.anchoredPosition; // bottom left pos
        print(blPicklePos);

		float distToLeft = Mathf.Abs(blPicklePos.x - activeArea.xMin);
        float distToRight = Mathf.Abs(blPicklePos.x - activeArea.xMax);
        var alignToLeft = distToLeft < distToRight;

        float distToTop = Mathf.Abs(blPicklePos.y - activeArea.yMax);
        float distToBottom = Mathf.Abs(blPicklePos.y - activeArea.yMin);
        var alignToTop = distToTop < distToBottom;



		Debug.Log($"distToLeft {distToLeft}");
		Debug.Log($"distToRight {distToRight}");
		return new Vector2(alignToLeft? activeArea.xMin : activeArea.xMax, alignToTop ? activeArea.yMax : activeArea.yMin ) - canvasRawSize * 0.5f;
    }

    public void UpdatePosition(bool immediately)
    {
        Vector2 canvasRawSize = _canvasRect.rect.size;

        // Calculate safe area bounds
        float canvasWidth = canvasRawSize.x;
        float canvasHeight = canvasRawSize.y;

        float canvasBottomLeftX = 0f;
        float canvasBottomLeftY = 0f;

        if (_useSafeArea)
        {
#if UNITY_2017_2_OR_NEWER && (UNITY_EDITOR || UNITY_ANDROID || UNITY_IOS)
            Rect safeArea = Screen.safeArea;

            int screenWidth = Screen.width;
            int screenHeight = Screen.height;

            canvasWidth *= safeArea.width / screenWidth;
            canvasHeight *= safeArea.height / screenHeight;

            canvasBottomLeftX = canvasRawSize.x * (safeArea.x / screenWidth);
            canvasBottomLeftY = canvasRawSize.y * (safeArea.y / screenHeight);
#endif
        }

        // Calculate safe area position of the popup
        // normalizedPosition allows us to glue the popup to a specific edge of the screen. It becomes useful when
        // the popup is at the right edge and we switch from portrait screen orientation to landscape screen orientation.
        // Without normalizedPosition, popup could jump to bottom or top edges instead of staying at the right edge
        Vector2 pos = canvasRawSize * 0.5f + (immediately ? new Vector2(_normalizedPosition.x * canvasWidth, _normalizedPosition.y * canvasHeight)
            : (RectTransform.anchoredPosition - new Vector2(canvasBottomLeftX, canvasBottomLeftY)));

        // Find distances to all four edges of the safe area
        float distToLeft = pos.x;
        float distToRight = canvasWidth - distToLeft;

        float distToBottom = pos.y;
        float distToTop = canvasHeight - distToBottom;

        float horDistance = Mathf.Min(distToLeft, distToRight);
        float vertDistance = Mathf.Min(distToBottom, distToTop);

        // Find the nearest edge's safe area coordinates
        if (horDistance < vertDistance)
        {
            if (distToLeft < distToRight)
                pos = new Vector2(_halfSize.x, pos.y);
            else
                pos = new Vector2(canvasWidth - _halfSize.x, pos.y);

            pos.y = Mathf.Clamp(pos.y, _halfSize.y, canvasHeight - _halfSize.y);
        }
        else
        {
            if (distToBottom < distToTop)
                pos = new Vector2(pos.x, _halfSize.y);
            else
                pos = new Vector2(pos.x, canvasHeight - _halfSize.y);

            pos.x = Mathf.Clamp(pos.x, _halfSize.x, canvasWidth - _halfSize.x);
        }

        pos -= canvasRawSize * 0.5f;

        _normalizedPosition.Set(pos.x / canvasWidth, pos.y / canvasHeight);
        

        //// Safe area's bottom left coordinates are added to pos only after normalizedPosition's value
        //// is set because normalizedPosition is in range [-canvasWidth / 2, canvasWidth / 2]
        //pos += new Vector2(canvasBottomLeftX, canvasBottomLeftY);

        //// If another smooth movement animation is in progress, cancel it
        //if (moveToPosCoroutine != null)
        //{
        //	StopCoroutine(moveToPosCoroutine);
        //	moveToPosCoroutine = null;
        //}

        //if (immediately)
        //	popupTransform.anchoredPosition = pos;
        //else
        //{
        //	// Smoothly translate the popup to the specified position
        //	moveToPosCoroutine = MoveToPosAnimation(pos);
        //	StartCoroutine(moveToPosCoroutine);
        //}
    }

    // Popup is clicked
    public void OnPointerClick(PointerEventData data)
    {
        Debug.Log($"on pointer click: delta{data.pressPosition}");
        //// Hide the popup and show the log window
        //if (!isPopupBeingDragged)
        //    debugManager.ShowLogWindow();
    }

    private Vector2 _offsetFromPickleCenter;

    public void OnBeginDrag(PointerEventData data)
    {
        // init ghost for current pickle
        _ghost.enabled = true;
        var clr = GetComponent<Image>().color;
        clr.a = 0.4f;
        _ghost.color = clr;
        _ghost.rectTransform.sizeDelta = RectTransform.sizeDelta;

        _isPopupBeingDragged = true;

        Vector2 localPoint;
        _offsetFromPickleCenter = Vector2.zero;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRect, data.position, null, out localPoint))
            _offsetFromPickleCenter = localPoint - RectTransform.anchoredPosition;
        UpdateDragPosition(data);

        //// If a smooth movement animation is in progress, cancel it
        //if (moveToPosCoroutine != null)
        //{
        //    StopCoroutine(moveToPosCoroutine);
        //    moveToPosCoroutine = null;
        //}
    }

    // Reposition the popup
    public void OnDrag(PointerEventData data)
    {
        UpdateDragPosition(data);
        //Vector2 localPoint;
        //if (RectTransformUtility.ScreenPointToLocalPointInRectangle(debugManager.canvasTR, data.position, data.pressEventCamera, out localPoint))
        //    popupTransform.anchoredPosition = localPoint;
    }

    // Smoothly translate the popup to the nearest edge
    public void OnEndDrag(PointerEventData data)
    {
        _ghost.enabled = false;
        _isPopupBeingDragged = false;
        UpdateDragPosition(data);
    }

    private void InitPosition()
    {
        switch (Alignment)
        {
            case BoundaryAlignment.Free:
                DoBoundariesRestriction();
                break;

        }
    }

    private void UpdateDragPosition(PointerEventData data)
    {
        // Update ghost position
        Vector2 canvasRawSize = _canvasRect.rect.size;
        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRect, data.position, null, out localPoint))
            _ghost.rectTransform.anchoredPosition = localPoint - _offsetFromPickleCenter;
        
        // Update pickle position
        switch (Alignment)
        {
            case BoundaryAlignment.Free:
                RectTransform.anchoredPosition = localPoint - _offsetFromPickleCenter;
                DoBoundariesRestriction();
                break;

        }

    }
   
    private void DoBoundariesRestriction()
    {
        Vector2 canvasRawSize = _canvasRect.rect.size;
        Vector2 blPicklePos = canvasRawSize * 0.5f + RectTransform.anchoredPosition; // bottom left pos

        if (blPicklePos.x - RectTransform.sizeDelta.x * 0.5f < _activeRect.x)
            blPicklePos.x = _activeRect.x + RectTransform.sizeDelta.x * 0.5f;

        if (blPicklePos.x + RectTransform.sizeDelta.x * 0.5f > _activeRect.xMax)
            blPicklePos.x = _activeRect.xMax - RectTransform.sizeDelta.x * 0.5f;

        if (blPicklePos.y + RectTransform.sizeDelta.y * 0.5f > _activeRect.yMax)
            blPicklePos.y = _activeRect.yMax - RectTransform.sizeDelta.y * 0.5f;

        if (blPicklePos.y - RectTransform.sizeDelta.y * 0.5f < _activeRect.yMin)
            blPicklePos.y = _activeRect.yMin + RectTransform.sizeDelta.y * 0.5f;



        RectTransform.anchoredPosition = blPicklePos - canvasRawSize * 0.5f; // convert back to coordinates relative to center
    }


    [ContextMenu("Print pickle state")]
    public void DbgPrintState()
    {
        Debug.Log($"Anchored position: {RectTransform.anchoredPosition}");
        Debug.Log($"SizeDelta: {RectTransform.sizeDelta}");
    }
}

