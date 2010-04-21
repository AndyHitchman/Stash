Stash is a persistence engine for .NET. It eschews relational models and object/relational mapping
and instead follows the No-Sql paradigm of storing serialised graphs or 'documents'.

It is effectively a Key/value store with access via Indexes.

Stash currently uses BerkeleyDB as its persistence store. Other persistent engines may be implemented. 

Executable documentation can be found in the Stash.ExecutableDoco project. I'm also trying to think of
a suitable example application. 

Improvement in the pipeline are:

  * Have queries work over the current session as well as the backing store.
  * Implement Azure Table Storage backing store (possibly build on Lokad.Cloud).
  * Look at other potential backing stores (e.g. Lucene.Net, ESENT, SQLServer).
  * Implement BSON and JSON serialisers.
  * Implement web server to serve graphs/documents directly over HTTP using a RESTful API.
  * Build a meaningful example application.
  * Refine executable documentation.
  * Explore migration strategies/tooling for:
		* New/changed indexes (calculate new index/recalculate existing index).
		* Migrating updated serialized objects (handle changes to type members).
  * Meaure performance and optimise if possible.
  * Look at how Stash could leverage Berkeley Replication and HA.
  * Explore options for sharding/partitioning data:
		* Probably a RemoteBackingStore and a ParititionBackingStore working in concert.
		* Need to examine 'hard' problems about adding/removing nodes and resilience. 
		* Rhino DHT could be useful here?