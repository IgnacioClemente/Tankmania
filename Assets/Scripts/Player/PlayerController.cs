using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Gesture { swipeRight, swipeLeft, swipeUp, swipeDown, swipeUpRight, swipeUpLeft, swipeDownRight, swipeDownLeft, None };

[RequireComponent(typeof(Health), typeof(Movement), typeof(Attack))]

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    [Header("UI")]
    [SerializeField] Joystick movementJoystick;
    [SerializeField] Joystick aimJoystick;
    [SerializeField] Button fireButton;
    [SerializeField] Image fireImage;
    [Header("Swipe")]
    [SerializeField] private float minSwipeValue = 10;
    [SerializeField] private float marginPercentage = 30;
    [Header("PowerUps")]
    [SerializeField] private float turboSpeedMultiplier;
    [SerializeField] private float turboCooldown;
    [SerializeField] private float turboDuration;
    [SerializeField] RectTransform panel;

    Gesture firstSwipe = Gesture.None;
    Gesture previous;

    //TODO: que solo detecte gestos en un area especifica de la pantalla (googlear)
    Gesture gestureDone;

    Vector2 ScreenSize;
    Vector2 initialPosition;
    Vector2 endPosition;
    Vector2 swipeDirection;

    NodoSwipe firstSwipeNodo;
    NodoSwipe secondSwipeNodo;

    private Movement movement;
    private Attack attack;
    private Health health;
    private bool canPowerUp = true;
    private bool isAlive = true;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);

        Instance = this;

        movement = GetComponent<Movement>();
        attack = GetComponent<Attack>();
        health = GetComponent<Health>();
        fireButton.onClick.AddListener(attack.Shoot);
    }

    void Start()
    {
        ScreenSize = new Vector2(Screen.width, Screen.height);
    }

    private void Update()
    {
        if (!isAlive) return;

        fireImage.fillAmount = attack.RemainingCooldown / attack.AttackSpeed;
        movement.Move(movementJoystick.Direction);
        attack.Aim(aimJoystick.Direction, this);

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
                        //SwipeCombination(initialPosition, endPosition);
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

        //Debug.Log ("Init: " + initialPosition);
        //Debug.Log ("End: " + endPosition);
        //Debug.Log ("Dir: " + swipeDirection);

        if (Mathf.Abs(swipeDirection.x) >= minSwipeValue || Mathf.Abs(swipeDirection.y) >= minSwipeValue)
            gestureDone = DetectGestureBasedOnSwipeDirection(swipeDirection);
        else
            gestureDone = Gesture.None;

        if(canPowerUp && gestureDone == Gesture.swipeUp) //do the turbo
        {
            movement.IncreaseSpeed(turboSpeedMultiplier, turboDuration);
            canPowerUp = false;
            Invoke(nameof(ResetPowerUp), turboCooldown);
        }

        if (gestureDone != Gesture.None)
        {
            CancelInvoke("CleanNodo");

            //Debug.Log ("Se hizo un " + gestureDone.ToString () + " con direccion " + swipeDirection.ToString ());

            //Si el first es null, es el primer swipe del usuario
            if (firstSwipeNodo == null) firstSwipeNodo = new NodoSwipe(initialPosition, endPosition, gestureDone);
            else if (secondSwipeNodo == null) secondSwipeNodo = new NodoSwipe(initialPosition, endPosition, gestureDone);

            CleanDirections();

            Invoke("CleanNodo", 2f);
        }
    }

    private void ResetPowerUp()
    {
        canPowerUp = true;
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

    void CleanNodo()
    {
        gestureDone = Gesture.None;
        firstSwipeNodo = null;
        secondSwipeNodo = null;
    }

    void CleanDirections()
    {
        initialPosition = Vector2.zero;
        endPosition = Vector2.zero;
        swipeDirection = Vector2.zero;
    }

    public void KillPlayer()
    {
        isAlive = false;
        GameManager.Instance.EndGame();
    }
}
