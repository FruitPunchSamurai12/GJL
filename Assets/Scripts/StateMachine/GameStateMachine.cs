using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateMachine : MonoBehaviour
{
    public static event Action<IState> OnGameStateChanged;

    private static GameStateMachine _instance;
    private StateMachine _stateMachine;

    public Type CurrentStateType => _stateMachine.CurrentState.GetType();

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
        _stateMachine = new StateMachine();
        _stateMachine.OnStateChanged += state => OnGameStateChanged?.Invoke(state);
        var menu = new Menu();
        var loading = new LoadLevel();
        var play = new Play();
        var pause = new Pause();
        var credits = new CreditsState();
        var cutscene1 = new StartCutscene();
        var cutscene2 = new EndCutscene();
        _stateMachine.SetState(menu);

        _stateMachine.AddTransition(menu, cutscene1, () => PlayButton.LevelToLoad != null);
        _stateMachine.AddTransition(cutscene1, loading, () => Cuscene.completed);
        _stateMachine.AddTransition(loading, play, loading.Finished);
        _stateMachine.AddTransition(play, pause, () => Controller.Instance.PausePressed);
        _stateMachine.AddTransition(pause, play, () => Controller.Instance.PausePressed);
        _stateMachine.AddTransition(pause, play, () => Continue.Pressed);
        _stateMachine.AddTransition(pause, menu, () => BackButton.Pressed);
        _stateMachine.AddTransition(play, cutscene2,() => GameManager.Instance.GameCleared);
        _stateMachine.AddTransition(cutscene2, credits, () => Cuscene.completed);
        _stateMachine.AddTransition(credits, menu, () => BackButton.Pressed);

    }

    private void Update()
    {
        _stateMachine.Tick();
    }
}

public class Menu : IState
{
    public void OnEnter()
    {
        PlayButton.LevelToLoad = null;
        SceneManager.LoadSceneAsync("Menu");
    }

    public void OnExit()
    {

    }

    public void Tick()
    {

    }
}

public class StartCutscene : IState
{
    public void OnEnter()
    {
        Cuscene.completed = false;
        SceneManager.LoadSceneAsync("Cutscene1");
    }

    public void OnExit()
    {

    }

    public void Tick()
    {

    }
}

public class EndCutscene : IState
{
    public void OnEnter()
    {
        Cuscene.completed = false;
        SceneManager.LoadSceneAsync("Cutscene2");
    }

    public void OnExit()
    {

    }

    public void Tick()
    {

    }
}

public class Play : IState
{
    public void OnEnter()
    {
        GameManager.Instance.InitializeGame();
    }

    public void OnExit()
    {

    }

    public void Tick()
    {

    }
}

public class LoadLevel : IState
{
    public bool Finished() => _operations.TrueForAll(t => t.isDone);
    private List<AsyncOperation> _operations = new List<AsyncOperation>();
    public void OnEnter()
    {
        _operations.Add(SceneManager.LoadSceneAsync("MainLevel"));
        _operations.Add(SceneManager.LoadSceneAsync("HUD", LoadSceneMode.Additive));
    }

    public void OnExit()
    {
        _operations.Clear();
    }

    public void Tick()
    {

    }
}

public class Pause : IState
{
    public static bool Active { get; private set; }
    public void OnEnter()
    {
        Time.timeScale = 0f;
        Active = true;
    }

    public void OnExit()
    {
        Time.timeScale = 1f;
        Active = false;
    }

    public void Tick()
    {

    }
}

public class CreditsState : IState
{
    public void OnEnter()
    {
        SceneManager.LoadSceneAsync("Credits");
    }

    public void OnExit()
    {
        GameManager.Instance.GameCleared = false;
    }

    public void Tick()
    {

    }
}