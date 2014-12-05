using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Dark Tonic/Master Audio/EventSounds")]
public class EventSounds : MonoBehaviour, ICustomEventReceiver {
	public bool showGizmo = true;
	public MasterAudio.SoundSpawnLocationMode soundSpawnMode = MasterAudio.SoundSpawnLocationMode.CallerLocation;
	public bool disableSounds = false;
	public bool hideUnused = true;
	public bool showPoolManager = false;
	public bool logMissingEvents = true;
	
	public enum EventType {
		OnStart,
		OnVisible,
		OnInvisible,
		OnCollision,
		OnTriggerEnter,
		OnTriggerExit,
		OnMouseEnter,
		OnMouseClick,
		OnSpawned,
		OnDespawned,
		OnEnable,
		OnDisable,
		OnCollision2D,
		OnTriggerEnter2D,
		OnTriggerExit2D,
		OnParticleCollision,
		UserDefinedEvent
	}
	
	public enum VariationType {
		PlaySpecific,
		PlayRandom
	}
	
	public static List<string> layerTagFilterEvents = new List<string>() {
		EventType.OnCollision.ToString(),
		EventType.OnTriggerEnter.ToString(),
		EventType.OnTriggerExit.ToString(),
		EventType.OnCollision2D.ToString(),
		EventType.OnTriggerEnter2D.ToString(),
		EventType.OnTriggerExit2D.ToString(),
		EventType.OnParticleCollision.ToString()
	};

	public static List<MasterAudio.PlaylistCommand> playlistCommandsWithAll = new List<MasterAudio.PlaylistCommand>() {
		MasterAudio.PlaylistCommand.FadeToVolume,
		MasterAudio.PlaylistCommand.Pause,
		MasterAudio.PlaylistCommand.PlayNextSong,
		MasterAudio.PlaylistCommand.PlayRandomSong,
		MasterAudio.PlaylistCommand.Resume,
		MasterAudio.PlaylistCommand.Stop,
		MasterAudio.PlaylistCommand.Mute,
		MasterAudio.PlaylistCommand.Unmute,
		MasterAudio.PlaylistCommand.ToggleMute
	};
	
	public AudioEventGroup startSound;
	public AudioEventGroup visibleSound;
	public AudioEventGroup invisibleSound;
	public AudioEventGroup collisionSound;
	public AudioEventGroup triggerSound;
	public AudioEventGroup triggerExitSound;
	public AudioEventGroup mouseEnterSound;
	public AudioEventGroup mouseClickSound;
	public AudioEventGroup spawnedSound;
	public AudioEventGroup despawnedSound;
	public AudioEventGroup enableSound;
	public AudioEventGroup disableSound;
	public AudioEventGroup collision2dSound;
	public AudioEventGroup triggerEnter2dSound;
	public AudioEventGroup triggerExit2dSound;
	public AudioEventGroup particleCollisionSound;
	public List<AudioEventGroup> userDefinedSounds = new List<AudioEventGroup>();
	
	public bool useStartSound = false;
	public bool useVisibleSound = false;
	public bool useInvisibleSound = false;
	public bool useCollisionSound = false;
	public bool useTriggerEnterSound = false;
	public bool useTriggerExitSound = false;
	public bool useMouseEnterSound = false;
	public bool useMouseClickSound = false;
	public bool useSpawnedSound = false;
	public bool useDespawnedSound = false;
	public bool useEnableSound = false;
	public bool useDisableSound = false;
	public bool useCollision2dSound = false;
	public bool useTriggerEnter2dSound = false;
	public bool useTriggerExit2dSound = false;
	public bool useParticleCollisionSound = false;
	
	private bool isVisible;
	
	private Transform trans;
	private GameObject go;
	
	void Awake() {
		this.trans = this.transform;
		this.go = this.gameObject;
		this.SpawnedOrAwake();
	}
	
	protected virtual void SpawnedOrAwake() {
		this.isVisible = false;
	}
	
	void Start() {
		CheckForIllegalCustomEvents();

		if (this.useStartSound) {
			PlaySounds(startSound, EventType.OnStart);
		}
	}
	
	void OnBecameVisible() {
		if (this.useVisibleSound && !this.isVisible) {
			this.isVisible = true;
			PlaySounds(visibleSound, EventType.OnVisible);
		}
	}
	
	void OnBecameInvisible() {
		if (this.useInvisibleSound) {
			this.isVisible = false;
			PlaySounds(invisibleSound, EventType.OnInvisible);
		}
	}
	
	void OnEnable() {
		RegisterReceiver();
		
		if (this.useEnableSound) {
			PlaySounds(enableSound, EventType.OnEnable);
		}
	}
	
	void OnDisable() {
		UnregisterReceiver();
		
		if (!this.useDisableSound || MasterAudio.AppIsShuttingDown) {
			return;
		}
		
		PlaySounds(disableSound, EventType.OnDisable);
	}
	
	#if UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_1 || UNITY_4_2
		// these events don't exist
	#else 
		void OnTriggerEnter2D(Collider2D other) {
			if (!this.useTriggerEnter2dSound) {
				return;
			}
			
			// check filters for matches if turned on
			if (triggerEnter2dSound.useLayerFilter && !triggerEnter2dSound.matchingLayers.Contains(other.gameObject.layer)) {
				return;
			}
			
			if (triggerEnter2dSound.useTagFilter && !triggerEnter2dSound.matchingTags.Contains(other.gameObject.tag)) {
				return;
			}
			
			PlaySounds(triggerEnter2dSound, EventType.OnTriggerEnter2D);
		}

		void OnTriggerExit2D(Collider2D other) {
			if (!this.useTriggerExit2dSound) {
				return;
			}
			
			// check filters for matches if turned on
			if (triggerExit2dSound.useLayerFilter && !triggerExit2dSound.matchingLayers.Contains(other.gameObject.layer)) {
				return;
			}
			
			if (triggerExit2dSound.useTagFilter && !triggerExit2dSound.matchingTags.Contains(other.gameObject.tag)) {
				return;
			}
			
			PlaySounds(triggerExit2dSound, EventType.OnTriggerExit2D);
		}

		void OnCollisionEnter2D(Collision2D collision) {
			if (!this.useCollision2dSound) {
				return;
			}
			
			// check filters for matches if turned on
			if (collision2dSound.useLayerFilter && !collision2dSound.matchingLayers.Contains(collision.gameObject.layer)) {
				return;
			}
			
			if (collision2dSound.useTagFilter && !collision2dSound.matchingTags.Contains(collision.gameObject.tag)) {
				return;
			}
			
			PlaySounds(collision2dSound, EventType.OnCollision2D);
		}
	#endif

	void OnCollisionEnter(Collision collision) {
		if (!this.useCollisionSound) {
			return;
		}

		// check filters for matches if turned on
		if (collisionSound.useLayerFilter && !collisionSound.matchingLayers.Contains(collision.gameObject.layer)) {
			return;
		}
		
		if (collisionSound.useTagFilter && !collisionSound.matchingTags.Contains(collision.gameObject.tag)) {
			return;
		}
		
		PlaySounds(collisionSound, EventType.OnCollision);
	}
	
	void OnTriggerEnter(Collider other) {
		if (!this.useTriggerEnterSound) {
			return;
		}

		// check filters for matches if turned on
		if (triggerSound.useLayerFilter && !triggerSound.matchingLayers.Contains(other.gameObject.layer)) {
			return;
		}
		
		if (triggerSound.useTagFilter && !triggerSound.matchingTags.Contains(other.gameObject.tag)) {
			return;
		}
		
		PlaySounds(triggerSound, EventType.OnTriggerEnter);
	}

	void OnTriggerExit(Collider other) {
		if (!this.useTriggerExitSound) {
			return;
		}

		// check filters for matches if turned on
		if (triggerExitSound.useLayerFilter && !triggerExitSound.matchingLayers.Contains(other.gameObject.layer)) {
			return;
		}
		
		if (triggerExitSound.useTagFilter && !triggerExitSound.matchingTags.Contains(other.gameObject.tag)) {
			return;
		}
		
		PlaySounds(triggerExitSound, EventType.OnTriggerExit);
	}
	
	void OnParticleCollision(GameObject other) {
		if (!this.useParticleCollisionSound) {
			return;
		}

		// check filters for matches if turned on
		if (particleCollisionSound.useLayerFilter && !particleCollisionSound.matchingLayers.Contains(other.gameObject.layer)) {
			return;
		}
		
		if (particleCollisionSound.useTagFilter && !particleCollisionSound.matchingTags.Contains(other.gameObject.tag)) {
			return;
		}
		
		PlaySounds(particleCollisionSound, EventType.OnParticleCollision);
	}
	
	
	void OnMouseEnter() {
		if (this.useMouseEnterSound) {
			PlaySounds(mouseEnterSound, EventType.OnMouseEnter);
		}
	}
	
	void OnMouseDown() {
		if (this.useMouseClickSound) {
			PlaySounds(mouseClickSound, EventType.OnMouseClick);
		}
	}	
	
	void OnSpawned() {
		this.SpawnedOrAwake();

		if (this.showPoolManager && this.useSpawnedSound) {
			PlaySounds(spawnedSound, EventType.OnSpawned);
		}
	}

	void OnDespawned() {
		if (this.showPoolManager && this.useDespawnedSound) {
			PlaySounds(despawnedSound, EventType.OnDespawned);
		}
	}

	void OnDrawGizmos() {
		if (showGizmo) {
			Gizmos.DrawIcon(this.transform.position, MasterAudio.GIZMO_FILE_NAME, true);
		}
	}

	private IEnumerator TryPlayStartSound(AudioEventGroup grp, AudioEvent aEvent) {
        YieldInstruction delay = new WaitForSeconds(MasterAudio.INNER_LOOP_CHECK_INTERVAL);

        for (var i = 0; i < 3; i++) {
			yield return delay;
	
			var result = PlaySingleSound(grp, aEvent, EventType.OnStart, false);
			if (result != null && result.SoundPlayed) {
				break;
			}
		}
	}
	
	public void PlaySounds (AudioEventGroup eventGrp, EventType eType) {
		for (var i = 0; i < eventGrp.SoundEvents.Count; i++) {
			PlaySingleSound(eventGrp, eventGrp.SoundEvents[i], eType);
		}
	}
	
	private PlaySoundResult PlaySingleSound(AudioEventGroup grp, AudioEvent aEvent, EventType eType, bool isFirstTry = true) {
		if (disableSounds || MasterAudio.AppIsShuttingDown) {
			return null;
		}
		
		float volume = aEvent.volume;
		string sType = aEvent.soundType;
		float? pitch = aEvent.pitch;
		if (!aEvent.useFixedPitch) {
			pitch = null;
		}
		
		PlaySoundResult soundPlayed = null;
		
		var soundSpawnModeToUse = soundSpawnMode;
		
	 	if (eType == EventType.OnDisable || eType == EventType.OnDespawned) {
			soundSpawnModeToUse = MasterAudio.SoundSpawnLocationMode.CallerLocation;
		}
		
		switch (aEvent.currentSoundFunctionType) {
			case MasterAudio.EventSoundFunctionType.PlaySound:
		    	string variationName = null;
				if (aEvent.variationType == VariationType.PlaySpecific) {
					variationName = aEvent.variationName;
				}
			
				if (eType == EventType.OnStart && isFirstTry && !MasterAudio.SoundGroupExists(sType)) {
		           // don't try to play sound yet.
		        } else {
		            switch (soundSpawnModeToUse)
		            {
		                case MasterAudio.SoundSpawnLocationMode.CallerLocation:
		                    soundPlayed = MasterAudio.PlaySound3DAtTransform(sType, this.trans, volume, pitch, aEvent.delaySound, variationName);
		                    break;
		                case MasterAudio.SoundSpawnLocationMode.AttachToCaller:
		                    soundPlayed = MasterAudio.PlaySound3DFollowTransform(sType, this.trans, volume, pitch, aEvent.delaySound, variationName);
		                    break;
		                case MasterAudio.SoundSpawnLocationMode.MasterAudioLocation:
		                    soundPlayed = MasterAudio.PlaySound(sType, volume, pitch, aEvent.delaySound, variationName);
		                    break;
		            }
		        }
				
				if (soundPlayed == null || !soundPlayed.SoundPlayed) {
		            if (eType == EventType.OnStart && isFirstTry) {
						// race condition met. So try to play it a few more times.
		                StartCoroutine(TryPlayStartSound(grp, aEvent));
					}
					return soundPlayed;
				}
				break;
			case MasterAudio.EventSoundFunctionType.PlaylistControl:
				soundPlayed = new PlaySoundResult() {	
					ActingVariation = null,
					SoundPlayed = true,
					SoundScheduled = false
				};
		
				if (string.IsNullOrEmpty(aEvent.playlistControllerName)) {
					aEvent.playlistControllerName = MasterAudio.ONLY_PLAYLIST_CONTROLLER_NAME;
				}
			
				switch (aEvent.currentPlaylistCommand) {
					case MasterAudio.PlaylistCommand.None:
						soundPlayed.SoundPlayed = false;
						break;
					case MasterAudio.PlaylistCommand.ChangePlaylist:
						if (string.IsNullOrEmpty(aEvent.playlistName)) {
							Debug.Log("You have not specified a Playlist name for Event Sounds on '" + this.trans.name + "'.");
							soundPlayed.SoundPlayed = false;
						} else {				
							if (aEvent.playlistControllerName == MasterAudio.NO_GROUP_NAME) {
								// don't play	
							} else {
								MasterAudio.ChangePlaylistByName(aEvent.playlistControllerName, aEvent.playlistName, aEvent.startPlaylist);
							}
						}
				
						break;				
					case MasterAudio.PlaylistCommand.FadeToVolume:
						if (aEvent.allPlaylistControllersForGroupCmd) {
							MasterAudio.FadeAllPlaylistsToVolume(aEvent.fadeVolume, aEvent.fadeTime);
						} else if (aEvent.playlistControllerName == MasterAudio.NO_GROUP_NAME) {
							// don't play	
						} else {
							MasterAudio.FadePlaylistToVolume(aEvent.playlistControllerName, aEvent.fadeVolume, aEvent.fadeTime);
						}
						break;				
					case MasterAudio.PlaylistCommand.Mute:
						if (aEvent.allPlaylistControllersForGroupCmd) {
							MasterAudio.MuteAllPlaylists();
						} else if (aEvent.playlistControllerName == MasterAudio.NO_GROUP_NAME) {
							// don't play	
						} else {
							MasterAudio.MutePlaylist(aEvent.playlistControllerName);
						}
						break;				
					case MasterAudio.PlaylistCommand.Unmute:
						if (aEvent.allPlaylistControllersForGroupCmd) {
							MasterAudio.UnmuteAllPlaylists();
						} else if (aEvent.playlistControllerName == MasterAudio.NO_GROUP_NAME) {
							// don't play	
						} else {
							MasterAudio.UnmutePlaylist(aEvent.playlistControllerName);
						}
						break;				
					case MasterAudio.PlaylistCommand.ToggleMute:
						if (aEvent.allPlaylistControllersForGroupCmd) {
							MasterAudio.ToggleMuteAllPlaylists();
						} else if (aEvent.playlistControllerName == MasterAudio.NO_GROUP_NAME) {
							// don't play	
						} else {
							MasterAudio.ToggleMutePlaylist(aEvent.playlistControllerName);
						}
						break;				
					case MasterAudio.PlaylistCommand.PlayClip:
						if (string.IsNullOrEmpty(aEvent.clipName)) {
							Debug.Log("You have not specified a clip name for Event Sounds on '" + this.trans.name + "'.");
							soundPlayed.SoundPlayed = false;
						} else {
							if (aEvent.playlistControllerName == MasterAudio.NO_GROUP_NAME) {
								// don't play	
							} else {
								MasterAudio.TriggerPlaylistClip(aEvent.playlistControllerName, aEvent.clipName);
							}
						}
				
						break;		
					case MasterAudio.PlaylistCommand.PlayRandomSong:
						if (aEvent.allPlaylistControllersForGroupCmd) {
							MasterAudio.TriggerRandomClipAllPlaylists();
						} else if (aEvent.playlistControllerName == MasterAudio.NO_GROUP_NAME) {
							// don't play	
						} else {
							MasterAudio.TriggerRandomPlaylistClip(aEvent.playlistControllerName);
						}
						break;				
					case MasterAudio.PlaylistCommand.PlayNextSong:
						if (aEvent.allPlaylistControllersForGroupCmd) {
							MasterAudio.TriggerNextClipAllPlaylists();
						} else if (aEvent.playlistControllerName == MasterAudio.NO_GROUP_NAME) {
							// don't play	
						} else {
							MasterAudio.TriggerNextPlaylistClip(aEvent.playlistControllerName);
						}
						break;				
					case MasterAudio.PlaylistCommand.Pause:
						if (aEvent.allPlaylistControllersForGroupCmd) {
							MasterAudio.PauseAllPlaylists();
						} else if (aEvent.playlistControllerName == MasterAudio.NO_GROUP_NAME) {
							// don't play	
						} else {
							MasterAudio.PausePlaylist(aEvent.playlistControllerName);
						}
						break;				
					case MasterAudio.PlaylistCommand.Stop:
						if (aEvent.allPlaylistControllersForGroupCmd) {
							MasterAudio.StopAllPlaylists();
						} else if (aEvent.playlistControllerName == MasterAudio.NO_GROUP_NAME) {
							// don't play	
						} else {
							MasterAudio.StopPlaylist(aEvent.playlistControllerName);
						}
						break;				
					case MasterAudio.PlaylistCommand.Resume:
						if (aEvent.allPlaylistControllersForGroupCmd) {
							MasterAudio.ResumeAllPlaylists();
						} else if (aEvent.playlistControllerName == MasterAudio.NO_GROUP_NAME) {
							// don't play	
						} else {
							MasterAudio.ResumePlaylist(aEvent.playlistControllerName);
						}
						break;				
				}
				break;
			case MasterAudio.EventSoundFunctionType.GroupControl:
				soundPlayed = new PlaySoundResult() {	
					ActingVariation = null,
					SoundPlayed = true,
					SoundScheduled = false
				};
			
				var soundTypesForCmd = new List<string>();
				if (!aEvent.allSoundTypesForGroupCmd) {
					soundTypesForCmd.Add(aEvent.soundType);					
				} else {
					soundTypesForCmd.AddRange(MasterAudio.RuntimeSoundGroupNames);
				}
			
				for (var i = 0 ; i < soundTypesForCmd.Count; i++) {
					var soundType = soundTypesForCmd[i];	
				
					switch (aEvent.currentSoundGroupCommand) {
						case MasterAudio.SoundGroupCommand.None:
							soundPlayed.SoundPlayed = false;
							break;
						case MasterAudio.SoundGroupCommand.FadeToVolume:
							MasterAudio.FadeSoundGroupToVolume(soundType, aEvent.fadeVolume, aEvent.fadeTime);
							break;
						case MasterAudio.SoundGroupCommand.FadeOutAllOfSound:
							MasterAudio.FadeOutAllOfSound(soundType, aEvent.fadeTime);
							break;
						case MasterAudio.SoundGroupCommand.Mute:
							MasterAudio.MuteGroup(soundType);
							break;
						case MasterAudio.SoundGroupCommand.Pause:
							MasterAudio.PauseSoundGroup(soundType);
							break;
						case MasterAudio.SoundGroupCommand.Solo:
							MasterAudio.SoloGroup(soundType);
							break;
						case MasterAudio.SoundGroupCommand.StopAllOfSound:
							MasterAudio.StopAllOfSound(soundType);
							break;
						case MasterAudio.SoundGroupCommand.Unmute:
							MasterAudio.UnmuteGroup(soundType);
							break;
						case MasterAudio.SoundGroupCommand.Unpause:
							MasterAudio.UnpauseSoundGroup(soundType);
							break;
						case MasterAudio.SoundGroupCommand.Unsolo:
							MasterAudio.UnsoloGroup(soundType);
							break;
					}	
				}

				break;
			case MasterAudio.EventSoundFunctionType.BusControl:
				soundPlayed = new PlaySoundResult() {	
					ActingVariation = null,
					SoundPlayed = true,
					SoundScheduled = false
				};
			
				var busesForCmd = new List<string>();
				if (!aEvent.allSoundTypesForBusCmd) {
					busesForCmd.Add(aEvent.busName);					
				} else {
					busesForCmd.AddRange(MasterAudio.RuntimeBusNames);
				}
				
				for (var i = 0; i < busesForCmd.Count; i++) {
					var busName = busesForCmd[i];
				
					switch (aEvent.currentBusCommand) {
						case MasterAudio.BusCommand.None:
							soundPlayed.SoundPlayed = false;
							break;
						case MasterAudio.BusCommand.FadeToVolume:
							MasterAudio.FadeBusToVolume(busName, aEvent.fadeVolume, aEvent.fadeTime);
							break;
						case MasterAudio.BusCommand.Pause:
							MasterAudio.PauseBus(busName);
							break;
						case MasterAudio.BusCommand.Stop:
							MasterAudio.StopBus(busName);
							break;
						case MasterAudio.BusCommand.Unpause:
							MasterAudio.UnpauseBus(busName);
							break;
						case MasterAudio.BusCommand.Mute:
							MasterAudio.MuteBus(busName);
							break;
						case MasterAudio.BusCommand.Unmute:
							MasterAudio.UnmuteBus(busName);
							break;
						case MasterAudio.BusCommand.Solo:
							MasterAudio.SoloBus(busName);
							break;
						case MasterAudio.BusCommand.Unsolo:
							MasterAudio.UnsoloBus(busName);
							break;
					}	
				}

				break;
			case MasterAudio.EventSoundFunctionType.CustomEventControl:
				soundPlayed = new PlaySoundResult() {	
					ActingVariation = null,
					SoundPlayed = false,
					SoundScheduled = false
				};
				
				if (eType == EventType.UserDefinedEvent) {
					Debug.LogError("Custom Event Receivers cannot fire events. Occured in Transform '" + this.name + "'.");
					break;
				}
				switch (aEvent.currentCustomEventCommand) {
					case MasterAudio.CustomEventCommand.FireEvent:
						MasterAudio.FireCustomEvent(aEvent.theCustomEventName);
						break;
				}
				break;
		}
		
		if (aEvent.emitParticles && soundPlayed != null && (soundPlayed.SoundPlayed || soundPlayed.SoundScheduled)) {
			MasterAudio.TriggerParticleEmission(this.trans, aEvent.particleCountToEmit);
		}
		
		return soundPlayed;
	}
	
	private void LogIfCustomEventMissing(AudioEventGroup eventGroup) {
 		if (!logMissingEvents) {
  	       return;
        }

		if (eventGroup.isCustomEvent) {
			if (!eventGroup.customSoundActive || string.IsNullOrEmpty(eventGroup.customEventName)) {
				return;
			}
		}
		
		for (var i = 0; i < eventGroup.SoundEvents.Count; i++) {
			var aEvent = eventGroup.SoundEvents[i];

			if (aEvent.currentSoundFunctionType != MasterAudio.EventSoundFunctionType.CustomEventControl) {
				continue;
			}
	
			string customEventName = aEvent.theCustomEventName;
			if (!MasterAudio.CustomEventExists(customEventName)) {
				MasterAudio.LogWarning("Transform '" + this.name + "' is set up to receive or fire Custom Event '" + customEventName + "', which does not exist in Master Audio.");
			}
		}
	}

	#region ICustomEventReceiver methods
	public void CheckForIllegalCustomEvents() {
		if (useStartSound) {
			LogIfCustomEventMissing(startSound);
		}
		if (useVisibleSound) {
			LogIfCustomEventMissing(visibleSound);
		}
		if (useInvisibleSound) {
			LogIfCustomEventMissing(invisibleSound);
		}
		if (useCollisionSound) {
			LogIfCustomEventMissing(collisionSound);
		}
		if (useTriggerEnterSound) {
			LogIfCustomEventMissing(triggerSound);
		}
		if (useTriggerExitSound) {
			LogIfCustomEventMissing(triggerExitSound);
		}
		if (useMouseEnterSound) {
			LogIfCustomEventMissing(mouseEnterSound);
		}
		if (useMouseClickSound) {
			LogIfCustomEventMissing(mouseClickSound);
		}
		if (useSpawnedSound) {
			LogIfCustomEventMissing(spawnedSound);
		}
		if (useDespawnedSound) {
			LogIfCustomEventMissing(despawnedSound);
		}
		if (useEnableSound) {
			LogIfCustomEventMissing(enableSound);
		}
		if (useDisableSound) {
			LogIfCustomEventMissing(disableSound);
		}
		if (useCollision2dSound) {
			LogIfCustomEventMissing(collision2dSound);
		}
		if (useTriggerEnter2dSound) {
			LogIfCustomEventMissing(triggerEnter2dSound);
		}
		if (useTriggerExit2dSound) {
			LogIfCustomEventMissing(triggerExit2dSound);
		}
		if (useParticleCollisionSound) {
			LogIfCustomEventMissing(particleCollisionSound);
		}

		for (var i = 0; i <  userDefinedSounds.Count; i++) {
			var custEvent = userDefinedSounds[i];

			LogIfCustomEventMissing(custEvent);
		}
	}

	public void ReceiveEvent(string customEventName) {
		for (var i = 0; i < userDefinedSounds.Count; i++) {
			var userDefGroup = userDefinedSounds[i];

			if (!userDefGroup.customSoundActive || string.IsNullOrEmpty(userDefGroup.customEventName)) {
				continue;
			}

			if (!userDefGroup.customEventName.Equals(customEventName)) {
				continue;
			}
			
			PlaySounds(userDefGroup, EventType.UserDefinedEvent);
		}
	}
	
	public bool SubscribesToEvent(string customEventName) {
		for (var i = 0; i < userDefinedSounds.Count; i++) {
			var customGrp = userDefinedSounds[i];

			if (customGrp.customSoundActive && !string.IsNullOrEmpty(customGrp.customEventName) && customGrp.customEventName.Equals(customEventName)) {
				return true;
			}
		}
	
		return false;
	}

	public void RegisterReceiver() {
		if (userDefinedSounds.Count > 0) {
			MasterAudio.AddCustomEventReceiver(this, go);
		}
	}
	
	public void UnregisterReceiver() {
		if (userDefinedSounds.Count > 0) {
			MasterAudio.RemoveCustomEventReceiver(this);
		}
	}
	#endregion
}
