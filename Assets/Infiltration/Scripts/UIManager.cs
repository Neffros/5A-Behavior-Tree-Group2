using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infiltration
{
    public class UIManager : MonoBehaviour
    {
        private bool _stateGame;
        public static bool StateSet;

        private void Start()
        {
            StateSet = false;
        }

        private void OnGUI()
        {
            if (!StateSet) return;

            var styleLabel = new GUIStyle
            {
                fontSize = 200,
            };
            var styleButton = new GUIStyle
            {
                fontSize = 100,
                alignment = TextAnchor.MiddleCenter,
            };
            GUI.skin.label = styleLabel;
            GUI.skin.button = styleButton;

            // print(StateSet);

            GUI.Label(new Rect(Screen.width / 4f, Screen.height / 2.5f, 250, 250),
                _stateGame ? "You win" : "You lose");
            if (GUI.Button(new Rect(Screen.width / 4f, Screen.height / 1.3f, 400, 150), "Retry"))
            {
                ReloadScene();
            }
        }

        public void GetStateGame(bool state)
        {
            _stateGame = state;
            StateSet = true;
        }

        private static void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}