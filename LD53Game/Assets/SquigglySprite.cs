using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquigglySprite : MonoBehaviour
{
    public float originalSizeX;
    public float originalSizeY;
    public int flipFactor;
    public float multFactorX = 1.05f;
    public float multFactorY = 1.05f;

    public float timeToReach;

    void Start() {
        flipFactor = 1;

        originalSizeX = transform.localScale.x;
        originalSizeY = transform.localScale.y;

        timeToReach = Time.time + 0.5f;
    }

    void Update() {
        if (Time.time > timeToReach) {
            transform.localScale = new Vector2((originalSizeX) * (Random.Range(1f, multFactorX)), (originalSizeY) * (Random.Range(1f, multFactorY)));

            timeToReach = Time.time + 0.1f;
            flipFactor = -flipFactor;
        }
    }
}
