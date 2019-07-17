using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour
{
	public int score = 0;					// The player's score.
    private bool dataSent = false;

	private PlayerControl playerControl;	// Reference to the player control script.
	private int previousScore = 0;          // The score in the previous frame.
    private DeltaStartScript deltaDNA;

    void Awake ()
	{
		// Setting up the reference.
		playerControl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        deltaDNA = GameObject.Find("DeltaDNA").GetComponent<DeltaStartScript>();
    }


	void Update ()
	{
		// Set the score text.
		GetComponent<GUIText>().text = "Score: " + score;

		// If the score has changed...
		if(previousScore != score)
			// ... play a taunt.
			playerControl.StartCoroutine(playerControl.Taunt());

		// Set the previous score to this frame's score.
		previousScore = score;

        if (score == 2000 && dataSent != true)
        {
            
            deltaDNA.FeatureUnlocked();
            dataSent = true;
        }
	}

}
