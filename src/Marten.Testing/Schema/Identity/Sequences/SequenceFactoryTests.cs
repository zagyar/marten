﻿using Marten.Schema;
using Marten.Schema.Identity.Sequences;
using Marten.Testing.Fixtures;
using Shouldly;
using Xunit;

namespace Marten.Testing.Schema.Identity.Sequences
{
    [Collection("DefaultSchema")]
    public class SequenceFactoryTests : IntegratedFixture
    {
        private readonly IDocumentSchema _schema;

        public SequenceFactoryTests()
        {
            _schema = theStore.Schema;

            _schema.Sequences.Hilo(typeof(Target), new HiloSettings())
                .ShouldBeOfType<HiloSequence>();
        }

        [Fact]
        public void can_create_table_on_fly_if_necessary()
        {
            _schema.DbObjects.SchemaFunctionNames().ShouldContain("public.mt_get_next_hi");
        }

        [Fact]
        public void can_create_function_on_fly_if_necessary()
        {
            _schema.DbObjects.SchemaFunctionNames().ShouldContain("public.mt_get_next_hi");
        }
    }

    public class SequenceFactoryOnOtherDatabaseSchemaTests : IntegratedFixture
    {
        public SequenceFactoryOnOtherDatabaseSchemaTests()
        {
            StoreOptions(x => x.DatabaseSchemaName = "seq_other");

            theStore.Schema.Sequences.Hilo(typeof(Target), new HiloSettings())
                .ShouldBeOfType<HiloSequence>();
        }

        [Fact]
        public void can_create_table_on_fly_if_necessary()
        {
            theStore.Schema.DbObjects.SchemaTables().ShouldContain("seq_other.mt_hilo");
        }

        [Fact]
        public void can_create_function_on_fly_if_necessary()
        {
            theStore.Schema.DbObjects.SchemaFunctionNames().ShouldContain("seq_other.mt_get_next_hi");
        }
    }
}