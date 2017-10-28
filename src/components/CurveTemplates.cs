using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlanetaryDiversity.Components
{
    /// <summary>
    /// A storage class for stock temperature and pressure curves
    /// </summary>
    public static class CurveTemplates
    {
        private static Keyframe ParseKey(String key)
        {
            String[] split = key.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            Keyframe result = new Keyframe
            {
                time = Single.Parse(split[0]),
                value = Single.Parse(split[1]),
                inTangent = Single.Parse(split[2]),
                outTangent = Single.Parse(split[3])
            };
            return result;
        }

        private static readonly FloatCurve DunaPressureCurve = new FloatCurve(new[]
        {
            ParseKey("0 1 0 -0.0007"),
            ParseKey("0.24 0.1888971 -0.000223 -0.000223"),
            ParseKey("0.4 0.03567728 -4.22E-05 -4.22E-05"),
            ParseKey("0.7 0.002220577 -2.787075E-06 -2.787075E-06"),
            ParseKey("1 0 0 0")
        });

        private static readonly FloatCurve DunaTemperatureCurve = new FloatCurve(new[]
        {
            ParseKey("0 0.932 0 -0.0004261126"),
            ParseKey("0.02 0.9312 -0.000573325 -0.000573325"),
            ParseKey("0.5 0.6148 -0.001877083 -0.001877083"),
            ParseKey("0.6 0.6 0 0"),
            ParseKey("0.9 0.6 0 0"),
            ParseKey("1 0.64 0.003746914 0")
        });

        private static readonly FloatCurve EvePressureCurve = new FloatCurve(new[]
        {
            ParseKey("0 1 -0.08693577 -0.08693577"),
            ParseKey("0.1080392 0.2240316 -0.0149408 -0.0149408"),
            ParseKey("0.2222222 0.04737232 -0.001997488 -0.001997488"),
            ParseKey("0.3333333 0.009869233 -0.0009176003 -0.0009176003"),
            ParseKey("0.5 0.0009869232 -3.677358E-05 -3.677358E-05"),
            ParseKey("0.8888889 4.539847E-06 -6.030081E-07 -6.030081E-07"),
            ParseKey("1 0 0 0")
        });

        private static readonly FloatCurve EveTemperatureCurve = new FloatCurve(new[]
        {
            ParseKey("0 1.029412 0 -0.01029338"),
            ParseKey("0.1666667 0.6862745 -0.004705439 -0.004705439"),
            ParseKey("0.5555556 0.4411765 0 0"),
            ParseKey("0.6666667 0.4656863 0 0"),
            ParseKey("0.7777778 0.3921569 0 0"),
            ParseKey("1 0.6127451 0.005894589 0")
        });

        private static readonly FloatCurve JoolPressureCurve = new FloatCurve(new[]
        {
            ParseKey("0 1 0 -0.05753474"),
            ParseKey("0.145 0.4132206 -0.01449255 -0.01449255"),
            ParseKey("0.61725 0.01464594 -0.001562163 -0.001562163"),
            ParseKey("0.75 0.001315898 -0.0001361465 -0.0001361465"),
            ParseKey("0.85 6.579488E-05 -1.001277E-05 -1.001277E-05"),
            ParseKey("1 0 0 0")
        });

        private static readonly FloatCurve JoolTemperatureCurve = new FloatCurve(new[]
        {
            ParseKey("0 1 0 -0.001182922"),
            ParseKey("0.145 0.825 -0.001207278 -0.001207278"),
            ParseKey("0.61725 0.6 0 0"),
            ParseKey("0.84 0.8 0.0009967944 0.0009967944"),
            ParseKey("0.9375 0.875 0 0"),
            ParseKey("0.97 0.835 0 0"),
            ParseKey("1 1.75 0.08717471 0")
        });

        private static readonly FloatCurve KerbinPressureCurve = new FloatCurve(new[]
        {
            ParseKey("0 1 0 -0.01501631"),
            ParseKey("0.01772893 0.8293033 -0.01289846 -0.01289826"),
            ParseKey("0.03485133 0.6877018 -0.01107876 -0.01107859"),
            ParseKey("0.05138729 0.5702444 -0.009515483 -0.009515338"),
            ParseKey("0.06735631 0.4728213 -0.00817254 -0.008172415"),
            ParseKey("0.08277728 0.3920206 -0.00701892 -0.007018813"),
            ParseKey("0.09766845 0.3250105 -0.006027969 -0.006027877"),
            ParseKey("0.1120475 0.2694408 -0.005176778 -0.0051767"),
            ParseKey("0.1259317 0.2233611 -0.004445662 -0.004445578"),
            ParseKey("0.1540917 0.1516743 -0.003016528 -0.00301646"),
            ParseKey("0.1728771 0.1171787 -0.002329273 -0.00232922"),
            ParseKey("0.1916721 0.09052848 -0.001798594 -0.001798554"),
            ParseKey("0.2382639 0.0477894 -0.0009448537 -0.0009448319"),
            ParseKey("0.3020443 0.02023288 -0.0003894095 -0.0003894005"),
            ParseKey("0.3853989 0.006815622 -0.0001252565 -0.0001252534"),
            ParseKey("0.4799117 0.002172943 -3.626878E-05 -3.626788E-05"),
            ParseKey("0.6011696 0.0005693036 -9.063159E-06 -9.062975E-06"),
            ParseKey("0.704459 0.000173086 -3.029397E-06 -3.029335E-06"),
            ParseKey("0.8095707 4.531778E-05 -8.827175E-07 -8.826996E-07"),
            ParseKey("0.890012 1.477495E-05 -3.077091E-07 -3.077031E-07"),
            ParseKey("1 0 0 0")
        });

        private static readonly FloatCurve KerbinTemperatureCurve = new FloatCurve(new[]
        {
            ParseKey("0 1.004007 0 -0.008125"),
            ParseKey("0.1259317 0.754878 -0.008096968 0"),
            ParseKey("0.2292913 0.754878 0 0.001242164"),
            ParseKey("0.3675604 0.7966899 0.001237475 0.003464929"),
            ParseKey("0.5411349 0.9430313 0.00344855 0"),
            ParseKey("0.5875605 0.9430313 0 -0.003444189"),
            ParseKey("0.8205733 0.7479094 -0.003422425 -0.002444589"),
            ParseKey("0.9828269 0.6513798 -0.002433851 0"),
            ParseKey("1 0.6513798 0 0")
        });

        private static readonly FloatCurve LaythePressureCurve = new FloatCurve(new[]
        {
            ParseKey("0 0.9999999 0 -0.005216384"),
            ParseKey("0.105 0.549535 -0.004252711 -0.004252711"),
            ParseKey("0.2 0.2925578 -0.002407767 -0.002407767"),
            ParseKey("0.34 0.1167954 -0.001092064 -0.001092064"),
            ParseKey("0.44 0.06270945 -0.0004677011 -0.0004677011"),
            ParseKey("0.62 0.02158865 -0.0001961767 -0.0001961767"),
            ParseKey("0.76 0.008395517 -7.855808E-05 -7.855808E-05"),
            ParseKey("1 0 0 0")
        });

        private static readonly FloatCurve LaytheTemperatureCurve = new FloatCurve(new[]
        {
            ParseKey("0 0.9822695 0 -0.009285714"),
            ParseKey("0.105 0.7304965 -0.009253677 0"),
            ParseKey("0.2 0.7304965 0 0.001419616"),
            ParseKey("0.34 0.7723404 0.001414257 0.003959919"),
            ParseKey("0.44 0.8351064 0.0039412 -0.0002581542"),
            ParseKey("0.62 0.7198582 -0.003911343 -0.0007623209"),
            ParseKey("0.76 0.7056738 0 0.001478429"),
            ParseKey("1 0.7588652 0 0")
        });

        public static List<KeyValuePair<FloatCurve, FloatCurve>> Atmospheres =
            new List<KeyValuePair<FloatCurve, FloatCurve>>()
            {
                new KeyValuePair<FloatCurve, FloatCurve>(DunaPressureCurve, DunaTemperatureCurve),
                new KeyValuePair<FloatCurve, FloatCurve>(EvePressureCurve, EveTemperatureCurve),
                new KeyValuePair<FloatCurve, FloatCurve>(JoolPressureCurve, JoolTemperatureCurve),
                new KeyValuePair<FloatCurve, FloatCurve>(KerbinPressureCurve, KerbinTemperatureCurve),
                new KeyValuePair<FloatCurve, FloatCurve>(LaythePressureCurve, LaytheTemperatureCurve)
            };
    }
}