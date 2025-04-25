using UnityEngine;

namespace Game
{
    public class Privacy : MonoBehaviour
    {
        public void OpenURL(string url)
        {
            Application.OpenURL(url);
        }
    }
}

