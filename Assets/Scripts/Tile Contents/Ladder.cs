public class Ladder : TileContent
{
    public override bool bounceOnCollision()
    {
        return false;
    }
    public void onPlayerContact()
    {
        Dungeon.instance.callLadderReached();
    }
}
