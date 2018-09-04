using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	private HealthController health = new HealthController(1);

    private Vector2 currentPosition;
    private Vector2 nextPosition;

    private int columnLimit;
    private int rowLimit;

    public Vector2[] movementPattern;
    private int movementCycle = 0;

    void Start() {
        //Temporary Hardcoding of movement pattern
        //movementPattern = new Vector2[] { new Vector2(0, -1), new Vector2(0, 0) };

        //UpdateNextPosition();
    }

    private void OnMove(Vector3 newPosition, Quaternion newRotation) {
        //Currently set to teleportation movement
        transform.SetPositionAndRotation(newPosition, newRotation);
        OnUpdateCurrentPosition();
    }

    private void OnUpdateCurrentPosition() {
        currentPosition = nextPosition;
    }

    private void OnUpdateNextPosition() {
        nextPosition[0] += movementPattern[movementCycle][0] % columnLimit;
        nextPosition[1] += movementPattern[movementCycle][1];

        movementCycle = (movementCycle + 1) % movementPattern.Length;
    }

    /**
     * Public API
     **/

    public void UpdateNextPosition() {
        OnUpdateNextPosition();
    }

    public void MoveAndUpdateNextPosition(Vector3 newPosition, Quaternion newRotation) {
        OnMove(newPosition, newRotation);
        OnUpdateNextPosition();
	}

    public Vector2 GetCurrentPosition() {
        return currentPosition;
    }

    public Vector2 GetNextPosition() {
        return nextPosition;
    }

	public void SetStartingPosition(int column, int row) {
		currentPosition[0] = column;
		currentPosition[1] = row;
		nextPosition [0] = column;
		nextPosition [1] = row;
	}

    public void SetGridLimits(int column, int row) {
        columnLimit = column;
        rowLimit = row;
    }

	public void TakeDamage(int dmg)
	{
		health.TakeDamage (dmg);
	}

	public bool IsDead
	{
		get{ return health.IsDead; }
	}

	public int Health
	{
		get{ return health.Health; }
	}
}
