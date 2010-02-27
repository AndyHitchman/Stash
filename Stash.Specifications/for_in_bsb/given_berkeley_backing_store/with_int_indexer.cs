namespace Stash.Specifications.for_in_bsb.given_berkeley_backing_store
{
    using System;
    using System.Linq;
    using Configuration;
    using Engine;
    using In.BDB.BerkeleyQueries;
    using Queries;

    public abstract class with_int_indexer : with_temp_dir
    {
        protected RegisteredGraph<ClassWithTwoAncestors> registeredGraph;
        protected RegisteredIndexer<ClassWithTwoAncestors, int> registeredIndexer;
        
        protected override void WithContext()
        {
            base.WithContext();

            registeredGraph = new RegisteredGraph<ClassWithTwoAncestors>();
            registeredIndexer = new RegisteredIndexer<ClassWithTwoAncestors, int>(new IntIndex());
            registeredGraph.RegisteredIndexers.Add(registeredIndexer);

            Subject.EnsureIndex(registeredIndexer);
        }
    }
}