namespace Highway.Data.Tests.TestDomain
{
    public class Foo
    {
        [StoredProcedureAttributes.Name("testID")]
        public virtual int Id { get; set; }

        [StoredProcedureAttributes.Name("testName")]
        public virtual string Name { get; set; }

        [StoredProcedureAttributes.Name("testAddress")]
        public virtual string Address { get; set; }
    }
}