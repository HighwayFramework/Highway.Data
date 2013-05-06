using System.Linq;
using Highway.Data;
using Highway.Data.Tests.TestDomain;

namespace Highway.Data.Tests.TestQueries
{
    public class StoredProcTestQuery : AdvancedQuery<Foo>
    {
        public StoredProcTestQuery()
        {
            ContextQuery = context =>
                {
                    var parameters = new FooName("Devlin"); 
                    var list = context.CallStoredProc(new GetFoos(),parameters).ToList<Foo>();
                    list.ForEach(x => x.AttachEntity(context));
                    return list.AsQueryable();
                };
        }

        private class GetFoos : StoredProc<FooName>
        {
            public GetFoos()
            {
                this.HasName("GetFoos").HasOwner("dbo").ReturnsTypes(typeof (Foo));
            }
        }

        private class FooName
        {
            public FooName(string name)
            {
                Name = name;
            }

            [StoredProcedureAttributes.Name("Name")]
            public string Name { get; set; }
        }
    }

    
}