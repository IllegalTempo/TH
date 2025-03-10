using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class FadeTrackMixer : PlayableBehaviour
{
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        Image img = playerData as Image;
        float currentAlpha = 0f;
        if (!img) return;
        int inputCount = playable.GetInputCount();
        for(int i = 0; i< inputCount;i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            if(inputWeight > 0f)
            {
                ScriptPlayable<FadeBehaviour> inputPlayable = (ScriptPlayable<FadeBehaviour>)playable.GetInput(i);
                FadeBehaviour input = inputPlayable.GetBehaviour();
                currentAlpha = inputWeight;
            }
        }
        img.color = new Color(0, 0, 0, currentAlpha);
    }
}
