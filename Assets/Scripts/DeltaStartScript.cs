using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DeltaDNA;

public class DeltaStartScript : MonoBehaviour {


    private Score score;
    // public int healthPacksUsed;
    //public int bombsUsed;
    private LayBombs layBombs;
    private PlayerHealth playerHealth;

    // Use this for initialization
    void Start () {

        DDNA.Instance.SetLoggingLevel(DeltaDNA.Logger.Level.DEBUG);

        //Register default handlers for event triggered campaigns. These will be candidates for handling ANY Event-Triggered Campaigns. 
        //Any handlers added to RecordEvent() calls with the .Add method will be evaluated before these default handlers. 
        DDNA.Instance.Settings.DefaultImageMessageHandler =
            new ImageMessageHandler(DDNA.Instance, imageMessage => {
                // the image message is already prepared so it will show instantly
                imageMessage.Show();
            });
        DDNA.Instance.Settings.DefaultGameParameterHandler = new GameParametersHandler(gameParameters => {
            // do something with the game parameters
            myGameParameterHandler(gameParameters);
            DeltaDNA.Logger.LogInfo("Received game parameters from event trigger: " + gameParameters);
        });
        DDNA.Instance.StartSDK();

        MissionStarted();

        playerHealth = GameObject.Find("hero").GetComponent<PlayerHealth>();
        layBombs = GameObject.Find("hero").GetComponent<LayBombs>();
        score = GameObject.Find("Score").GetComponent<Score>();

    }


    public void MissionStarted()
    {
        //Build event Params
        GameEvent myGameEvent = new GameEvent("missionStarted")
            .AddParam("missionName", "Demo Game")
            .AddParam("isTutorial", false);
        DDNA.Instance.RecordEvent(myGameEvent);
    }

	// Update is called once per frame

    public void MissionCompleted()
    {
        GameEvent completeGameEvent = new GameEvent("missionCompleted")
            .AddParam("isTutorial", false)
            .AddParam("missionName", "Demo Game")
            .AddParam("healthPacksUsed", playerHealth.healthPacksUsed)
            .AddParam("bombsUsed", layBombs.bombsUsed)
            .AddParam("userScore", score.score);

        DDNA.Instance.RecordEvent(completeGameEvent);
    }

    public void FeatureUnlocked()
    {
        GameEvent featureUnlocked = new GameEvent("featureUnlocked")
            .AddParam("userScore", score.score)
            .AddParam("featureName", "Max Dealth Down")
            .AddParam("featureType", "Difficulty Increase")
            .AddParam("maxHealthDown", 50);


        DDNA.Instance.RecordEvent(featureUnlocked).Run();
    }

    public void eventScoreHit()
    {
    }

    private void myGameParameterHandler(Dictionary<string, object> gameParameters)
    {
        // Parameters Received      
        Debug.Log("Received game parameters from event trigger: " + DeltaDNA.MiniJSON.Json.Serialize(gameParameters));
    }

    private void myImageMessageHandler(ImageMessage imageMessage)
    {
        // Add a handler for the 'dismiss' action.
        imageMessage.OnDismiss += (ImageMessage.EventArgs obj) => {
            Debug.Log("Image Message dismissed by " + obj.ID);

            // NB : parameters not processed if player dismisses action
        };

        // Add a handler for the 'action' action.
        imageMessage.OnAction += (ImageMessage.EventArgs obj) => {
            Debug.Log("Image Message actioned by " + obj.ID + " with command " + obj.ActionValue);

            // Process parameters on image message if player triggers image message action
            if (imageMessage.Parameters != null) myGameParameterHandler(imageMessage.Parameters);
        };

        // the image message is already cached and prepared so it will show instantly
        imageMessage.Show();
    }

    void Update () {
		
	}
}
