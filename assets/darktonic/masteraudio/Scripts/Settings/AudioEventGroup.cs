using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class AudioEventGroup {
	// tag / layer filters
	public bool useLayerFilter = false;
	public bool useTagFilter = false;
	public List<int> matchingLayers = new List<int>() { 0 };
	public List<string> matchingTags = new List<string>() { "Default" };
	
	// for custom events only
	public bool customSoundActive = false; 
	public bool isCustomEvent = false;
	public string customEventName = string.Empty;
	
	public List<AudioEvent> SoundEvents = new List<AudioEvent>();
}
