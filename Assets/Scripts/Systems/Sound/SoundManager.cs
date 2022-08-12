using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Systems
{
    public sealed class SoundManager : MonoBehaviour
    {
        public Slider ambientSlider;
        public Slider musicSlider;
        public Slider effectsSlider;
        public TMP_Text ambientText;
        public TMP_Text musicText;
        public TMP_Text effectText;

        public void ChangeAmbientSound()
        {
            SoundSettings.ambientSound = ambientSlider.value;
            ambientText.text = SoundSettings.ambientSound.ToString();
        }
        public void ChangeMusicSound()
        {
            SoundSettings.musicSound = musicSlider.value;
            musicText.text = SoundSettings.musicSound.ToString();

        }
        public void ChangeEffectsSound()
        {
            SoundSettings.effectsSound = effectsSlider.value;
            Debug.Log(SoundSettings.effectsSound);
            effectText.text = SoundSettings.effectsSound.ToString();

        }
    }
}
