using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour {

    [SerializeField]
    private bool _isLeftHand;

    [SerializeField]
    private bool _isRightHand;

    [SerializeField]
    private bool _isEnding;

    [SerializeField]
    private bool _isUsingSound;

    private Controller _player;

    private void OnDisable()
    {
        if (_player != null)
        {
            var tutorialScene = TutorialSceneManager.instance as TutorialSceneManager;

            if (_isUsingSound)
                tutorialScene.SoundActiveAndNotShow();
            else
                tutorialScene.NotShow();

            _player.ActivedPlayerController(false, true);
            _player.colliderObstacle = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            var tutorialScene = TutorialSceneManager.instance as TutorialSceneManager;
            _player = other.GetComponent<Controller>();
            if (_isEnding)
            {
                tutorialScene.FinishTutorial();
                return;
            }

            if (_isLeftHand && _isRightHand)
            {
                tutorialScene.ShowImage("점프하기");
                _player.speedL = 0;
                _player.speedR = 0;
                _player.colliderObstacle = true;
            }
            else if(_isLeftHand)
            {
                tutorialScene.ShowImage("왼손처리");
            }
            else if(_isRightHand)
            {
                tutorialScene.ShowImage("오른손처리");
            }
            else
            {
                tutorialScene.ShowImage("걸어가기");
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            var tutorialScene = TutorialSceneManager.instance as TutorialSceneManager;
            tutorialScene.NotShow();
            this.enabled = false;
        }
    }
}
