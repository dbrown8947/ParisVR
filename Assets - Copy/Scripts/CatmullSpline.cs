/*  
*  FILE          : CatmullSpline.cs 
*  PROJECT       : ParisVR
*  PROGRAMMER    : Dustin Brown, Ronnie Skowron, Anthony Bastos
*  FIRST VERSION : 2018-02-08
*  DESCRIPTION   : 
*    used to determin the curve of the river and road nodes
*/
// used http://www.habrador.com/tutorials/interpolation/1-catmull-rom-splines/ as a reference


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
*   NAME    : CatmullSpline 
*   PURPOSE : Calculate the curve of each node for the roads and rivers
*/
public class CatmullSpline : MonoBehaviour
{

	public enum Uniformity
	{
		Uniform,
		Centripetal,
		Chordal
	}
	/* 
    *  METHOD        : Interpolate 
    * 
    *  DESCRIPTION   : When finding the bottom most point, use swap to filter the bottom most nodes
    *
    *  PARAMETERS    : Vector3 start : first point
                        Vector3 end : second point
                        Vector3 tanPoint1 : first tangent point
                        Vector3 tanPoint2 : second tangent point
                        float t : the curve 
      RETURN: Vector3 position :  position of the curve;
    */
	public static Vector3 Interpolate (Vector3 start, Vector3 end, Vector3 tanPoint1, Vector3 tanPoint2, float t)
	{
		// Catmull-Rom splines are Hermite curves with special tangent values.
		// Hermite curve formula:
		// (2t^3 - 3t^2 + 1) * p0 + (t^3 - 2t^2 + t) * m0 + (-2t^3 + 3t^2) * p1 + (t^3 - t^2) * m1
		// For points p0 and p1 passing through points m0 and m1 interpolated over t = [0, 1]
		// Tangent M[k] = (P[k+1] - P[k-1]) / 2
		// With [] indicating subscript
		Vector3 position = (2.0f * t * t * t - 3.0f * t * t + 1.0f) * start
		                   + (t * t * t - 2.0f * t * t + t) * tanPoint1
		                   + (-2.0f * t * t * t + 3.0f * t * t) * end
		                   + (t * t * t - t * t) * tanPoint2;

		return position;
	}

	public static Vector3 Interpolate (Vector3 start, Vector3 end, Vector3 tanPoint1, Vector3 tanPoint2, float t, out Vector3 tangent)
	{
		// Calculate tangents
		// p'(t) = (6t² - 6t)p0 + (3t² - 4t + 1)m0 + (-6t² + 6t)p1 + (3t² - 2t)m1
		tangent = (6 * t * t - 6 * t) * start
		+ (3 * t * t - 4 * t + 1) * tanPoint1
		+ (-6 * t * t + 6 * t) * end
		+ (3 * t * t - 2 * t) * tanPoint2;
		return Interpolate (start, end, tanPoint1, tanPoint2, t);
	}
	/* 
     *  METHOD        : Interpolate 
     * 
     *  DESCRIPTION   : calculate the coordinates of the curve
     *
     *  PARAMETERS    : Vector3 start : first point
                        Vector3 end : second point
                        Vector3 tanPoint1 : first tangent point
                        Vector3 tanPoint2 : second tangent point
                        float t : the curve 
                        out Vector3 tangent : the tangent calculated

        RETURN: the vector 3 position of the curve;
     */
	public static Vector3 Interpolate (Vector3 start, Vector3 end, Vector3 tanPoint1, Vector3 tanPoint2, float t, out Vector3 tangent, out Vector3 curvature)
	{
		// Calculate second derivative (curvature)
		// p''(t) = (12t - 6)p0 + (6t - 4)m0 + (-12t + 6)p1 + (6t - 2)m1
		curvature = (12 * t - 6) * start
		+ (6 * t - 4) * tanPoint1
		+ (-12 * t + 6) * end
		+ (6 * t - 2) * tanPoint2;
		return Interpolate (start, end, tanPoint1, tanPoint2, t, out tangent);

	}
		

}