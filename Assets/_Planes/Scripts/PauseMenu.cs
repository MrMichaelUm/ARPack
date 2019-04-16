using UnityEngine;

namespace Planes
{
    public class PauseMenu : MonoBehaviour
    {
        public static PauseMenu Instance;

        #region Singleton
        private void Awake()
        {
            if (Instance != null)
                return;
            Instance = this;
        }
        #endregion

        public void PauseOn()
        {
            Time.timeScale = 0;
        }

        public void PauseOff()
        {
            Time.timeScale = 1;
        }
    }
}