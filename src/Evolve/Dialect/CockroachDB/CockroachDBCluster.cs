using EvolveDb.Connection;
using EvolveDb.Metadata;

namespace EvolveDb.Dialect.CockroachDB
{
    internal class CockroachDBCluster : DatabaseHelper
    {
        public CockroachDBCluster(WrappedConnection wrappedConnection) : base(wrappedConnection)
        {
        }

        public override string DatabaseName => "CockroachDB";

        public override string CurrentUser => "current_user";

        public override SqlStatementBuilderBase SqlStatementBuilder => new SimpleSqlStatementBuilder();

        public override string GetCurrentSchemaName() => WrappedConnection.QueryForString("SHOW database");

        public override IEvolveMetadata GetMetadataTable(string schema, string tableName) => new CockroachDBMetadataTable(schema, tableName, this);

        public override Schema GetSchema(string schemaName) => new CockroachDBDatabase(schemaName, WrappedConnection);

        public override bool TryAcquireApplicationLock(object? lockId = null) => true;

        public override bool ReleaseApplicationLock(object? lockId = null) => true;
    }
}