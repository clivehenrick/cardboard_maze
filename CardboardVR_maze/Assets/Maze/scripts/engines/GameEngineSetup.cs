using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace com.henrick.games.maze {
	
	public class GameEngineSetup : MonoBehaviour {
		
		private GameObject referenceClock;
		private ClockManager referenceClockManager;
		private bool gameIsActive = false;
		private float restartTimer;
		private List<GameObject> nightmaresMonsters = new List<GameObject>();
		private SoundManager soundManager;
		private bool resetTimerActive = false;
		private PlayerHealth playerHealth;

		/** SET IN THE INSPECTOR */
		
		/**
         * Set the amount of time for the game to run in Seconds
         */
		
		public int gamelengthSeconds = 120;
		public GameObject startScreen;
		public float restartDelay = 5f;
		public GameObject winScreen;
		public GameObject loseScreen;
		public GameObject endCreditsCanvas;

		
		/** GETTERS AND SETTERS */
		public bool isGameActive {
			get { return gameIsActive; }
		}
		
		public void addNightmare( GameObject newNightmare ) {
			nightmaresMonsters.Add(newNightmare);
		}

		/**TODO NO IDEA IF THIS IS NEEDED */

		public void subtrackNightmare( GameObject oldNightmare) {
			foreach(GameObject nightmare in nightmaresMonsters) {
				if ( oldNightmare == nightmare ) {
					nightmaresMonsters.Remove (nightmare);
					Destroy (nightmare, 2f);
				}
			}
		}

		/** PUBLIC FUNCTIONS */
		
		// * THIS IS THE INIT SCRIPT FOR THE ENGINE */

		void Start () {
			referenceClock = GameObject.Find ("DigitalClock");

			GameObject kidsvoice = GameObject.Find("kidsVoice");
			soundManager = kidsvoice.GetComponent<SoundManager>();

			GameObject bedTarget = GameObject.Find ("BedTarget");
			playerHealth = bedTarget.GetComponent<PlayerHealth>();

			if (referenceClock != null) {
				referenceClockManager = referenceClock.GetComponent<ClockManager>();

			}

			cleanUpEndingCanvas();
			resetGameState();
		}

		void Update() {
			if (resetTimerActive) {
				restartTimer += Time.deltaTime;
				
				if (restartTimer >= restartDelay) {
					restartTimer = 0f;
					resetTimerActive = false;
					cleanUpScene();
					resetGameState();
				}
			}
		}

		public int getCurrentGameTimeLeftInSeconds() {
			int currentMins = 0;
			int currentSeconds = 0;
			
			if (referenceClockManager) {
				currentMins = referenceClockManager.Minutes;
				currentSeconds = referenceClockManager.Seconds;
			}
			
			return currentMins * 60 + currentSeconds;
		}
		
		/**
         * The Player wins the game and the 2 minitues pass without him being killed
         */
		
		public void playerWins() {
			gameIsActive = false;
			
			winScreen.SetActive(true);
			loseScreen.SetActive(false);
			
			//TODO ADD LOGIC
			//TODO PLAY AUDIO FILES
			//TODO TRANSITION

			cleanUpAllNightmares();
			resetTimerActive = true;
		}
		
		/**
         * The Player ( Bed Health goes to 0 )
         */
		
		public void playerLoses() {
			gameIsActive = false;
			
			//TODO ADD LOGIC

			//TODO TRANSITION

			soundManager.playloseKidVoice();

			winScreen.SetActive(false);
			loseScreen.SetActive(true);

			cleanUpAllNightmares();
			resetTimerActive = true;
			
		}

		/**
         * We call this at the start of a new game, after the start game button is called
         */
		
		public void startNewGame() {
			referenceClockManager.UseSystemTime = false;
			List<int> returnList = convertTimeForClock(gamelengthSeconds);
			referenceClockManager.SetCurrentTime(00,returnList[0],returnList[1]);
			gameIsActive = true;
			resetTimerActive = false;

			startScreen.SetActive(false);
			soundManager.playGameStartingKidVoice();
			soundManager.playGameBackgroundLoop();
		}
		
		/* We call this when we want to reset everything to the pause state at the start of the game */
		
		private void resetGameState() {
			gameIsActive = false;
			referenceClockManager.UseSystemTime = true;
			startScreen.SetActive(true);
			playerHealth.resetHealth ();
			soundManager.playPreGameBackgroundLoop();

			//TODO Move the Camera back to the set location for the GuiScreen

		}
		
		/** This is the cleanUp to kill all the Nightmares and Clean Up the Game */
		
		private void cleanUpScene() {
			//TODO We have a lot to do here!!
			//Application.LoadLevel (Application.loadedLevel); //This was what it used to do!
			
			
			cleanUpEndingCanvas();
		}
		
		private List<int> convertTimeForClock(int lengthSeconds) {
			int totalMinitues = 0; 
			int totalSeconds = lengthSeconds;
			
			totalMinitues = totalSeconds/60;
			totalSeconds = totalSeconds % 60;
			
			List<int> returnList = new List<int>();
			returnList.Add(totalMinitues);
			returnList.Add(totalSeconds);
			return returnList;
		}
		
		private void cleanUpEndingCanvas() {
			endCreditsCanvas.SetActive(true);
			winScreen.SetActive(false);
			loseScreen.SetActive(false);
		}

		private void cleanUpAllNightmares() {
			foreach ( GameObject nightmare in nightmaresMonsters ) {
				if (nightmare != null) { Destroy (nightmare,0); }
			}
		}
		
	}
}