using System;
using UnityEngine;

[Serializable]
public class GridConfigDictionary : SerializableDictionary<Vector2, Config> { }

[Serializable]
public class ScreenObjectDictionary : SerializableDictionary<Screen, GameObject> { }

[Serializable]
public class SoundDictionary : SerializableDictionary<Sounds, AudioClip> { }

[Serializable]
public class MusicDictionary : SerializableDictionary<BackgroundMusic, AudioClip> { }

[Serializable]
public class ObjectColorDictionary : SerializableDictionary<UnityEngine.Object, Color> {}

[Serializable]
public class GridScoreDictionary : SerializableDictionary<Vector2, ScoreDictionary> { }

[Serializable]
public class ScoreDictionary : SerializableDictionary<int, float> { }

#if NET_4_6 || NET_STANDARD_2_0
[Serializable]
public class StringHashSet : SerializableHashSet<string> {}
#endif
