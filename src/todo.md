Questions:
- I don't seem to need any mappings in my ReadonlyDataContext or any derived types.  Is that correct?

Make sure I didn't mess up any namespaces

Look at updating Before/After save events in all data contexts the way I did it in sqe.

Decide how to get IReadonlyDataContext into queries / scalars
- Use a common base interface for IDataContext / IReadonlyDataContext
- Make IReadonlyDataContext inherit from IDataContext  and throw in non-implemented members.  Seems awfully tacky.
- Add overloads to Execute, Find, Etc... in query/scalar bases classes.
- replace IDataContext with IReadonlyDataContext in query / scalar classes and have DataContext inherit from ReadonlyDataContext.  Kind of back to square one with inheritance, though.

Standardize names for TSelector, TSelection, TProjector, TProjection

Should IRepository.Context move to IReadonlyRepository?
Should IUnitOfWork get a readonly version?
Should IDomain get a readonly version?
Make a ReadOnlyDataContext?
  Update DomainRepositoryFactory to use it.

Look to see what uses of each interface can be made into the readonly version of that interface.

Extract base classes for all new concretions.
Run code cleanup, but only on new types.

Remove this file.


Suggestions moving forward:
- Pick an approach to namespaces and stick to it.
- Pick an approach to generic filenames and stick to it.
