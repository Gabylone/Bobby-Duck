[System.Serializable]
public class Node {

	public string name;
	public int x, y;

	public bool switched;

	public Node ( string n, int p1 , int p2 ) {
		name = n;
		x = p1;
		y = p2;
		switched = false;
	}

}