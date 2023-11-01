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
    public bool StickToCorners;

    [HideInInspector]
    public RectTransform RectTransform;
    private RectTransform _canvasRect;
    private Rect _activeRect;
    private Vector2 _normalizedPosition = Vector2.zero;
    private Vector2 _halfSize;
    private Image _ghost;
    private Vector2 _offsetFromPickleCenter;


    public void Init(RectTransform canvasRect, Rect activeRect, Image ghost)
    {
        RectTransform = transform as RectTransform;
        _canvasRect = canvasRect;
        _activeRect = activeRect;
        _halfSize = RectTransform.sizeDelta * 0.5f;
        _ghost = ghost;
        InitPosition();
    }

    public void OnScreenChange(RectTransform canvasRect, Rect activeRect)
    {
        _canvasRect = canvasRect;
        _activeRect = activeRect;

        // Set position from normalized position
        float x = _activeRect.x + _activeRect.width * _normalizedPosition.x;
        float y = _activeRect.y + _activeRect.height * _normalizedPosition.y;
        RectTransform.anchoredPosition = BottomLeftPostToAnchoredPos(new Vector2(x, y));
        InitPosition();
    }

    public BoundaryAlignment CalculateNearest(Rect activeArea)
    {
        Vector2 blPicklePos = AnchoredPosToBottomLeftPos(RectTransform.anchoredPosition); // bottom left pos

        float distToLeft = Mathf.Abs(blPicklePos.x - activeArea.xMin);
        float distToRight = Mathf.Abs(blPicklePos.x - activeArea.xMax);
        float distToTop = Mathf.Abs(blPicklePos.y - activeArea.yMax);
        float distToBottom = Mathf.Abs(blPicklePos.y - activeArea.yMin);

        float minHoriz = distToLeft;
        BoundaryAlignment minHorizAlignment = BoundaryAlignment.LeftEdge;
        if (distToLeft > distToRight)
        {
            minHoriz = distToRight;
            minHorizAlignment = BoundaryAlignment.RightEdge;
        }

        float minVert = distToBottom;
        BoundaryAlignment minVertAlignment = BoundaryAlignment.BottomEdge;
        if (distToTop < distToBottom)
        {
            minVert = distToTop;
            minVertAlignment = BoundaryAlignment.TopEdge;
        }

        return minVert < minHoriz ? minVertAlignment : minHorizAlignment;
    }

    public void OnPointerClick(PointerEventData data)
    {
    }

    public void OnBeginDrag(PointerEventData data)
    {
        // init ghost for current pickle
        _ghost.enabled = true;
        var clr = GetComponent<Image>().color;
        clr.a = 0.4f;
        _ghost.color = clr;
        _ghost.rectTransform.sizeDelta = RectTransform.sizeDelta;

        Vector2 localPoint;
        _offsetFromPickleCenter = Vector2.zero;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRect, data.position, null, out localPoint))
            _offsetFromPickleCenter = localPoint - RectTransform.anchoredPosition;
        UpdateDragPosition(data);
    }

    // Reposition the popup
    public void OnDrag(PointerEventData data)
    {
        UpdateDragPosition(data);
    }

    // Smoothly translate the popup to the nearest edge
    public void OnEndDrag(PointerEventData data)
    {
        _ghost.enabled = false;
        UpdateDragPosition(data);
    }

    private void InitPosition()
    {
        HandlePosition();
    }

    private void UpdateDragPosition(PointerEventData data)
    {
        // Update ghost position
        Vector2 canvasRawSize = _canvasRect.rect.size;
        Vector2 ghostCanvasCenPos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRect, data.position, null, out ghostCanvasCenPos))
            _ghost.rectTransform.anchoredPosition = ghostCanvasCenPos - _offsetFromPickleCenter;


        // Update pickle position
        RectTransform.anchoredPosition = ghostCanvasCenPos - _offsetFromPickleCenter;
        HandlePosition();
    }

    private void HandlePosition()
    {
        Vector2 canvasRawSize = _canvasRect.rect.size;
        Vector2 blPicklePos = AnchoredPosToBottomLeftPos(RectTransform.anchoredPosition); // Original pos
        var normalizedSize = RectTransform.sizeDelta / _activeRect.size;

        var handleAlignment = Alignment;
        if (Alignment == BoundaryAlignment.NearestEdge)
            handleAlignment = CalculateNearest(_activeRect);

        // 1. align to border of _activeRect (center of RectTransform)
        // 2. save normalized related to _activeRect
        // 3. DoBoundariesRestriction
        switch (handleAlignment)
        {
            case BoundaryAlignment.Free:
            {
                blPicklePos = AnchoredPosToBottomLeftPos(RectTransform.anchoredPosition); // Convert to left-bottom coordinate
                _normalizedPosition.Set((blPicklePos.x - _activeRect.x) / _activeRect.width, (blPicklePos.y - _activeRect.y) / _activeRect.height); // Normalized position of pickle center on the left edge of the active rect

                // Mini-stick
                // Stick to top-left corner
                if (_normalizedPosition.x - normalizedSize.x * 0.5f < 0f && _normalizedPosition.y + normalizedSize.y * 0.5f >= 1f)
                {
                    _normalizedPosition.x = 0f;
                    _normalizedPosition.y = 1f;
                }

                // Stick to top-right corner
                if (_normalizedPosition.x + normalizedSize.x * 0.5f >= 1f && _normalizedPosition.y + normalizedSize.y * 0.5f >= 1f)
                {
                    _normalizedPosition.x = 1f;
                    _normalizedPosition.y = 1f;
                }

                // Stick to bottom-left corner
                if (_normalizedPosition.x - normalizedSize.x * 0.5f < 0f && _normalizedPosition.y - normalizedSize.y * 0.5f < 0f)
                {
                    _normalizedPosition.x = 0f;
                    _normalizedPosition.y = 0f;
                }

                // Stick to bottom-right corner
                if (_normalizedPosition.x + normalizedSize.x * 0.5f >= 1f && _normalizedPosition.y - normalizedSize.y * 0.5f < 0f)
                {
                    _normalizedPosition.x = 1f;
                    _normalizedPosition.y = 0f;
                }


                // Regular stick
                // Stick to top-left corner
                if (_normalizedPosition.x - normalizedSize.x < 0f && _normalizedPosition.y + normalizedSize.y >= 1f)
                {
                    _normalizedPosition.x = 0f;
                    _normalizedPosition.y = 1f;
                }

                // Stick to top-right corner
                if (_normalizedPosition.x + normalizedSize.x >= 1f && _normalizedPosition.y + normalizedSize.y >= 1f)
                {
                    _normalizedPosition.x = 1f;
                    _normalizedPosition.y = 1f;
                }

                // Stick to bottom-left corner
                if (_normalizedPosition.x - normalizedSize.x < 0f && _normalizedPosition.y - normalizedSize.y < 0f)
                {
                    _normalizedPosition.x = 0f;
                    _normalizedPosition.y = 0f;
                }

                // Stick to bottom-right corner
                if (_normalizedPosition.x + normalizedSize.x >= 1f && _normalizedPosition.y - normalizedSize.y < 0f)
                {
                    _normalizedPosition.x = 1f;
                    _normalizedPosition.y = 0f;
                }

                // Sync back to normalized pos
                float x = _activeRect.x + _activeRect.width * _normalizedPosition.x;
                float y = _activeRect.y + _activeRect.height * _normalizedPosition.y;
                RectTransform.anchoredPosition = BottomLeftPostToAnchoredPos(new Vector2(x, y));

                DoBoundariesRestriction();
                break;
            }
            case BoundaryAlignment.LeftEdge:
                {
                    RectTransform.anchoredPosition = BottomLeftPostToAnchoredPos(new Vector2(_activeRect.xMin, blPicklePos.y)); // Clamp to left edge
                    blPicklePos = AnchoredPosToBottomLeftPos(RectTransform.anchoredPosition); // Convert to left-bottom coordinate
                    _normalizedPosition.Set(0f, (blPicklePos.y - _activeRect.y) / _activeRect.height); // Normalized position of pickle center on the left edge of the active rect

                    // For rotation - check if attached to a corner. Mini-stick to top and bottom
                    if (_normalizedPosition.y + normalizedSize.y * 0.5f >= 1f)
                        _normalizedPosition.y = 1f;
                    if (_normalizedPosition.y < normalizedSize.y * 0.5f)
                        _normalizedPosition.y = 0f;

                    // Stick to top and bottom (epsilon = half pickle size)
                    if (StickToCorners)
                    {
                        if (_normalizedPosition.y + normalizedSize.y >= 1f)
                            _normalizedPosition.y = 1f;
                        if (_normalizedPosition.y - normalizedSize.y < 0f)
                            _normalizedPosition.y = 0f;
                    }

                    // Sync back to normalized pos
                    float x = _activeRect.x + _activeRect.width * _normalizedPosition.x;
                    float y = _activeRect.y + _activeRect.height * _normalizedPosition.y;
                    RectTransform.anchoredPosition = BottomLeftPostToAnchoredPos(new Vector2(x, y));

                    DoBoundariesRestriction();
                    break;
                }
            case BoundaryAlignment.RightEdge:
                {
                    RectTransform.anchoredPosition = BottomLeftPostToAnchoredPos(new Vector2(_activeRect.xMax, blPicklePos.y)); // Clamp to right edge
                    blPicklePos = AnchoredPosToBottomLeftPos(RectTransform.anchoredPosition); // Convert to left-bottom coordinate
                    _normalizedPosition.Set(1f, (blPicklePos.y - _activeRect.y) / _activeRect.height); // Normalized position of pickle center on the right edge of active rect

                    // For rotation - check if attached to a corner. Mini-stick to top and bottom
                    if (_normalizedPosition.y + normalizedSize.y * 0.5f >= 1f)
                        _normalizedPosition.y = 1f;
                    if (_normalizedPosition.y < normalizedSize.y * 0.5f)
                        _normalizedPosition.y = 0f;

                    // Stick to top and bottom (epsilon = half pickle size)
                    if (StickToCorners)
                    {
                        if (_normalizedPosition.y + normalizedSize.y >= 1f)
                            _normalizedPosition.y = 1f;
                        if (_normalizedPosition.y - normalizedSize.y < 0f)
                            _normalizedPosition.y = 0f;
                    }

                    // Sync back to normalized pos
                    float x = _activeRect.x + _activeRect.width * _normalizedPosition.x;
                    float y = _activeRect.y + _activeRect.height * _normalizedPosition.y;
                    RectTransform.anchoredPosition = BottomLeftPostToAnchoredPos(new Vector2(x, y));

                    DoBoundariesRestriction();
                    break;
                }
            case BoundaryAlignment.TopEdge:
                {
                    RectTransform.anchoredPosition = BottomLeftPostToAnchoredPos(new Vector2(blPicklePos.x, _activeRect.yMax)); // Clamp to top edge
                    blPicklePos = AnchoredPosToBottomLeftPos(RectTransform.anchoredPosition); // Convert to left-bottom coordinate
                    _normalizedPosition.Set((blPicklePos.x - _activeRect.xMin) / _activeRect.width, 1f); // Normalized position of pickle center on the top edge of active rect

                    // For rotation - check if attached to a corner. Mini-stick to left and right
                    if (_normalizedPosition.x + normalizedSize.x * 0.5f >= 1f)
                        _normalizedPosition.x = 1f;
                    if (_normalizedPosition.x < normalizedSize.x * 0.5f)
                        _normalizedPosition.x = 0f;

                    // Stick to left and right (epsilon = half pickle size)
                    if (StickToCorners)
                    {
                        if (_normalizedPosition.x + normalizedSize.x >= 1f)
                            _normalizedPosition.x = 1f;
                        if (_normalizedPosition.x - normalizedSize.x < 0f)
                            _normalizedPosition.x = 0f;
                    }

                    // Sync back to normalized pos
                    float x = _activeRect.x + _activeRect.width * _normalizedPosition.x;
                    float y = _activeRect.y + _activeRect.height * _normalizedPosition.y;
                    RectTransform.anchoredPosition = BottomLeftPostToAnchoredPos(new Vector2(x, y));

                    DoBoundariesRestriction();
                    break;
                }
            case BoundaryAlignment.BottomEdge:
            {
                RectTransform.anchoredPosition = BottomLeftPostToAnchoredPos(new Vector2(blPicklePos.x, _activeRect.yMin)); // Clamp to bottom edge
                blPicklePos = AnchoredPosToBottomLeftPos(RectTransform.anchoredPosition); // Convert to left-bottom coordinate
                _normalizedPosition.Set((blPicklePos.x - _activeRect.xMin) / _activeRect.width, 0f); // Normalized position of pickle center on the bottom edge of active rect

                // For rotation - check if attached to a corner. Mini-stick to left and right
                if (_normalizedPosition.x + normalizedSize.x * 0.5f >= 1f)
                    _normalizedPosition.x = 1f;
                if (_normalizedPosition.x < normalizedSize.x * 0.5f)
                    _normalizedPosition.x = 0f;

                // Stick to left and right (epsilon = half pickle size)
                if (StickToCorners)
                {
                    if (_normalizedPosition.x + normalizedSize.x >= 1f)
                        _normalizedPosition.x = 1f;
                    if (_normalizedPosition.x - normalizedSize.x < 0f)
                        _normalizedPosition.x = 0f;
                }

                // Sync back to normalized pos
                float x = _activeRect.x + _activeRect.width * _normalizedPosition.x;
                float y = _activeRect.y + _activeRect.height * _normalizedPosition.y;
                RectTransform.anchoredPosition = BottomLeftPostToAnchoredPos(new Vector2(x, y));

                DoBoundariesRestriction();
                break;
            }
        }
    }

    private Vector2 DoBoundariesRestriction()
    {
        Vector2 blPicklePos = AnchoredPosToBottomLeftPos(RectTransform.anchoredPosition);

        if (blPicklePos.x - RectTransform.sizeDelta.x * 0.5f < _activeRect.x)
            blPicklePos.x = _activeRect.xMin + RectTransform.sizeDelta.x * 0.5f;

        if (blPicklePos.x + RectTransform.sizeDelta.x * 0.5f > _activeRect.xMax)
            blPicklePos.x = _activeRect.xMax - RectTransform.sizeDelta.x * 0.5f;

        if (blPicklePos.y + RectTransform.sizeDelta.y * 0.5f > _activeRect.yMax)
            blPicklePos.y = _activeRect.yMax - RectTransform.sizeDelta.y * 0.5f;

        if (blPicklePos.y - RectTransform.sizeDelta.y * 0.5f < _activeRect.yMin)
            blPicklePos.y = _activeRect.yMin + RectTransform.sizeDelta.y * 0.5f;

        RectTransform.anchoredPosition = BottomLeftPostToAnchoredPos(blPicklePos); // convert back to coordinates relative to center
        return blPicklePos;
    }

    Vector2 BottomLeftPostToAnchoredPos(Vector2 BLPos)
    {
        return BLPos - _canvasRect.rect.size * 0.5f;
    }

    Vector2 AnchoredPosToBottomLeftPos(Vector2 anchoredPos)
    {
        return _canvasRect.rect.size * 0.5f + anchoredPos;
    }


    [ContextMenu("Print pickle state")]
    public void DbgPrintState()
    {
        Debug.Log($"Anchored position: {RectTransform.anchoredPosition}");
        Debug.Log($"_normalizedPosition: {_normalizedPosition}");
        Debug.Log($"SizeDelta: {RectTransform.sizeDelta}");
    }
}

