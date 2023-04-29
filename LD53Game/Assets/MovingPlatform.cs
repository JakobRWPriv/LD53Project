using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public enum Type {
        BackAndForth,
        Looping,
        Elevator
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
        if (movingPlatformType == Type.Elevator) {
            if (currentPoint < platformPoints.Length - 1) {
                if (!smoothMovement)
                    MoveToPointLinear(platformPoints[currentPoint + 1]);
                else
                    MoveToPointTween(platformPoints[currentPoint + 1]);
                currentPoint++;
            }
        }
    }

    public void MoveToPreviousPoint() {
        if (movingPlatformType == Type.Elevator) {
            if (currentPoint > 0) {
                if (!smoothMovement)
                    MoveToPointLinear(platformPoints[currentPoint - 1]);
                else
                    MoveToPointTween(platformPoints[currentPoint - 1]);
                currentPoint--;
            }
        }
    }

    private void Start() {
        targetPoint = platformPoints[0];

        if (movingPlatformType != Type.Elevator && !waitsForPlayerBeforeStarting) {
            MoveToNextPoint();
        }
    }

    private void Update() {
        if (!canMove || smoothMovement)
            return;

        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);
        //transform.position = Vector3.SmoothDamp(transform.position, targetPoint.position, ref velocity, speed * Time.deltaTime);
        

        if (transform.position == targetPoint.position) {
            if (movingPlatformType == Type.Elevator)
                canMove = false;
            else
                MoveToNextPoint();
        }
    }
}
