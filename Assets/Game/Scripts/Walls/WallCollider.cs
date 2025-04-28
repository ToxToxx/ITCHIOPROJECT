using UnityEngine;

namespace Game
{
    public class WallCollider : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            GameStateManager.Instance.SetState(GameState.Result);
        }

    }
}