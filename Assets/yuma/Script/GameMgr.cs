using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMgr : SingletonMonoBehaviour<GameMgr>
{
    private string SceneBuf;
    private string SceneBufBuf;

    public string backSceneName;    // ひとつ前のシーンの名前を保存する用

    void Start()
    {
        Application.targetFrameRate = 60;
        SceneBuf = GetNowSceneName();   // 今のシーンを入れる
    }

    private void Update()
    {

        // シーンが変わったら前のシーンを入れる
        if (SceneBuf != GetNowSceneName())
        {
            SceneBufBuf = SceneBuf;
            SceneBuf = GetNowSceneName();
        }
    }

    static public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Stage1シーンへ遷移する
    public void GotoBuildScene()
    {

        ChangeScene("BulidScene");
    }

    // Stage1シーンへ遷移する
    public void GotoTitleScene() {

        ChangeScene("TitleScene");
    }


    // ゲームを終了する
    public void GotoQuit()
    {

        Application.Quit();
    }

    // 現在のシーンを返す
    public string GetNowSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

    // 現在のシーンを返して読み込みし直す
    public void Restart()
    {
        ChangeScene(GetNowSceneName());
    }

    public void BackScene()
    {
        ChangeScene(SceneBufBuf);
    }


}
