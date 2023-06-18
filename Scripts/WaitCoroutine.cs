using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitCoroutine
{

    public IEnumerator DisablePanel(GameObject gameObject, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        gameObject.SetActive(false);
        Debug.Log("Reached coroutine");
    }

    public IEnumerable Wait(float seconds) { yield return new WaitForSeconds(seconds); }

}
