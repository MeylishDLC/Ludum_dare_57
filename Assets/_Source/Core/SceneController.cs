using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Core
{
    public class SceneController
    {
        public void RestartScene()
        {
            var curSceneIndex = SceneManager.GetActiveScene().buildIndex; 
            SceneManager.LoadSceneAsync(curSceneIndex);
        }
    }
}