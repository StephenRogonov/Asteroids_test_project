namespace _Project.Scripts.UI
{
    public class HudModel
    {
        public int LaserShotsCount { get; set; }

        public void ChangeLaserShotsCount(int shots)
        {
            LaserShotsCount += shots;
        }
    }
}