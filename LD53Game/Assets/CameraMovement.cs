using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public PlayerController player;
    float posX;
    float posY;
    Vector2 velocity;
    public float yOffset;

    public float minXClamp, maxXClamp;
    public float minYClamp, maxYClamp;
    
    void FixedUpdate() {
        float playerXOffset = 0.1f;

        if (player.isFacingRight) {
            playerXOffset = 0.1f;
        } else {
            playerXOffset = -0.1f;
        }

        posX = Mathf.SmoothDamp(transform.position.x, player.transform.position.x, ref velocity.x, 0.3f);
        posY = Mathf.SmoothDamp(transform.position.y, player.transform.position.y, ref velocity.y, 0.3f);

        transform.position = new Vector3(posX + playerXOffset, posY + yOffset, -10);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minXClamp, maxXClamp), Mathf.Clamp(transform.position.y, minYClamp, maxYClamp), -10);
    }
}