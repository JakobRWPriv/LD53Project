using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public enum Type {
        BackAndForth,
        Looping,
        LoopTeleport
    };
    public Type movingPlatformType;

    public bool waitsForPlayerBeforeStarting;
    public bool smoothMovement;
    public bool movingBackwards;

    public Transform[] platformPoints;
    public int currentPoint;

    public Transform targetPoint;
    public bool canMove;
    public float speed;
    private Vector3 velocity = Vector3.zero;
    public float startDelay;

    public void MoveToPointLinear(Transform point) {
        targetPoint = point;
        canMove = true;
    }

    public void MoveToPointTween(Transform point) {
        LeanTween.move(gameObject, point, speed).setEase(LeanTweenType.easeInOutSine).setOnComplete(MoveToNextPoint);
    }

    public void TweenFinished() {
    }

    public void MoveToNextPoint() {
        if (movingPlatformType == Type.BackAndForth) {
            if (currentPoint == platformPoints.Length - 1) {
                movingBackwards = true;
            } else if (currentPoint == 0) {
                movingBackwards = false;
            }
            if (!movingBackwards) {
                if (!smoothMovement)
                    MoveToPointLinear(platformPoints[currentPoint + 1]);
                else
                    MoveToPointTween(platformPoints[currentPoint + 1]);
                currentPoint++;
            } else {
                if (!smoothMovement)
                    MoveToPointLinear(platformPoints[currentPoint - 1]);
                else
                    MoveToPointTween(platformPoints[currentPoint - 1]);
                currentPoint--;
            }
        }
        if (movingPlatformType == Type.Looping) {
            if (currentPoint == platformPoints.Length - 1) {
                currentPoint = -1;
            }
            if (!smoothMovement)
                MoveToPointLinear(platformPoints[currentPoint + 1]);
            else
                MoveToPointTween(platformPoints[currentPoint + 1]);
            currentPoint++;
        }
        if (movingPlatformType == Type.LoopTeleport) {
            currentPoint++;
            if (currentPoint == platformPoints.Length - 1) {
                transform.position = platformPoints[1].position;
                currentPoint--;
            }
        }
        
    }
    private void Start() {
        if (waitsForPlayerBeforeStarting) return;
        if (startDelay > 0) {
            canMove = false;
            StartCoroutine(DelayCo(startDelay));
        }
        if (movingPlatformType != Type.LoopTeleport) {
            targetPoint = platformPoints[1];
        } else {
            targetPoint = platformPoints[0];
        }
        if (smoothMovement) {
            currentPoint = 1;
            MoveToPointTween(platformPoints[1]);
        }
    }

    public void StartWhenPlayerLands() {
        waitsForPlayerBeforeStarting = false;
        if (smoothMovement) {
            currentPoint = 1;
            MoveToPointTween(platformPoints[1]);
        }
    }

    IEnumerator DelayCo(float delay) {
        yield return new WaitForSeconds(delay);
        canMove = true;
    }

    private void Update() {
        if (!canMove || smoothMovement)
            return;

        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);
        //transform.position = Vector3.SmoothDamp(transform.position, targetPoint.position, ref velocity, speed * Time.deltaTime);
        

        if (transform.position == targetPoint.position) {
            MoveToNextPoint();
        }
    }
}
