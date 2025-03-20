using UnityEngine;

namespace UI
{
    public class MainUi : MonoBehaviour
    {

        public void OnStartClick()
        {
            StartNewGame();
        }

        public void StartNewGame()
        {
            gameObject.SetActive(false);
        }
    }
}
