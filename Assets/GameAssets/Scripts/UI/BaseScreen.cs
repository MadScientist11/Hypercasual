using UnityEngine;

namespace Hypercasual.UI
{
    public class BaseScreen : MonoBehaviour
    {
        public virtual void Initialize()
        {
        }

        public void Show() =>
            gameObject.SetActive(true);

        public void Hide() =>
            gameObject.SetActive(false);

        public void Refresh()
        {
            Hide();
            Show();
        }

        protected virtual void OnRefresh()
        {
        }
    }
}