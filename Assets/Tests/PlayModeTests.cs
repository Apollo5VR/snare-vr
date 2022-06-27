using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;


public class PlayModeTests
{
    //Arrange, Act, Assert

    [SetUp]
    public void Setup()
    {
         //any values that need to be updated before all the tests start
    }

    //TODO - need to disable ScriptsConnector in following functions for this to run
    [UnityTest]
    public IEnumerator TestModifyHealth()
    {
        //arrange
        UserHealthManager manager = new UserHealthManager();

        float userHealthBefore = manager.healthPoints;
        float modAmount = -5.0f;
        float userHealthAfter;

        //Act
        manager.ModifyHealth("playerID", modAmount);

        yield return null;

        userHealthAfter = manager.healthPoints;

        Assert.AreNotEqual(userHealthBefore, userHealthAfter);
    }

    [TearDown]
    public void TearDown()
    {

    }

    /*
    //UNOFFICIAL METHOD FOR: Testing live in-game actions (more an integration test than Unit Test, but best hack i've witnessed)
    [UnityTest]
    public IEnumerator ExampleGrabbingListener()
    {
        TestScriptsConnector.SpawnTrap += SpawnTrapUnitTestActions;
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }

    private void SpawnTrapUnitTestActions()
    {
        //actions you want to test here
        throw new NotImplementedException();
    }

    // A Test behaves as an ordinary method
    [Test]
    public void NewTestScriptSimplePasses()
    {
        // Use the Assert class to test conditions
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator NewTestScriptWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
    */
}

