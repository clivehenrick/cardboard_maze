using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace com.henrick.games.maze {
	/** This is for the KidsVoice Primary */

	public class SoundEngineManager : MonoBehaviour {
	
		private AudioSource[] kidsVoices;
		private List<AudioSource> deathSounds = new List<AudioSource>();
	
		private float timer = 0.0f;           //for counting seconds
		private float frameTime = 1.0f / 15.0f;        //length of a frame at 15fps
		private float fadeTime = 5;           
		private AudioSource preGameBackgroundLoop;
		private AudioSource gameBackgroundLoop;
		private bool fadeIsActive = false;
		private bool playMainGameLoop = false;
		private float volume = 1.0f;

		public void playEnemyDeathKidsVoice() {
			int seednum = Random.Range(0,2);
			AudioSource snd = deathSounds[seednum];
			snd.Play();
		}

		public void playloseKidVoice() {
			foreach (AudioSource snd in kidsVoices) {
				if (snd.clip.name == "MommyDaddy_Editted") {
					snd.Play();
					break;
				}
			}
		}

		public void playGameStartingKidVoice() {
			foreach (AudioSource snd in kidsVoices) {
				if (snd.clip.name == "WhatIsThat_Editted") {
					snd.Play();
					break;
				}
			}
		}

		public void playPreGameBackgroundLoop() {
			playMainGameLoop = false;
			/*
			foreach (AudioSource snd in kidsVoices) {
				if (snd.clip.name == "forgotten-toys") {
					snd.Play();
					break;
				} else {
					if (snd.isPlaying) { snd.Stop(); }
				}
			}
			*/
		}

		public void playGameBackgroundLoop() {
			playMainGameLoop = true;
			gameBackgroundLoop.Play();
			/*
			foreach (AudioSource snd in kidsVoices) {
				if (snd.clip.name == "backgroundMusic") {
					snd.Play();
					break;
				} else {
					if (snd.isPlaying) { snd.Stop(); }
				}
			}
			*/
		}


		void Start () {
			kidsVoices = GetComponents<AudioSource>();

			foreach (AudioSource snd in kidsVoices) {
				if (snd.clip.name == "IDoNotBelieveInYou" || snd.clip.name == "YouDoNotExist" || snd.clip.name == "YourNotReal_Editted") {
					if (snd != null ) { deathSounds.Add(snd); }
				} else if ( snd.clip.name == "backgroundMusic" ) {
					gameBackgroundLoop = snd;
					gameBackgroundLoop.volume = 0.0f;
				} else if ( snd.clip.name == "preGameMusic" ) {
					preGameBackgroundLoop = snd;
					preGameBackgroundLoop.volume = volume;
				}
			}

			preGameBackgroundLoop.Play();
			//gameBackgroundLoop.Play();
			timer = 0.0f;

			InvokeRepeating("audioFade", 1.0f, 1.0f / 15.0f);
		}


		private void audioFade() {

			//if (fadeIsActive){

				if (playMainGameLoop) 
				{

					if (timer > 0.0f)
					{//timer is used to crossfade the two audio sources
						timer -= frameTime;
						if (timer <= 0.0f)
						{
							preGameBackgroundLoop.volume = volume;
							gameBackgroundLoop.volume = 0.0f;
							timer = 0.0f;
						}
					}
				}
				else 
				{//we are not safe
					if (timer < fadeTime)
					{//timer is used to crossfade the two audio sources
						timer += frameTime;
						if (timer >= fadeTime)
						{
							gameBackgroundLoop.volume = volume;
							preGameBackgroundLoop.volume = 0.0f;
							timer = fadeTime;
						}
					}
				}

				if (timer != (!playMainGameLoop ? 0.0f : fadeTime))
				{//we're crossfading
					float crossfade = timer / fadeTime;
					preGameBackgroundLoop.volume = crossfade * volume;
					gameBackgroundLoop.volume = (1.0f - crossfade) * volume;
				}


			//}
		}

	}
}
