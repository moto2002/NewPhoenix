using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class LoadLevelController : MonoBehaviour
{
    public static LoadLevelController Instance { get; private set; }
    public GameSceneType CurrentScene { get; private set; }

    #region MonoBehaviour methods

    void Awake()
    {
        Instance = this;
        this.CurrentScene = GameSceneType.LoginScene;
    }

    #endregion

    #region LoadLevel

    public void LoadLevel(GameSceneType level)
    {
        this.ClearUI();
        StopAllCoroutines();
        StartCoroutine(this.LoadLevelInspector(level));
    }

    private IEnumerator<AsyncOperation> LoadLevelInspector(GameSceneType level)
    {
        AsyncOperation operation = Application.LoadLevelAsync((int)level);
        yield return operation;
        //Debug.Log("LoadLevelInspector : " + operation.isDone);

        if (operation.isDone)
        {
            GameSceneType lastLevel = this.CurrentScene;
            this.CurrentScene = level;
        }
    }

    #endregion

    #region private methods

    private void ClearUI()
    {
        UIController.Instance.CloseAllOpendPanel();
    }

    #endregion
}
