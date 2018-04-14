namespace Necromatic.Character.NPC.Strategies.Results
{
    public class TreeSpottedResult : StrategyResult
    {
        public World.Tree ToCut { get; private set; }

        public TreeSpottedResult(World.Tree tree)
        {
            NextDesiredStrategy = typeof(CutTree);
            Priority = 5;
            ToCut = tree;
        }
    }
}