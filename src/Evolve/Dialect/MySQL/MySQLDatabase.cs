﻿using System;
using EvolveDb.Connection;
using EvolveDb.Metadata;

namespace EvolveDb.Dialect.MySQL
{
    internal class MySQLDatabase : DatabaseHelper
    {
        private const string LOCK_ID = "Evolve";

        public MySQLDatabase(WrappedConnection wrappedConnection) : base(wrappedConnection)
        {
        }

        public override string DatabaseName => "MySQL";

        public override string CurrentUser => "SUBSTRING_INDEX(USER(),'@',1)";

        public override SqlStatementBuilderBase SqlStatementBuilder => new SimpleSqlStatementBuilder();

        public override string GetCurrentSchemaName() => WrappedConnection.QueryForString("SELECT DATABASE();");

        public override IEvolveMetadata GetMetadataTable(string schema, string tableName) => new MySQLMetadataTable(schema, tableName, this);

        public override Schema GetSchema(string schemaName) => new MySQLSchema(schemaName, WrappedConnection);

        public override bool TryAcquireApplicationLock(object? lockId = null) => WrappedConnection.QueryForLong($"SELECT GET_LOCK('{lockId?? LOCK_ID}', 0);") == 1;

        public override bool ReleaseApplicationLock(object? lockId = null) => WrappedConnection.QueryForLong($"SELECT RELEASE_LOCK('{lockId ?? LOCK_ID}');") == 1;
    }
}
