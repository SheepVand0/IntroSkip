namespace SheepIntroSkip.Core
{
    internal class ZenjectGrabber
    {
        public ZenjectGrabber(
          BasicBeatmapObjectManager p_BasicBeatMapObjectManager)
        {
            ObjectsGrabber.ObjectManager = p_BasicBeatMapObjectManager;
        }
    }
}
