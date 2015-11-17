using UnityEngine;
public class FightTest : MonoBehaviour
{
    #region MonoBehaviour methods

    void Start()
    {
        LoadSceneCtrller.Instance.LoadLevel(GameSceneType.PVEScene);
    }

    #endregion
}
