using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowController {

    private int maxFlow;
    private int currentFlow;

    public delegate void PlayerDead();
    public static event PlayerDead PlayerDeadEvent;

    public FlowController(int maxFlow, int startingFlow) {
        this.maxFlow = maxFlow;
        this.currentFlow = startingFlow;
    }

    public int Flow {
        get { return currentFlow; }
    }

    public void TakeDamage(int damage) {
        if (currentFlow > 100)
        {
            currentFlow = 100;
            return;
        }
        currentFlow = Mathf.Max(currentFlow - damage*20, 0);
        if (IsDead) { PlayerDeadEvent(); }
    }

    public void Heal(int heal) {
        currentFlow = Mathf.Min(currentFlow + heal, maxFlow);
        if (IsDead) { PlayerDeadEvent(); }
    }

    public bool IsDead {
        get { return currentFlow <= 0; }
    }
}
