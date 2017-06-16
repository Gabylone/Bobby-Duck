public enum State {
	//
	UndiscoveredSea,
	DiscoveredSea,
	UndiscoveredIsland,
	DiscoveredIsland,
	VisitedIsland

}

public class Chunk
{
	public State state;
	private IslandData islandData;

	public Chunk () {
	}

	public IslandData IslandData {
		get {
			return islandData;
		}
		set {
			islandData = value;

			state = State.UndiscoveredIsland;
		}
	}
}

