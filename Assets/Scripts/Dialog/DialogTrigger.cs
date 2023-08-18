using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using TMPro;
using System;
using UnityEngine.UI;

public class DialogTrigger : MonoBehaviour {


    public TextMeshProUGUI dialogPopupText;

    [SerializeField] private GameObject PopUp;
    [SerializeField] private TextAsset inkJSON;

    [SerializeField] private VerticalLayoutGroup choiceButtonContainer;
    [SerializeField] private Button choiceButtonPrefab;

    private bool inRange;
    private Story story;

    private void Awake() {
        inRange = false;
        PopUp.SetActive(false);
        story = new Story(inkJSON.text);
        Debug.Log(story.Continue());
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
            inRange = true;
        }
    }

    private void OnTriggerExit(Collider col) {
        if (col.gameObject.tag == "Player") {
            inRange = false;
        }
    }

    private void UpdateText() {
        while(story.canContinue) {
            dialogPopupText.text = story.ContinueMaximally();

            if (story.currentChoices.Count > 0) {
                string choices = "";
                for (int i = 0; i < story.currentChoices.Count; ++i) {
                    Choice choice = story.currentChoices[i];
                    choices += choice.text;
                }
                dialogPopupText.text = choices;
            }
        }
    }
}
