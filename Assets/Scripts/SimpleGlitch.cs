using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SimpleGlitch : MonoBehaviour
{
    public float glitchChance = 0.1f;
    public Renderer holoRenderer;
    private WaitForSeconds glitchLoopWait = new WaitForSeconds(.1f);
    private WaitForSeconds glitchDuration = new WaitForSeconds(.1f);

    private IEnumerator Start()
    {
        while (true)
        {
            var glitchTest = Random.Range(0f, 1f);
            if (glitchTest <= glitchChance)
                StartCoroutine(Glitch());

            yield return glitchLoopWait;
        }
    }

    private IEnumerator Glitch()
    {
        glitchDuration = new WaitForSeconds(Random.Range(.05f, .25f));
        holoRenderer.material.SetFloat("_Amount", 1f);
        holoRenderer.material.SetFloat("_CutoutThresh", .29f);
        holoRenderer.material.SetFloat("_Amplitude", Random.Range(5, 10));
        holoRenderer.material.SetFloat("_Speed", Random.Range(1, 10));
        yield return glitchDuration;
        holoRenderer.material.SetFloat("_Amount", 0f);
        holoRenderer.material.SetFloat("_CutoutThresh", 0f);
    }
}
