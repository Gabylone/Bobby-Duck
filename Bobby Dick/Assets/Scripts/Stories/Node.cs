[System.Serializable]
public class Node {

	public string name;
	public int x, y;

	public int decal;

	public Node () {
		
	}

	public Node ( string n, int p1 , int p2 ) {
		name = n;
		x = p1;
		y = p2;
		decal = 0;
	}

}