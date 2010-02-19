//namespace Stash.Specifications.for_configuration.given_map_context
//{
//    using System;
//    using NUnit.Framework;
//    using Support;
//
//    [TestFixture]
//    public class when_told_to_reduce : with_dummy_map_context
//    {
//        [Test]
//        public void it_should_complain_if_the_reducer_is_null()
//        {
//            Reduction<object, object> expected = null;
//
//            typeof(ArgumentNullException)
//                .ShouldBeThrownBy(() => Sut.ReduceWith(expected));
//        }
//
//        [Test]
//        public void it_should_register_the_reducer()
//        {
//            var expected = new DummyReduction();
//            Sut.ReduceWith(expected);
//            Sut.RegisteredMapper.RegisteredReducers.ShouldContain(reducer => reducer.Reduction == expected);
//        }
//    }
//}