using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour {

    public List<GameObject> sprites;
    public List<GameObject> texts;

    private int current;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < texts.Count; i++) {
            sprites[i].SetActive(false);
            texts[i].SetActive(false);
        }
        current = 0;
        sprites[current].SetActive(true);
        texts[current].SetActive(true);
    }

    public void next() {
        sprites[current].SetActive(false);
        texts[current].SetActive(false);
        if (current != sprites.Count-1) {
            current++;
        }
        sprites[current].SetActive(true);
        texts[current].SetActive(true);
    }
}
