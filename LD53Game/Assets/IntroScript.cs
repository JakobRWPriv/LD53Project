using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScript : MonoBehaviour
{
    void Start() {
        StartCoroutine(IntroCo());
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.S)) {
            LoadLevel();
        }
    }

    IEnumerator IntroCo() {
        yield return new WaitForSeconds(49f);
        LoadLevel();
    }

    void LoadLevel() {
        SceneManager.LoadScene(1);
    }
}
