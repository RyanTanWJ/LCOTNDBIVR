using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class serves to represent the CHANGE from the current position to the next position.
 */
public class MovementPattern : MonoBehaviour{

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
        if (beat != 4)
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
}
