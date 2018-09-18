using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowController : MonoBehaviour {

    private int maxFlow;
    private int currentFlow;
    
    public FlowController(int maxFlow, int startingFlow) {
        this.maxFlow = maxFlow;
        this.currentFlow = startingFlow;
    }

    public int Flow {
        get { return currentFlow; }
    }

    public void TakeDamage(int damage) {
        currentFlow = Mathf.Max(currentFlow - damage, 0);
    }

    public void Heal(int heal) {
        currentFlow = Mathf.Min(currentFlow + heal, maxFlow);
    }

    public bool IsDead {
        get { return currentFlow <= 0; }
    }
}
