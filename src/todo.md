Look at updating Before/After save events in all data contexts the way I did it in sqe.

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