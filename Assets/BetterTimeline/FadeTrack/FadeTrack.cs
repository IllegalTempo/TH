using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;
[TrackClipType(typeof(FadeClip))]
[TrackBindingType(typeof(Image))]
public class FadeTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<FadeTrackMixer>.Create(graph, inputCount);
    }
}
