﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	private HealthController health = new HealthController(1);

    private Vector2 currentPosition;
    private Vector2 nextPosition;

    private int columnLimit;
    private int rowLimit;

	public int score = 100;

	private static float MoveTime;
	private static float InverseMoveTime;

    public int movementPattern;

    private AudioSource deathAudio;

    [SerializeField]
    private GameObject deathFXPrefab;

    [SerializeField]
    public float speed;

    void Start() {
        //Temporary Hardcoding of movement pattern
        //movementPattern = new Vector2[] { new Vector2(0, -1), new Vector2(0, 0) };

        deathAudio = GetComponent<AudioSource>();

		if (MoveTime <= 0)
		{
			MoveTime = GameObject.FindGameObjectWithTag ("RhythmController").GetComponent<RhythmController> ().GetSecondsPerBeat();
			InverseMoveTime = 1f / MoveTime;
		}
    }

    private void OnMove(Vector3 newPosition, Quaternion newRotation) {
        //Currently set to teleportation movement
        //transform.SetPositionAndRotation(newPosition, newRotation);
		StartCoroutine(SmoothMovement(newPosition));
		StartCoroutine (SmoothRotation (newRotation, MoveTime));
		OnUpdateCurrentPosition();
    }

    private void OnUpdateCurrentPosition() {
        currentPosition = nextPosition;
    }

    private void OnUpdateNextPosition(Vector2Int posChange) {
		nextPosition[0] = (((currentPosition[0] + posChange[0]) % columnLimit)  + columnLimit) % columnLimit; //in case of negative numbers
		nextPosition[1] = currentPosition[1] + posChange[1];
    }

    private void OnUpdateNextPositionRandom(Vector2Int posChange)
    {
        nextPosition[0] = (((currentPosition[0] + posChange[0]) % columnLimit) + columnLimit) % columnLimit; //in case of negative numbers
        nextPosition[1] = currentPosition[1] + posChange[1];
    }

    //Co-routine for moving units from one space to next, takes a parameter end to specify where to move to.
    IEnumerator SmoothMovement (Vector3 endPos)
	{
		//Calculate the remaining distance to move based on the square magnitude of the difference between current position and end parameter. 
		//Square magnitude is used instead of magnitude because it's computationally cheaper.
		float sqrRemainingDistance = (transform.position - endPos).sqrMagnitude;

		//While that distance is greater than a very small amount (Epsilon, almost zero):
		while(sqrRemainingDistance > float.Epsilon)
		{
			//Find a new position proportionally closer to the end, based on the moveTime
			Vector3 newPostion = Vector3.MoveTowards(transform.position, endPos, InverseMoveTime * Time.deltaTime * 8.0f);

			//Set the current transform's position to the new position
			transform.position = newPostion;

			//Recalculate the remaining distance after moving.
			sqrRemainingDistance = (transform.position - endPos).sqrMagnitude;

			//Return and loop until sqrRemainingDistance is close enough to zero to end the function
			yield return null;
		}
        yield return null;
	}

	public IEnumerator SmoothRotation (Quaternion endRot, float time)
	{
		float elapsedTime = 0.0f;

		while (elapsedTime < time) {
			//Rotations
			transform.rotation = Quaternion.Slerp(transform.rotation, endRot,  (elapsedTime / time )  );

			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame ();
		}
		yield return null;
	}

    /**
    * Public API
    **/

    public void UpdateNextPosition(List<Vector2Int> pattern, int beat, bool randomPattern) {
        if (randomPattern)
        {
            OnUpdateNextPositionRandom(pattern[Random.Range(0, pattern.Count)]);
        }
        else
        {
            // TODO here I need to either use many different pattern (for each different starts)
            // Or I make sure that the selection of the pattern is independant of the beat and comes from within the enemy
            // IDEA use offset here inside enemy at creation to target a specific moment in pattern
            OnUpdateNextPosition(pattern[beat%4]);  
        }
    }

    public void MoveAndUpdateNextPosition(Vector3 newPosition, Quaternion newRotation, List<Vector2Int> pattern, int beat, bool randomPattern) {
        OnMove(newPosition, newRotation);
        if (randomPattern)
        {
            OnUpdateNextPositionRandom(pattern[Random.Range(0, pattern.Count)]);
        }
        else
        {
            OnUpdateNextPosition(pattern[beat % 4]);
        }

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

    public GameObject GetDeathFX()
    {
        return deathFXPrefab;
    }
}
