using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformIterator {
	Transform[] points;
	int currPoint;

	/// <summary>
	/// Initializes a new instance of the <see cref="TransformIterator"/> class, setting the points.
	/// </summary>
	/// <param name="points">The transforms.</param>
	public TransformIterator(Transform[] points) {
		this.points = points;
		currPoint = -1;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="TransformIterator"/> class, setting the points and the current index.
	/// </summary>
	/// <param name="points">The transforms.</param>
	/// <param name="currPoint">The chosen point to start with.</param>
	public TransformIterator(Transform[] points, int currPoint) {
		this.points = points;
		this.currPoint = currPoint;
	}

	public bool EndList() {
		Debug.Log (points.Length.ToString() + " : " + currPoint.ToString());
		return currPoint >= points.Length;
	}

	/// <summary>
	/// Gets the current point index.
	/// </summary>
	/// <returns>The index.</returns>
	public int GetIndex() {
		return currPoint;
	}

	/// <summary>
	/// Gets the current point. If the GetNextPoint method hasn't been called, or if the index goes over the limit, it will set the currPoint to 0 and return the first point.
	/// </summary>
	/// <returns>The current point.</returns>
	public Transform GetPoint() {
		return points [(currPoint < 0 || currPoint >= points.Length) ? currPoint = 0 : currPoint];
	}

	/// <summary>
	/// Iterates the index, then returns the next point in the array.
	/// </summary>
	/// <returns>The transform of the next camera point.</returns>
	public Transform GetNextPoint() {
		currPoint++;
		return GetPoint();
	}

	public bool GetNextPoint(out Transform point) {
		point = points [currPoint];

		if (currPoint + 1 >= points.Length) {
			return false;
		} else {
			currPoint++;
			point = points [currPoint];
			return true;
		}
	}

	/// <summary>
	/// Resests the index of the array to -1.
	/// </summary>
	public void ResetPoints() {
		currPoint = -1;
	}

	/// <summary>
	/// Resets the current point of the array to 'index'.
	/// </summary>
	/// <param name="index">The new Index.</param>
	public void ResetPoints(int index) {
		currPoint = index;
	}

	/// <summary>
	/// Resests the camera points in the point array, and resets the index.
	/// </summary>
	/// <param name="newPoints">New points.</param>
	public void ResetPoints(Transform[] newPoints) {
		points = newPoints;
		ResetPoints ();
	}

	/// <summary>
	/// Resets the camera points and the index to newPoints and index respectively.
	/// </summary>
	/// <param name="newPoints">The new transform array of points.</param>
	/// <param name="index">The new index of the point.</param>
	public void ResetPoints(Transform[] newPoints, int index) {
		points = newPoints;
		currPoint = index;
	}
}
