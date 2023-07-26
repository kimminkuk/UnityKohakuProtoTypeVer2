using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
public class LoadSceneForPlayerSaveState : MonoBehaviour
{
    [SerializeField] private PlayerSaveManager _playerSaveManager;
    [SerializeField] private string _sceneForSaveExists;
    [SerializeField] private string _sceneForNoSave;

    private Coroutine _coroutine;

    // public void Trigger() {

    //     if (_coroutine != null) {
    //         _coroutine = StartCoroutine(LoadSceneCoroutine());
    //     }

    //     if (_playerSaveManager.SaveExists()) {
    //         SceneManager.LoadScene(_sceneForSaveExists);
    //     } else {
    //         SceneManager.LoadScene(_sceneForNoSave);
    //     }
    // }

    public async void Trigger() {

        if (_coroutine != null) {
            _coroutine = StartCoroutine(LoadSceneCoroutine());
        }

        if (await _playerSaveManager.SaveExists()) {
            SceneManager.LoadScene(_sceneForSaveExists);
        } else {
            SceneManager.LoadScene(_sceneForNoSave);
        }
    }


    private IEnumerator LoadSceneCoroutine() {
        var saveExisisTask = _playerSaveManager.SaveExists();
        yield return new WaitUntil( () => saveExisisTask.IsCompleted);

        if (saveExisisTask.Result) {
            SceneManager.LoadScene(_sceneForSaveExists);
        } else {
            SceneManager.LoadScene(_sceneForNoSave);
        }

        _coroutine = null;
    }
}
