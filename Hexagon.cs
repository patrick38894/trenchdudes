class Hexagon {
	public Triangle [] triangles = new Triangle[6];
	public static float diamond = 1f / 2f * Math.Sqrt(3);
	public static float hourglass = 2f / math.Sqrt(3);
	public static float diagonal = 1.0f;
	
	public int clock6(int toClock) {
		if (toClock >= 0)
			return toClock % 6;
		return 6 + toClock; //only works for numbers >= -6
	}

	public int clock12(int toClock) {
		if (toClock >= 0)
			return toClock % 12;
		return toClock + 12; // for n >= -12
	}
	
	public int clock12Invert(int toInvert) {
		return clock12(toInvert + 6);
	}

	public int clock6Invert(int toInvert) {
		return clock12(toInvert + 3);
	}

	public void linkTo(int index, Hexagon targetHex) {
		Triangle.doubleLink(triangles[index], 2 * index, targetHex.triangles[clock6Invert(index)], clock12Invert(2 * index), diamond); //mid2mid
		Triangle.doubleLink(triangles[clock6(index-1)], clock12(2 * index + 1),
						targetHex.triangles[clock6Invert(index)], clock12(clock12Invert(2 * index) + 1), diagonal); //left2mid
		Triangle.doubleLink(triangles[clock6(index+1)], clock12(2 * index - 1),
						targetHex.triangles[clock6Invert(index)], clock12(clock12Invert(2 * index) - 1), diagonal); //right2mid
		Triangle.doubleLink(triangles[index], clock12(2 * index-1),
						targetHex.triangles[clock6(clock6invert(index) +1)], clock12(clock12invert(index * 2) - 1), diagonal); //mid2left
		Triangle.doubleLink(triangles[index], clock12(2 * index+1),
						targetHex.triangles[clock6(clock6invert(index) -1)], clock12(clock12invert(index * 2) + 1), diagonal); //mid2right
		Triangle.doubleLink(triangles[clock6(index-1), clock12(index * 2),
						targetHex.triangles[clock6(clock6invert(index) +1)], clock12(clock12invert(index * 2)), hourglass); //left2left
		Triangle.doubleLink(triangles[clock6(index+1), clock12(index * 2),
						targetHex.triangles[clock6(clock6invert(index) -1)], clock12(clock12invert(index * 2)), hourglass); //left2left
	}

	public void linkIn () {
	}
}
