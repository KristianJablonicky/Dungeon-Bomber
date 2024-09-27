using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Character
{
    protected override void Start()
    {
        base.Start();
        metronome.onBeat += onTick;
    }

    public override void die()
    {
        metronome.onBeat -= onTick;
        base.die();
    }

    private void onTick(object sender, System.EventArgs e)
    {
        onTick();
        //StartCoroutine(waitHalfBeat());
    }
    private IEnumerator waitHalfBeat()
    {
        yield return new WaitForSeconds(metronome.getBeatLength() * 0.5f);
        onTick();
    }

    protected abstract void onTick();
}
