using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using TMPro;
using System;
using UnityEngine.UI;
using Unity.VisualScripting;
using Microsoft.VisualBasic.Devices;

public class DialogTrigger : MonoBehaviour {


    public TextMeshProUGUI dialogPopupText;

    [SerializeField] private GameObject PopUp;
    [SerializeField] private TextAsset inkJSON;
    [SerializeField] private GameObject buttonContainer;
    [SerializeField] private GameObject dialogOption1;
    [SerializeField] private GameObject dialogOption2;
    [SerializeField] private GameObject dialogOption3;
    [SerializeField] public Camera mainCam;
    [SerializeField] public Camera UICam;

    private bool inRange;
    private Story story;

    private void Awake() {
        inRange = false;
        PopUp.SetActive(false);
        story = new Story(inkJSON.text);
        mainCam.enabled = true;
        UICam.enabled = false;
    }

    private void Update() {
       if (inRange) {
           PopUp.SetActive(true);
           UpdateText();
       } else {
            PopUp.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag == "Player") {
            Debug.Log("Triggereed");
            inRange = true;
            mainCam.enabled = false;
            UICam.enabled = true;
        }
    }

    private void OnTriggerExit(Collider col) {
        if (col.gameObject.tag == "Player") {
            inRange = false;
            mainCam.enabled = true;
            UICam.enabled = false;
        }
    }

    void OnClickChoiceButton(Choice choice) {
        story.ChooseChoiceIndex(choice.index); // tells ink which choice was selected
        RefreshChoiceView(); // removes choices from the screen
        UpdateText();

    }

    private void RefreshChoiceView() {
        if (buttonContainer != null) {
            foreach(Transform button in buttonContainer.transform){
                Destroy(button.gameObject);
            }
        }
    }

    private void UpdateText() {
        while(story.canContinue) {
            dialogPopupText.text = story.ContinueMaximally();

            if (story.currentChoices.Count > 0) {
                for (int i = 0; i < story.currentChoices.Count; ++i) {
                    //var choice = story.currentChoices[i];
                    //var button = CreateChoiceButton(choice.text); // creates a choice button

                    //button.onClick.AddListener(() => OnClickChoiceButton(choice));
                }
            }
        }
    }
}
