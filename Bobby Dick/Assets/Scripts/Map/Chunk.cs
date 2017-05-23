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

	public int x = 0;
	public int y = 0;

	private IslandData islandData;

	public Chunk () {
		//
	}

	public Chunk (int _x , int _y) {
		x = _x;
		y = _y;
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

