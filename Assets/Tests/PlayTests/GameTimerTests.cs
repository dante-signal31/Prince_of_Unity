using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Prince;
using Tests.PlayTests.Tools.Lang;
using Tests.PlayTests.Tools.Scene;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TestTools;

namespace Tests.PlayTests
{
    public class GameTimerTests
    {
    //     private GameObject _prince;
    //     private GameObject _enemy;
    //
    //     private CameraController _cameraController;
    //     private LevelLoader _levelLoader;
    //     private GameConfiguration _gameConfiguration;
    //     private HUDManager _hudManager;
    //     private GameTimer _gameTimer;
    //     private Room _room00;
    //
    //     private string _currentScene = "TheTrap";
    //
    //     [UnitySetUp]
    //     public IEnumerator Setup()
    //     {
    //         yield return TestSceneManager.ReLoadScene(_currentScene);
    //         
    //         if (_prince == null) _prince = GameObject.Find("Prince");
    //         if (_enemy == null) _enemy = GameObject.Find("Enemy");
    //         if (_cameraController == null)
    //             _cameraController = GameObject.Find("LevelCamera").GetComponentInChildren<CameraController>();
    //         if (_levelLoader == null)
    //             _levelLoader = GameObject.Find("LevelLoader").GetComponentInChildren<LevelLoader>();
    //         if (_gameConfiguration == null)
    //             _gameConfiguration = GameObject.Find("GameManagers").GetComponentInChildren<GameConfiguration>();
    //         if (_hudManager == null)
    //             _hudManager = GameObject.Find("HUDManager").GetComponentInChildren<HUDManager>();
    //         if (_gameTimer == null)
    //             _gameTimer = GameObject.Find("GameTimer").GetComponentInChildren<GameTimer>();
    //         if (_room00 == null)
    //             _room00 = GameObject.Find("Room_0_0").GetComponentInChildren<Room>();
    //
    //         _prince.SetActive(false);
    //         
    //         AccessPrivateHelper.SetPrivateField<int>(_gameConfiguration, "gameTotalTime", 10);
    //         
    //         List<GameTimer.PlannedEvent> plannedEvents = AccessPrivateHelper.GetPrivateField<List<GameTimer.PlannedEvent>>(_gameTimer,"plannedEvents");
    //         plannedEvents.Clear();
    //         GameTimer.PlannedEvent plannedEvent1 = new GameTimer.PlannedEvent
    //         {
    //             triggeringType = GameTimer.TriggeringTimeTypes.GameTotalTimePercentage, elapsedPercentage = 0,
    //             eventToTrigger = new UnityEvent()
    //         };
    //         GameTimer.PlannedEvent plannedEvent2 = new GameTimer.PlannedEvent
    //         {
    //             triggeringType = GameTimer.TriggeringTimeTypes.GameTotalTimePercentage, elapsedPercentage = 50,
    //             eventToTrigger = new UnityEvent()
    //         };
    //         GameTimer.PlannedEvent plannedEvent3 = new GameTimer.PlannedEvent
    //         {
    //             triggeringType = GameTimer.TriggeringTimeTypes.GameTotalTimePercentage, elapsedPercentage = 100,
    //             eventToTrigger = new UnityEvent()
    //         };
    //         plannedEvent1.eventToTrigger.AddListener(PrintStartMessage);
    //         plannedEvent2.eventToTrigger.AddListener(PrintMiddleMessage);
    //         plannedEvent3.eventToTrigger.AddListener(PrintEndMessage);
    //         plannedEvents.Add(plannedEvent1);
    //         plannedEvents.Add(plannedEvent2);
    //         plannedEvents.Add(plannedEvent3);
    //         AccessPrivateHelper.AccessPrivateMethod(_gameTimer, "ReindexPlannedEventsList", null);
    //         _gameTimer.ResetTimer();
    //         
    //         yield return new EnterPlayMode();
    //     }
    //     
    //     [UnityTearDown]
    //     public IEnumerator TearDown()
    //     {
    //         yield return TestSceneManager.UnLoadScene(_currentScene);
    //     }
    //
    //     // Test that timer executes expected functions at configured times.
    //     [UnityTest]
    //     public IEnumerator GameTimerTest()
    //     {
    //         _cameraController.PlaceInRoom(_room00);
    //         _prince.SetActive(false);
    //         AccessPrivateHelper.SetPrivateField(_gameConfiguration, "gameTotalTime", 10);
    //         // Let movement happen.
    //         yield return new WaitForSeconds(1);
    //         Assert.True(_hudManager.CurrentMessage == "Start message");
    //         yield return new WaitForSeconds(4);
    //         Assert.True(_hudManager.CurrentMessage == "Middle game message");
    //         yield return new WaitForSeconds(5);
    //         Assert.True(_hudManager.CurrentMessage == "End game message");
    //     }
    //
    //     private void PrintStartMessage()
    //     {
    //         _hudManager.SetMessage("Start message");
    //     }
    //
    //     private void PrintMiddleMessage()
    //     {
    //         _hudManager.SetMessage("Middle game message");
    //     }
    //
    //     private void PrintEndMessage()
    //     {
    //         _hudManager.SetMessage("End game message");
    //     }
    }
}