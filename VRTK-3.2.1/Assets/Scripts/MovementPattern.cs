using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class serves to keep track of the change in position of an enemy on the beat
 */
public class MovementPattern : MonoBehaviour{

    private List<List<Vector2Int>> fixedPatterns;

    private List<List<Vector2Int>> randomPatterns;

    private static Vector2Int noMove = new Vector2Int(0, 0);
    private static Vector2Int forward = new Vector2Int(0, -1);
    private static Vector2Int left = new Vector2Int(-1, 0);
    private static Vector2Int right = new Vector2Int(1, 0);
    private static Vector2Int diagLeft = new Vector2Int(-1, -1);
    private static Vector2Int diagRight = new Vector2Int(1, -1);

    void Start()
    {
        fixedPatterns = new List<List<Vector2Int>>();
        randomPatterns = new List<List<Vector2Int>>();
        populatePatterns();
    }

    private void populatePatterns()
    {
        //Populate Fixed Patterns
        fixedPatterns.Add(noMovement());
        fixedPatterns.Add(leftRightMovement());
        fixedPatterns.Add(simpleForwardMovement());
        fixedPatterns.Add(intermediateForwardMovement());
        fixedPatterns.Add(fastForwardMovement());
        fixedPatterns.Add(snakingMovement());
        fixedPatterns.Add(zigZagMovement());
        //Populate Random Patterns
        randomPatterns.Add(tMovement());
        randomPatterns.Add(diagonalMovement());
    }

    private List<Vector2Int> noMovement()
    {
        List<Vector2Int> pattern = new List<Vector2Int>(4);
        while (pattern.Count < pattern.Capacity)
        {
            pattern.Add(noMove);
        }
        return pattern;
    }

    private List<Vector2Int> leftRightMovement()
    {
        List<Vector2Int> pattern = new List<Vector2Int>(4);
        pattern.Add(left);
        pattern.Add(noMove);
        pattern.Add(right);
        pattern.Add(noMove);
        return pattern;
    }

    private List<Vector2Int> simpleForwardMovement()
    {
        List<Vector2Int> pattern = new List<Vector2Int>(4);
        pattern.Add(forward);
        while (pattern.Count < pattern.Capacity)
        {
            pattern.Add(noMove);
        }
        return pattern;
    }

    private List<Vector2Int> intermediateForwardMovement()
    {
        List<Vector2Int> pattern = new List<Vector2Int>(4);
        pattern.Add(forward);
        pattern.Add(noMove);
        pattern.Add(forward);
        pattern.Add(noMove);
        return pattern;
    }

    private List<Vector2Int> fastForwardMovement()
    {
        List<Vector2Int> pattern = new List<Vector2Int>(4);
        pattern.Add(forward);
        pattern.Add(forward);
        pattern.Add(forward);
        pattern.Add(noMove);
        return pattern;
    }

    private List<Vector2Int> snakingMovement()
    {
        List<Vector2Int> pattern = new List<Vector2Int>(4);
        pattern.Add(left);
        pattern.Add(forward);
        pattern.Add(right);
        pattern.Add(forward);
        return pattern;
    }

    private List<Vector2Int> zigZagMovement()
    {
        List<Vector2Int> pattern = new List<Vector2Int>(4);
        pattern.Add(diagLeft);
        pattern.Add(noMove);
        pattern.Add(diagRight);
        pattern.Add(noMove);
        return pattern;
    }

    private List<Vector2Int> tMovement()
    {
        List<Vector2Int> pattern = new List<Vector2Int>();
        pattern.Add(left);
        pattern.Add(diagLeft);
        pattern.Add(right);
        pattern.Add(diagRight);
        return pattern;
    }

    private List<Vector2Int> diagonalMovement()
    {
        List<Vector2Int> pattern = new List<Vector2Int>();
        pattern.Add(diagLeft);
        //pattern.Add(noMove);
        pattern.Add(diagRight);
        return pattern;
    }

    public List<Vector2Int> RetrievePattern(int patternNumber, out bool rand)
    {
        if (patternNumber >= fixedPatterns.Count)
        {
            rand = true;
            return randomPatterns[patternNumber%fixedPatterns.Count];
        }
        else
        {
            rand = false;
            return fixedPatterns[patternNumber];
        }
    }
    /*
    [SerializeField]
    int pattern;

	int beats = 4;

    [SerializeField]
    List<int> MoveCycle = new List<int>(4);

    Vector2 posChange;

    //Bools for use in Snaking and Zig-Zag
    bool horizontal = false;
    bool left = false;

    void assignPosChange(int beat)
    {
        posChange = new Vector2(0,0);
        switch (pattern)
        {
            case 0:
                //Simple Forward Movement
                SimpleForwardMovement(beat);
                break;
            case 1:
                //Intermediate Forward Movement
                IntermediateForwardMovement(beat);
                break;
            case 2:
                //Quick Forward Movement
                QuickForwardMovement(beat);
                break;
            case 3:
                //Snaking
                Snaking(beat);
                break;
            case 4:
                //Zig-Zag Movement
                ZigZag(beat);
                break;
            case 5:
                //Diagonal Movement
                DiagonalMovement(beat);
                break;
            case 6:
                //T-Movement
                TMovement(beat);
                break;
			default:
				break;
        }
    }

    private void TMovement(int beat)
    {
        System.Random rand = new System.Random();
        double d = rand.NextDouble();
        if (d >= 0.66)
        {
            posChange[0] = 1;
        }
        else if (d >= 0.33)
        {
            posChange[0] = -1;
        }
        else
        {
            posChange[1] = -1;
        }
    }

    private void DiagonalMovement(int beat)
    {
        if (beat == 1 || beat == 3)
        {
            System.Random rand = new System.Random();
            if (rand.NextDouble() >= 0.5)
            {
                posChange[0] = 1;
            }
            else
            {
                posChange[0] = -1;
            }
            posChange[1] = -1;
        }
    }

    void ZigZag(int beat)
    {
        if (beat == 1 || beat == 3)
        {
            if (left)
            {
                posChange[0] = -1;
            }
            else
            {
                posChange[0] = 1;
            }
			left = !left;
            posChange[1] = -1;
        }
    }

    void Snaking(int beat)
    {
        if (beat == 1 || beat == 3)
        {
            if (horizontal)
            {
                if (left)
                {
                    posChange[0] = -1;
                }
                else
                {
                    posChange[0] = 1;
                }
                //Change Horizontal Direction
                left = !left;
            }
            else
            {
                posChange[1] = -1;
            }
            //Change Axis Direction
            horizontal = !horizontal;
        }
    }

    void SimpleForwardMovement(int beat)
    {
        if (beat == 1)
        {
            posChange[1] = -1;
        }
    }

    void IntermediateForwardMovement(int beat)
    {
        if (beat == 1 || beat == 3)
        {
            posChange[1] = -1;
        }
    }

    void QuickForwardMovement(int beat)
    {
        if (beat != 0)
        {
            posChange[1] = -1;
        }
    }

    public Vector2 PositionChange(int beatNum)
    {
        assignPosChange(beatNum);
        return posChange;
    }

	public int Beats
	{
		get { return beats; }
	}
    */
}
