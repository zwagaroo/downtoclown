using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class ScreenItem
{
    public string name;
    public GameScreen screen;
}


[Serializable]
public class ScreenManager : MonoBehaviour
{

    public List<ScreenItem> screens = new();

    public GameScreen currentScreen;

    public GameScreen GetScreen(string name)
    {
        foreach (var screen in screens)
        {
            if (screen.name == name)
            {
                return screen.screen;
            }
        }
        return null;
    }

    public void SetScreen(string screenName)
    {
        if (currentScreen != null)
        {
            Hide(currentScreen);
        }

        var screen = GetScreen(screenName);

        if (!screen)
        {
            Debug.Log("YOU MESSED UP THE SCREEN NAME!!!!");
            return;
        }

        currentScreen = screen;

        //possibly add a transition here?

        Show(currentScreen);
    }

    public void Hide(GameScreen screen)
    {
        screen.Hide();
    }

    public void Show(GameScreen screen)
    {
        screen.Show();
    }

    /// <summary>
    /// hides all screens
    /// </summary>
    public void HideAll()
    {
        foreach(ScreenItem screenItem in screens)
        {
            screenItem.screen.Hide();
        }
    }
}
