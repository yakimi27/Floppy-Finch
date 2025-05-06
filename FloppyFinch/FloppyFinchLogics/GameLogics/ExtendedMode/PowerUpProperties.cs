namespace FloppyFinchLogics.GameLogics.ExtendedLogics;

public class PowerUpProperties
{
    public static void HeartPickup()
    {
        ExtendedModeGame.Hearts++;
    }

    public static void JetpackDuration()
    {
        ExtendedModeGame.Jetpack = 15; /*takes constant from config and applies instead of hard-coded numbers*/
    }

    public static async void ScoreMultiplierDuration(int seconds = 5)
    {
        ExtendedModeGame.ScoreMultiplier = true;
        while (seconds != 0)
        {
            /*calls textbox timer update method*/
            await Task.Delay(seconds *
                             1000); /*seconds will be renamed into a new variable, and its data will be taken from config*/
            seconds--;
        }

        ExtendedModeGame.ScoreMultiplier = false;
    }

    public static async void ShieldDuration(int seconds = 15)
    {
        ExtendedModeGame.Shield = true;
        while (seconds != 0)
        {
            /*calls textbox timer update method*/
            await Task.Delay(seconds *
                             1000); /*seconds will be renamed into a new variable, and its data will be taken from config*/
            seconds--;
        }

        ExtendedModeGame.Shield = false;
    }
}