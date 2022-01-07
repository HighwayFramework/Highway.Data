Consider removing IReadonlyDataContext from InMemoryDataContext.

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

Suggestions moving forward:
- Pick an approach to namespaces and stick to it.
- Pick an approach to generic filenames and stick to it.
