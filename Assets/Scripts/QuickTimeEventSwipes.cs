using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Gesture { swipeRight, swipeLeft, swipeUp, swipeDown, swipeUpRight, swipeUpLeft, swipeDownRight, swipeDownLeft, None };

public class QuickTimeEventSwipes : MonoBehaviour
{
    [SerializeField] List<Transform> arrows;
    [SerializeField] RectTransform panel;
    [SerializeField] float timeToSwipe = 2;
    [SerializeField] float quicktimeCooldown = 5;
    [Header("Swipe")]
    [SerializeField] private float minSwipeValue = 10;
    [SerializeField] private float marginPercentage = 30;

    //TODO: que solo detecte gestos en un area especifica de la pantalla (googlear)
    Gesture gestureDone;
    Vector2 initialPosition;
    Vector2 endPosition;
    Vector2 swipeDirection;

    NodoSwipe swipe;

    private Vector2 screenSize;
    private int arrowNumber;
    private float actualTimeToSwipe;

    private void Start()
    {
        ///TODO: restringir swipes en el panel
        screenSize = new Vector2(panel.sizeDelta.x, panel.sizeDelta.y);
        Debug.Log(screenSize);

        for (int i = 0; i < arrows.Count; i++)
            arrows[i].gameObject.SetActive(false);

        Invoke(nameof(QuickTimeEvent), quicktimeCooldown);
    }

    private void Update()
    {
        if (actualTimeToSwipe <= 0) return;

        actualTimeToSwipe -= Time.deltaTime;

        if(actualTimeToSwipe <= 0)
        {
            TurnOffArrow();
        }

        if (Application.isMobilePlatform)
        {
            if (Input.touchCount > 0)
            {
                switch (Input.GetTouch(0).phase)
                {
                    case TouchPhase.Began:
                        initialPosition = Input.GetTouch(0).position;
                        break;
                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:

                        endPosition = Input.GetTouch(0).position;
                        break;
                }
            }
        }
        else if (Application.isEditor)
        {
            if (Input.GetMouseButtonDown(0))
            {
                initialPosition = Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(0))
            {
                endPosition = Input.mousePosition;
            }
        }
        //Calculo direccion
        if (initialPosition != Vector2.zero && endPosition != Vector2.zero) swipeDirection = endPosition - initialPosition;

        if (Mathf.Abs(swipeDirection.x) >= minSwipeValue || Mathf.Abs(swipeDirection.y) >= minSwipeValue)
            gestureDone = DetectGestureBasedOnSwipeDirection(swipeDirection);
        else
            gestureDone = Gesture.None;

        if (gestureDone != Gesture.None)
        {
            //TODO: hacer una clase gesture (nodoswipe?) para que la tengan las flechas y se sepa  el gesto de la flecha activa
            swipe = new NodoSwipe(initialPosition, endPosition, gestureDone);
            if (swipe.Gesture == (Gesture)arrowNumber)
            {
                ///TODO: reiniciar valores de gesturedone y swipe
                PlayerController.Instance.ActivePower(swipe.Gesture);
                TurnOffArrow();
            }
        }
    }

    private Gesture DetectGestureBasedOnSwipeDirection(Vector2 swipeDirection)
    {
        //Debug.Log("Margen en X: " + Mathf.Abs(swipeDirection.y) * marginPercentage / 100 + ". Margen en Y: " + Mathf.Abs(swipeDirection.x) * marginPercentage / 100);
        if (swipeDirection.x < 0 && Mathf.Abs(swipeDirection.y) < Mathf.Abs(swipeDirection.x) * marginPercentage / 100)
            return Gesture.swipeLeft;
        if (swipeDirection.x > 0 && Mathf.Abs(swipeDirection.y) < Mathf.Abs(swipeDirection.x) * marginPercentage / 100)
            return Gesture.swipeRight;

        if (swipeDirection.y > 0 && Mathf.Abs(swipeDirection.x) < Mathf.Abs(swipeDirection.y) * marginPercentage / 100)
            return Gesture.swipeUp;
        if (swipeDirection.y < 0 && Mathf.Abs(swipeDirection.x) < Mathf.Abs(swipeDirection.y) * marginPercentage / 100)
            return Gesture.swipeDown;

        if (swipeDirection.x > 0 && swipeDirection.y > 0)
            return Gesture.swipeUpRight;
        if (swipeDirection.x < 0 && swipeDirection.y > 0)
            return Gesture.swipeUpLeft;
        if (swipeDirection.x > 0 && swipeDirection.y < 0)
            return Gesture.swipeDownRight;
        if (swipeDirection.x < 0 && swipeDirection.y < 0)
            return Gesture.swipeDownLeft;

        return Gesture.None;
    }

    private void QuickTimeEvent()
    {
        arrowNumber = Random.Range(0, arrows.Count -1);
        arrows[arrowNumber].gameObject.SetActive(true);
        actualTimeToSwipe = timeToSwipe;
    }

    public void TurnOffArrow()
    {
        CancelInvoke(nameof(QuickTimeEvent));

        actualTimeToSwipe = 0;
        arrows[arrowNumber].gameObject.SetActive(false);

        Invoke(nameof(QuickTimeEvent), quicktimeCooldown);
    }
}
