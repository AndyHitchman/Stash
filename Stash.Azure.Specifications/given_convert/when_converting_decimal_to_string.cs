namespace Stash.Azure.Specifications.given_convert
{
    using Engine;
    using NUnit.Framework;
    using Support;

    [TestFixture]
    public class when_converting_decimal_to_string
    {
        [Test]
        public void zero_is_represented_sortably()
        {
            var zero = 0.0m;
            zero.AsString().ShouldEqual("00000000000000000000000000000.0000000000000000000000000000");
        }

        [Test]
        public void min_value_is_represented_sortably()
        {
            var min = decimal.MinValue;
            min.AsString().ShouldEqual("-20771837485735662406456049664.9999999999999999999999999999");
        }

        [Test]
        public void minus_1_value_is_represented_sortably()
        {
            var minus_one = decimal.MinusOne;
            minus_one.AsString().ShouldEqual("-99999999999999999999999999998.9999999999999999999999999999");
        }

        [Test]
        public void max_value_is_represented_sortably()
        {
            var max = decimal.MaxValue;
            max.AsString().ShouldEqual("79228162514264337593543950335.0000000000000000000000000000");
        }

        [Test]
        public void max_value_is_alphatbetically_greater_than_max_less_1()
        {
            var max = decimal.MaxValue;
            var maxLess = max - 1m;
            max.AsString().ShouldBeGreaterThan(maxLess.AsString());
        }

        [Test]
        public void min_value_is_alphatbetically_less_than_min_plus_1()
        {
            var min = decimal.MinValue;
            var minMore = min + 1m;
            min.AsString().ShouldBeLessThan(minMore.AsString());
        }

        [Test]
        public void min_value_is_alphatbetically_less_than_minus_1()
        {
            var min = decimal.MinValue;
            var minus_one = decimal.MinusOne;
            min.AsString().ShouldBeLessThan(minus_one.AsString());
        }

        [Test]
        public void small_value_is_alphatbetically_less_than_insignificant_negative_value()
        {
            var small = -34589723.0123847234m;
            var insignificant = -0.000000000001m;
            small.AsString().ShouldBeLessThan(insignificant.AsString());
        }
    }
}