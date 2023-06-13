using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DragCharacter : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    [SerializeField] private Camera mainCamera;
    //public Camera mainCamera;
    private void OnMouseDown()
    {
        //gameUIManager.gameUiInstance.isStart true -> gameStop , false -> game Doing..
        if (gameUIManager.gameUiInstance.isStart) {
            isDragging = true;
            offset = transform.position - GetMouseWorldPosition();
        }
    }
    private void OnMouseUp()
    {
        if (gameUIManager.gameUiInstance.isStart) {        
            isDragging = false;
            // Check if the character is dropped inside a box
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.1f);
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Box"))
                {
                    // Character is dropped inside a box
                    SnapToBox(collider.gameObject);
                    break;
                }
            }
        }
    }
    private void OnMouseDrag()
    {
        if (isDragging)
        {
            transform.position = GetMouseWorldPosition() + offset;
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 0;
        return mainCamera.ScreenToWorldPoint(mousePosition);
    }
    private void SnapToBox(GameObject box)
    {
        transform.position = box.transform.position;
    }    
}
