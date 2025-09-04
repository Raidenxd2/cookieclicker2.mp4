using UnityEngine;

public class MiniGameMine_Trigger : MonoBehaviour
{
    //possible types are: stone, hardstone, heat, red, white
    public string type;
    public MiniGameMine miniGameMine;

    void Update()
    {
        if (type == "stone" && miniGameMine.Player.localPosition.y <= -400 && miniGameMine.reachedStone == false)
        {
            miniGameMine.reachedStone = true;
            if (miniGameMine.tempHammerStrength >= 0.3f)
            {
                miniGameMine.DecreseHammerStrength(0.3f);
            }
        }
        else if (type == "hardstone" && miniGameMine.Player.localPosition.y <= -800 && miniGameMine.reachedHardStone == false)
        {
            miniGameMine.reachedHardStone = true;
            if (miniGameMine.tempHammerStrength >= 0.8f)
            {
                miniGameMine.DecreseHammerStrength(0.8f);
            }
        }
        else if (type == "heat" && miniGameMine.Player.localPosition.y <= -1200 && miniGameMine.reachedHeat == false)
        {
            miniGameMine.reachedHeat = true;
            if (miniGameMine.tempHammerStrength >= 1.2f)
            {
                miniGameMine.DecreseHammerStrength(1.2f);
            }
        }
        else if (type == "red" && miniGameMine.Player.localPosition.y <= -1600 && miniGameMine.reachedRed == false)
        {
            miniGameMine.reachedRed = true;
            if (miniGameMine.tempHammerStrength >= 1.2f)
            {
                miniGameMine.DecreseHammerStrength(1.2f);
            }
        }
        else if (type == "white" && miniGameMine.Player.localPosition.y <= -2000 && miniGameMine.reachedWhite == false)
        {
            miniGameMine.reachedWhite = true;
            if (miniGameMine.tempHammerStrength >= 1.4f)
            {
                miniGameMine.DecreseHammerStrength(1.4f);
            }
        }
        else if (type == "end" && miniGameMine.Player.localPosition.y <= -2340 && miniGameMine.reachedEnd == false)
        {
            miniGameMine.reachedEnd = true;
            miniGameMine.GameWin();
            if (miniGameMine.tempHammerStrength >= 1.4f)
            {
                miniGameMine.DecreseHammerStrength(1.4f);
            }
        }
    }
}