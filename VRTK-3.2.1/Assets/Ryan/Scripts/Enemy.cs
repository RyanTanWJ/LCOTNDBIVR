using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	private int health;

    private Vector2 currentPosition;
    private Vector2 nextPosition;

    //private int columnLimit;
    private int rowLimit;

    private Vector2[] movementPattern;
    private int movementCycle = 0;

    [SerializeField]
    private int movementCycleLength;

    private void Start() {
        UpdateNextPosition();
    }

    private void OnMove() {
        //TODO: Movment Code
        // Do fancy stuff here wooooo!

        OnUpdateCurrentPosition();
    }

    private void OnUpdateCurrentPosition() {
        currentPosition = nextPosition;
    }

    private void OnUpdateNextPosition() {
        nextPosition[0] += movementPattern[movementCycle][0];
        nextPosition[1] += movementPattern[movementCycle][1] % rowLimit;

        movementCycle = movementCycle + 1 % movementCycleLength;
    }

    /**
     * Public API
     **/

    public void UpdateNextPosition() {
        OnUpdateNextPosition();
    }

    public void MoveAndUpdateNextPosition() {
        OnMove();
        OnUpdateNextPosition();
	}

    public Vector2 GetCurrentPosition() {
        return currentPosition;
    }

    public Vector2 GetNextPosition() {
        return nextPosition;
    }

}
