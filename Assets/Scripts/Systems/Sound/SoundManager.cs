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
        [SerializeField] private GameObject musicPlayer;
        private MusicHandler musicHandler;

        public void Awake()
        {
            musicHandler = musicPlayer.GetComponent<MusicHandler>();
        }
        public void ChangeAmbientSound()
        {
            SoundSettings.ambientSound = ambientSlider.value / 100;
            ambientText.text = ambientSlider.value.ToString();
        }
        public void ChangeMusicSound()
        {
            SoundSettings.musicSound = musicSlider.value / 100;
            musicText.text = musicSlider.value.ToString();
            musicHandler.UpdateSound();
        }
        public void ChangeEffectsSound()
        {
            SoundSettings.effectsSound = effectsSlider.value / 100;
            effectText.text = effectsSlider.value.ToString();
        }
    }
}
