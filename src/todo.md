Get rid of DebugFormat / TraceFormat in favor of string interpolation.

Decide how to get IReadonlyDataContext into queries / scalars
- Use a common base interface for IDataContext / IReadonlyDataContext
- Make IReadonlyDataContext inherit from IDataContext  and throw in non-implemented members.  Seems awfully tacky.
- Add overloads to Execute, Find, Etc... in query/scalar bases classes.
- replace IDataContext with IReadonlyDataContext in query / scalar classes and have DataContext inherit from ReadonlyDataContext.  Kind of back to square one with inheritance, though.

Standardize names for TSelector, TSelection, TProjector, TProjection

Extract base classes for all new concretions.

Extract a DataContextBase (if time.  Not important)

Remove this file.


Questions:
- I don't seem to need any mappings in my ReadonlyDataContext or any derived types.  Is that correct?
- Are you OK with me updating the test assembly to .net 5.0?  Standard 3.0 is outdated.
- Are you OK with me using environment variables / user secrets to store connection strings?  I can add a little readme.md explaining how to set that up locally.
  * This will allow us to not depend on a specific database location for integration tests.
- Are you OK with me using DBUp for integration test database deployment?
- I like to use a convention that creates a database instance per test run, and deletes them when done.  Any issues here?


Suggestions moving forward:
- Pick an approach to namespaces and stick to it.
- Pick an approach to generic filenames and stick to it.
