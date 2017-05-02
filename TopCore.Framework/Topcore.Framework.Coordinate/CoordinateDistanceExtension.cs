﻿#region	License

//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → TopCore </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> TopCore </Project>
//     <File>
//         <Name> CoordinateDistanceExtension.cs </Name>
//         <Created> 27 Apr 17 1:28:26 PM </Created>
//         <Key> 935ee223-1480-4d8b-8990-3901d4b419ee </Key>
//     </File>
//     <Summary>
//         CoordinateDistanceExtension.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------

#endregion License

using System;

namespace Topcore.Framework.Coordinate
{
	public static class CoordinateDistanceExtension
	{
		/// <summary>
		///     Distance to targetCoordinate 
		/// </summary>
		/// <param name="src"> </param>
		/// <param name="dest"></param>
		/// <returns> UnitOfLength.Kilometer </returns>
		public static double DistanceTo(this Coordinate src, Coordinate dest)
		{
			return DistanceTo(src, dest, UnitOfLength.Kilometer);
		}

		/// <summary>
		///     By Spherical law of cosines http://en.wikipedia.org/wiki/Spherical_law_of_cosines 
		/// </summary>
		public static double DistanceTo(this Coordinate src, Coordinate dest, UnitOfLength unitOfLength)
		{
			var theta = src.Longitude - dest.Longitude;
			var thetaRad = theta * CoordinateConst.DegreesToRadians;

			var targetRad = dest.Latitude * CoordinateConst.DegreesToRadians;
			var baseRad = src.Latitude * CoordinateConst.DegreesToRadians;

			var dist =
				Math.Sin(baseRad) * Math.Sin(targetRad) + Math.Cos(baseRad) *
				Math.Cos(targetRad) * Math.Cos(thetaRad);

			dist = Math.Acos(dist);

			// calculate to earth radius by miles
			dist = dist * CoordinateConst.EarthRadiusMile;

			return unitOfLength.ConvertFromMiles(dist);
		}

		/// <summary>
		///     By Haversine https://en.wikipedia.org/wiki/Haversine_formula 
		/// </summary>
		/// <returns></returns>
		public static double DistanceToByHaversine(this Coordinate src, Coordinate dest, UnitOfLength unitOfLength)
		{
			double dLat = (dest.Latitude - src.Latitude) * CoordinateConst.DegreesToRadians;
			double dLon = (dest.Longitude - src.Longitude) * CoordinateConst.DegreesToRadians;

			double a = Math.Pow(Math.Sin(dLat / 2), 2) +
					   Math.Cos((src.Latitude) * CoordinateConst.DegreesToRadians) * Math.Cos((dest.Latitude) * CoordinateConst.DegreesToRadians) *
					   Math.Pow(Math.Sin(dLon / 2), 2);

			// central angle, aka arc segment angular distance
			double centralAngle = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
			double dist = CoordinateConst.EarthRadiusMile * centralAngle;

			return unitOfLength.ConvertFromMiles(dist);
		}

		/// <summary>
		///     By Geographical distance http://en.wikipedia.org/wiki/Geographical_distance 
		/// </summary>
		public static double DistanceToByGeo(this Coordinate src, Coordinate dest, UnitOfLength unitOfLength)
		{
			double radLatSrc = src.Latitude * CoordinateConst.DegreesToRadians;
			double radLatDest = dest.Latitude * CoordinateConst.DegreesToRadians;
			double dLat = radLatDest - radLatSrc;
			double dLon = (dest.Longitude - src.Longitude) * CoordinateConst.DegreesToRadians;

			double a = (dLon) * Math.Cos((radLatSrc + radLatDest) / 2);

			// central angle, aka arc segment angular distance
			double centralAngle = Math.Sqrt(a * a + dLat * dLat);

			// great-circle (orthodromic) distance on Earth between 2 points
			var dist = CoordinateConst.EarthRadiusMile * centralAngle;
			return unitOfLength.ConvertFromMiles(dist);
		}

		public class UnitOfLength
		{
			public static UnitOfLength Meter = new UnitOfLength(CoordinateConst.MileToMeter);
			public static UnitOfLength Kilometer = new UnitOfLength(CoordinateConst.MileToKilometer);
			public static UnitOfLength NauticalMile = new UnitOfLength(CoordinateConst.NauticalMileToMile);
			public static UnitOfLength Mile = new UnitOfLength(1);

			private readonly double _fromMilesFactor;

			private UnitOfLength(double fromMilesFactor)
			{
				_fromMilesFactor = fromMilesFactor;
			}

			public double ConvertFromMiles(double input)
			{
				return input * _fromMilesFactor;
			}
		}
	}
}