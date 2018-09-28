using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDisplay : MonoBehaviour {
    
	[SerializeField]
	TMPro.TextMeshPro health;
	[SerializeField]
	TMPro.TextMeshPro score;
    
    public void setHealth(string healthText)
    {
        health.text = healthText;
    }

    public void setScore(string scoreText)
    {
        score.text = scoreText;
    }
}
