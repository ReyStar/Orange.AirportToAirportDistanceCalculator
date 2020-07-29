using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Orange.AirportToAirportDistanceCalculator.Domain.Models;

namespace Orange.AirportToAirportDistanceCalculator.Domain.Tests
{
    class GeoDistanceTestCaseSource
    {
        public static IEnumerable<TestCaseData> GetGeoDistanceTestData()
        {
            return new[]
            {
                new TestCaseData(new GeoCoordinate(0.0, 0.0), new GeoCoordinate(0.0, 0.0), 0.0),
                new TestCaseData(new GeoCoordinate(22.55, 43.12), new GeoCoordinate(13.45, 100.28), 3786.251258825624),
                new TestCaseData(new GeoCoordinate(20.10, 57.30), new GeoCoordinate( 0.57, 100.21),3196.671009759937 ),
                new TestCaseData(new GeoCoordinate(51.45, 1.15), new GeoCoordinate(41.54, 12.27), 863.0311907424888),
                new TestCaseData(new GeoCoordinate(22.34, 17.05), new GeoCoordinate(51.56, 4.29), 2130.8298370015464),
                new TestCaseData(new GeoCoordinate(63.24, 56.59), new GeoCoordinate(8.50, 13.14), 4346.398369403186),
                new TestCaseData(new GeoCoordinate(90.00, 0.00), new GeoCoordinate(48.51, 2.21), 2867.58),
                new TestCaseData(new GeoCoordinate(45.04, 7.42), new GeoCoordinate(3.09, 101.42), 6261.05275709582),
                new TestCaseData(new GeoCoordinate(34.879722222222,33.630555555556), new GeoCoordinate(34.75,32.416666666667), 69.439556391155),
                new TestCaseData(new GeoCoordinate(49.38237278700955,30.05859375), new GeoCoordinate(6.839169626342808,-0.17578125), 3424.34),
                new TestCaseData(new GeoCoordinate(28.51214261987337,-16.30645751953125), new GeoCoordinate(27.93254072134666,-15.6060791015625),  58.51),
                new TestCaseData(new GeoCoordinate(36.03133177633189,-84.0234375), new GeoCoordinate(60.413852350464936,35.33203125), 4948.03),
                new TestCaseData(new GeoCoordinate(28.044106826715712,-16.688232421875), new GeoCoordinate(28.502488316130417,-16.28448486328125), 40.10),
            };
        }
    }
}
