using System;
using Logic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GUI : MonoBehaviour
    {
        [SerializeField] private GameObject p1Score;
        [SerializeField] private GameObject p2Score;
        [SerializeField] private GameObject p1NextBrickImage;
        [SerializeField] private GameObject p2NextBrickImage;
        [SerializeField] private Sprite[] brickVariats;
        public void UpdateScore(int playerId, int score)
        {
            if (playerId == 1)
            {
                UpdateScore(p1Score, score);
            }
            else
            {
                UpdateScore(p2Score, score);
            }
        }

        public void SetNextBrick(int playerId, int brickId)
        {
            Debug.Log(brickId);
            Image imageRef;
            if (playerId == 1)
            {
                imageRef = p1NextBrickImage.GetComponent<Image>();
            }
            else
            {
                imageRef = p2NextBrickImage.GetComponent<Image>();
            }
            imageRef.sprite = brickVariats[brickId];
        }

        static private void UpdateScore(GameObject tmp, int newScore)
        {            
            tmp.GetComponent<TextMeshProUGUI>().text = String.Format("Wynik: {0}", newScore);
        }

        public void ResetGui()
        {
            UpdateScore(p1Score, 0);
            UpdateScore(p2Score, 0);
            p1NextBrickImage.GetComponent<Image>().sprite = null;
            p2NextBrickImage.GetComponent<Image>().sprite = null;
        }
    }
}
