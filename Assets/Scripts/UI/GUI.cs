using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace UI
{
    public class GUI : MonoBehaviour
    {
        [SerializeField] private GameObject p1Score;
        [SerializeField] private GameObject p2Score;

        public void UpdateScore(int playerId, int score)
        {
            if (playerId == 1)
            {
                updateText(p1Score, score.ToString());
            }
            else
            {
                updateText(p2Score, score.ToString());
            }
        }

        static private void updateText(GameObject tmp, string text)
        {
            tmp.GetComponent<TextMeshPro>().text = text;
        }
    }
}
