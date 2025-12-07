/* Author: E. Nathan Lee
 * Date: 12/7/2025
 * Description: A 2D camera controller script that allows for a smooth,
 *              loose camera follow based on dead zones.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //==============
    // Camera Target
    //==============
    [Header("Camera Target")]
    public Transform target;

    //=========================
    // Camera Movement Settings
    //=========================
    [Header("Camera Movement Settings")]
    public float smoothSpeed = 5f;
    public float deadZoneLeft = 2f;
    public float deadZoneRight = 2f;
    public float deadZoneUp = 10f;
    public float deadZoneDown = 1f;
    private float initialZ;
    public int pixelsPerUnit = 64; // General PPU in my sprites/tiles

    void Start()
    {
        initialZ = transform.position.z; // Initial Z for 2D camera
    }

    void LateUpdate()
    {
        if (target == null) return; //If nothing to follow, exit

        // Current positions
        float camX = transform.position.x; 
        float playerX = target.position.x;
        float camY = transform.position.y;
        float playerY = target.position.y;

        // Only move camera when player leaves dead zone
        if (playerX < camX - deadZoneLeft)
        {
            camX = playerX + deadZoneLeft;
        }
        else if (playerX > camX + deadZoneRight)
        {
            camX = playerX - deadZoneRight;
        }

        if (playerY < camY - deadZoneDown)
        {
            camY = playerY + deadZoneDown;
        }
        else if (playerY > camY + deadZoneUp)
        {
            camY = playerY - deadZoneUp;
        }

        // Smooth movement -- applying Lerp
        Vector3 newPos = new Vector3(camX, camY, initialZ);
        newPos = Vector3.Lerp(transform.position, newPos, smoothSpeed * Time.deltaTime);
        float ppu = (float)pixelsPerUnit;
        newPos.x = Mathf.Round(newPos.x * ppu) / ppu;
        newPos.y = Mathf.Round(newPos.y * ppu) / ppu;

        transform.position = newPos;
    }
}
