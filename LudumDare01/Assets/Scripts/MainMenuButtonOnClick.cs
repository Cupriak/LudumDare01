using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButtonOnClick : MonoBehaviour {

   public void PlaySoundEffect()
    {
        GetComponent<AudioSource>().Play();
    }
}
