using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils {
	public struct Timer {
		public float Duration { get; private set; }
		public bool Paused { get; private set; }
		private float _cachedTimeLeft;
		private float _timeLastSampled;

		public float TimeLeft {
			get {
				if (!Paused) _cachedTimeLeft = Mathf.Max(_cachedTimeLeft - (Time.time - _timeLastSampled), 0);
				_timeLastSampled = Time.time;
				return _cachedTimeLeft;
			}
			set {
				_cachedTimeLeft = value;
				_timeLastSampled = Time.time;
			}
		}
		public float ElapsedFraction { get => 1 - TimeLeft / Duration; }

		public Timer(float durationSeconds, bool pausedDefault = false) {
			Duration = durationSeconds;
			_cachedTimeLeft = 0;
			_timeLastSampled = Time.time;
			Paused = pausedDefault;
			if (!pausedDefault) Start();
		}

		public void Start() {
			TimeLeft = Duration;
			Paused = false;
		}
		public void Pause() {
			float left = TimeLeft;
			Paused = true;
		}
		public void UnPause() { 
			float left = TimeLeft; 
			Paused = false;
		}
		public void Stop() { TimeLeft = 0; }

		public static implicit operator bool(Timer timer) => timer.TimeLeft > 0;
		public static implicit operator Timer(float durationSeconds) => new Timer(durationSeconds);
	}
}