using Enemy;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Widget
{
    public interface IGameStateService : IGameService
    {
        public void ReloadGame();
    }
    
    public class GameStateService : IGameStateService
    {
        private readonly IEnemyService _enemyService;
        
        public GameStateService(IEnemyService enemyService)
        {
            _enemyService = enemyService;
        }
        
        public void ReloadGame()
        {
            AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(sceneBuildIndex: 1);
            asyncOperation.completed += OnUnloadSceneCompleted;
        }

        private void OnUnloadSceneCompleted(AsyncOperation asyncOperation)
        {
            _enemyService.DespawnAllEnemies();
            SceneManager.LoadSceneAsync(sceneBuildIndex: 1, LoadSceneMode.Additive);
        }
    }
}