using System;
using System.Collections;
using UnityEngine;

public class PopupBGHandler : MonoBehaviour
{
    public static Action<bool> OnOverlayOpen;

    // Start is called before the first frame update
    IEnumerator Start()
    {
       // yield return new WaitForSeconds(1);
        OnOverlayOpen?.Invoke(false);
        yield return null;
    }


    private void OnDestroy()
    {
        OnOverlayOpen?.Invoke(true);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
