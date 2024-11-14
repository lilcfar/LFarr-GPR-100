using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMoverScript : MonoBehaviour
{
    public PlayerController playerController;

    public Vector3 moveAmount = new Vector3(-30f, 0f, 0f); // Distance to move the camera
    public float moveDuration = 1f; // Duration for smooth movement

    public Button MoveSceneLeftButton; // Button to move forward
    public Button MoveSceneRightButton;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool isMoving = false;
    private bool isAtStart = true;
    private float elapsedTime = 0f;

    private void Start()
    {
        startPosition = transform.position; // Store the initial position
        targetPosition = startPosition + moveAmount; 

        MoveSceneRightButton.gameObject.SetActive(false); // Hide the back button initially

        MoveSceneLeftButton.onClick.AddListener(MoveCameraForward);
        MoveSceneRightButton.onClick.AddListener(MoveCameraBack);
    }

    private void Update()
    {
        if (isMoving)
        {
            elapsedTime += Time.deltaTime;
            Vector3 currentTarget = isAtStart ? targetPosition : startPosition;
            transform.position = Vector3.Lerp(transform.position, currentTarget, elapsedTime / moveDuration);

            if (elapsedTime >= moveDuration)
            {
                transform.position = currentTarget;
                isMoving = false;
                isAtStart = !isAtStart;
                ToggleButtons();

                // Enable or disable player movement based on camera position
                if (!isAtStart)
                {
                    playerController.EnableMovement(true);
                }
                else
                {
                    playerController.EnableMovement(false);
                }
            }
        }
    }

    private void MoveCameraForward()
    {
        if (!isMoving && isAtStart)
        {
            StartMoving();
        }
    }

    private void MoveCameraBack()
    {
        if (!isMoving && !isAtStart)
        {
            StartMoving();
        }
    }

    private void StartMoving()
    {
        elapsedTime = 0f;
        isMoving = true;
    }

    private void ToggleButtons()
    {
        MoveSceneLeftButton.gameObject.SetActive(isAtStart); // Show forward button if at start
        MoveSceneRightButton.gameObject.SetActive(!isAtStart); // Show back button if at target
    }
}



// before adding player controller stuff just incase 
//public class CameraMoverScript: MonoBehaviour
//{
//    public Vector3 moveAmount = new Vector3(-30f, 0f, 0f); // Distance to move the camera
//    public float moveDuration = 1f; // Duration for smooth movement

//    public Button MoveSceneLeftButton; // Button to move forward
//    public Button MoveSceneRightButton;

//    private Vector3 startPosition;
//    private Vector3 targetPosition;
//    private bool isMoving = false;
//    private bool isAtStart = true;
//    private float elapsedTime = 0f;

//    private void Start()
//    {
//        startPosition = transform.position; // Store the initial position
//        targetPosition = startPosition + moveAmount; // Define the target position

//        MoveSceneRightButton.gameObject.SetActive(false); // Hide the back button initially

//        // Assign button click listeners
//        MoveSceneLeftButton.onClick.AddListener(MoveCameraForward);
//        MoveSceneRightButton.onClick.AddListener(MoveCameraBack);
//    }

//    private void Update()
//    {
//        if (isMoving)
//        {
//            elapsedTime += Time.deltaTime;
//            Vector3 currentTarget = isAtStart ? targetPosition : startPosition;
//            transform.position = Vector3.Lerp(transform.position, currentTarget, elapsedTime / moveDuration);

//            if (elapsedTime >= moveDuration)
//            {
//                transform.position = currentTarget; // Set position exactly at target
//                isMoving = false;
//                isAtStart = !isAtStart; // Toggle position state
//                ToggleButtons(); // Show/hide buttons as needed
//            }
//        }
//    }

//    private void MoveCameraForward()
//    {
//        if (!isMoving && isAtStart)
//        {
//            StartMoving();
//        }
//    }

//    private void MoveCameraBack()
//    {
//        if (!isMoving && !isAtStart)
//        {
//            StartMoving();
//        }
//    }

//    private void StartMoving()
//    {
//        elapsedTime = 0f;
//        isMoving = true;
//    }

//    private void ToggleButtons()
//    {
//        MoveSceneLeftButton.gameObject.SetActive(isAtStart); // Show forward button if at start
//        MoveSceneRightButton.gameObject.SetActive(!isAtStart); // Show back button if at target
//    }
//}
