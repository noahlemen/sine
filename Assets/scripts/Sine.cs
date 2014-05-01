using UnityEngine;
using System;  // Needed for Math

public class Sine : MonoBehaviour
{
	// un-optimized version
	public float frequency = 440f;
	public float gain = 0.05f;
	
	private float increment;
	private float phase;
	private float sampling_frequency = 44100f;

	void OnAudioFilterRead(float[] data, int channels)
	{
		// update increment in case frequency has changed
		increment = frequency * 2f * (float)Math.PI / sampling_frequency;
		for (var i = 0; i < data.Length; i = i + channels)
		{
			phase = phase + increment;
			// this is where we copy audio data to make them “available” to Unity
			data[i] = (gain*(float)Math.Sin(phase));
			// if we have stereo, we copy the mono data to each channel
			if (channels == 2f) data[i + 1] = data[i];
			if (phase > 2f * Math.PI) phase = 0f;
		}
	}
} 

