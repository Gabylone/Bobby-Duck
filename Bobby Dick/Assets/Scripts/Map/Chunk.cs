public enum ChunkState {
	//
	UndiscoveredSea,
	DiscoveredSea,
	UndiscoveredIsland,
	DiscoveredIsland,
	VisitedIsland

}

public class Chunk
{
	public int stateID;
	private IslandData islandData;

	public Chunk () {
	}

	public IslandData IslandData {
		get {
			return islandData;
		}
		set {
			islandData = value;

			State = ChunkState.UndiscoveredIsland;
		}
	}

	public ChunkState State {
		get {
			return (ChunkState)stateID;
		}
		set {
			stateID = (int)value;
		}
	}
}

