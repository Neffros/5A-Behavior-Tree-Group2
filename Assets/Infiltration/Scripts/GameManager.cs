using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infiltration
{
    public class GameManager : MonoBehaviour
    {
        public static bool StateSet;

        private void Start()
        {
            StateSet = false;
        }
        
        public void GetStateGame(bool state)
        {
            StateSet = true;
        }

        public static void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
