namespace FloppyFinchLogics.GameLogics.ExtendedLogics;

public class PowerupProperties
{
    public static void HeartPickup()
    {
        ExtendedGame.Hearts++;
    }

    public static void JetpackDuration()
    {
        ExtendedGame.Jetpack = 15; /*takes constant from config and applies instead of hard-coded numbers*/
    }

    public static async void ScoreMultiplierDuration(int seconds = 5)
    {
        ExtendedGame.ScoreMultiplier = true;
        while (seconds != 0)
        {
            /*calls textbox timer update method*/
            await Task.Delay(seconds *
                             1000); /*seconds will be renamed into a new variable, and its data will be taken from config*/
            seconds--;
        }

        ExtendedGame.ScoreMultiplier = false;
    }

    public static async void ShieldDuration(int seconds = 15)
    {
        ExtendedGame.Shield = true;
        while (seconds != 0)
        {
            /*calls textbox timer update method*/
            await Task.Delay(seconds *
                             1000); /*seconds will be renamed into a new variable, and its data will be taken from config*/
            seconds--;
        }

        ExtendedGame.Shield = false;
    }
}