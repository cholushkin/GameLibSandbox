using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class PicklesLayout : MonoBehaviour
{
    public List<PickleBase> Pickles = new List<PickleBase>();
    public float boundaryPadding = 10f;
    public bool UseSafeArea;
    public Canvas Canvas;
    private Rect _activeArea;

    void Awake()
    {
	    _activeArea = CalculateActiveRect();
		var canvasRect = Canvas.GetComponent<RectTransform>();
		foreach (var pickle in Pickles)
		{
			pickle.Init(canvasRect, UseSafeArea);
		}

		FirstRound();

    }

    // On the first round we decide locations for each pickle based on their settings 
    private void FirstRound()
    {
	    foreach (var pickle in Pickles)
	    {
		    var pos = pickle.CalculateTargetPosition(_activeArea);
		    pickle.RectTransform.anchoredPosition = pos;

	    }

    }

    // On the second round we use "physics" to push them so they can take right position

    // On the third round we tween pickles to destination based on their distance to target from the round 1

    // we do position recalculation any of the following cases:
    // 1. user drag'n'droped any pickle
    // 2. canvas resized or scaled
    // 3. screen changed orientation 


    private void Update2()
    {
        for (int i = 0; i < Pickles.Count; i++)
        {
            for (int j = i + 1; j < Pickles.Count; j++)
            {
                if (RectsIntersect(Pickles[i].RectTransform, Pickles[j].RectTransform))
                {
                    //print($"colllide {Pickles[i].RectTransform.rect} {Pickles[j].RectTransform.rect}");
                    //Vector2 newDelta = CalculateSeparationDelta(rectTransforms[i], rectTransforms[j]);
                    //Vector2 newPos = rectTransforms[i].anchoredPosition + newDelta;

                    //// Check boundaries using canvas bounds
                    //Rect canvasBounds = GetCanvasBounds();
                    //newPos.x = Mathf.Clamp(newPos.x, canvasBounds.xMin + boundaryPadding, canvasBounds.xMax - boundaryPadding);
                    //newPos.y = Mathf.Clamp(newPos.y, canvasBounds.yMin + boundaryPadding, canvasBounds.yMax - boundaryPadding);

                    //rectTransforms[i].anchoredPosition = newPos;
                }
            }
        }

        foreach (var pickle in Pickles)
            pickle.UpdatePosition();
    }

   

    private bool RectsIntersect(RectTransform rect1, RectTransform rect2)
    {
        Rect r1 = rect1.rect;
        Rect r2 = rect2.rect;
        return r1.Overlaps(r2);
        //return r1.xMin < r2.xMax && r1.xMax > r2.xMin && r1.yMin < r2.yMax && r1.yMax > r2.yMin;
    }

    private Vector2 CalculateSeparationDelta(RectTransform rect1, RectTransform rect2)
    {
	    Rect r1 = rect1.rect;
        Rect r2 = rect2.rect;

        float xSeparation = Mathf.Max(r1.xMax - r2.xMin, r2.xMax - r1.xMin);
        float ySeparation = Mathf.Max(r1.yMax - r2.yMin, r2.yMax - r1.yMin);

        Vector2 newDelta = new Vector2(xSeparation, ySeparation);
        return newDelta;
    }

    private Rect CalculateActiveRect()
    {
	    Assert.IsNotNull(Canvas);
	    var canvasRect = Canvas.GetComponent<RectTransform>();
		Vector2 canvasRawSize = canvasRect.rect.size;

		// Calculate safe area bounds
		float canvasWidth = canvasRawSize.x;
		float canvasHeight = canvasRawSize.y;

		float canvasBottomLeftX = 0f;
		float canvasBottomLeftY = 0f;

		if (UseSafeArea)
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
		return new Rect(canvasBottomLeftX, canvasBottomLeftY, canvasWidth, canvasHeight);
    }
}