namespace Stash.Specifications.for_engine.for_partitioning.given_partitioned_backing_store
{
    using System;
    using System.Collections.Generic;
    using BackingStore;
    using Engine.Partitioning;
    using Queries;
    using Rhino.Mocks;
    using Support;

    public class when_told_to_get_the_count_of_records_for_query : Specification
    {
        private Dictionary<IPartition, IBackingStore> backingStores;
        private PartitioningBackingStore sut;
        private IPartitionedQuery mockQuery;
        private IPartition mockPartition1;
        private IPartition mockPartition2;
        private IBackingStore mockBackingStore1;
        private IBackingStore mockBackingStore2;
        private int actual;

        protected override void WithContext()
        {
            mockPartition1 = MockRepository.GenerateStub<IPartition>();
            mockPartition2 = MockRepository.GenerateStub<IPartition>();
            mockBackingStore1 = MockRepository.GenerateMock<IBackingStore>();
            mockBackingStore2 = MockRepository.GenerateMock<IBackingStore>();
            mockQuery = MockRepository.GenerateStub<IPartitionedQuery>();

            backingStores = new Dictionary<IPartition, IBackingStore>
                {
                    {mockPartition1, mockBackingStore1}, 
                    {mockPartition2, mockBackingStore2}
                };

            sut = new PartitioningBackingStore(backingStores);
        }

        protected override void Given()
        {
            mockQuery.Expect(_ => _.GetQueryForPartition(mockPartition1)).Return(mockQuery);
            mockQuery.Expect(_ => _.GetQueryForPartition(mockPartition2)).Return(mockQuery);

            mockBackingStore1.Expect(_ => _.Count(mockQuery)).Return(13);
            mockBackingStore2.Expect(_ => _.Count(mockQuery)).Return(29);
        }

        protected override void When()
        {
            actual = sut.Count(mockQuery);
        }

        [Then]
        public void it_should_get_the_total_of_the_count_of_both_backing_stores()
        {
            actual.ShouldEqual(42);
        }

        [Then]
        public void it_should_have_delegated_the_work_to_both_backing_stores()
        {
            mockBackingStore1.VerifyAllExpectations();
            mockBackingStore2.VerifyAllExpectations();
        }

        [Then]
        public void it_should_have_asked_for_partitioned_query_for_partition_specific_queries()
        {
            mockQuery.VerifyAllExpectations();
        }
    }
}