Should IRepository.Context move to IReadonlyRepository?
Should IUnitOfWork get a readonly version?
Should IDomain get a readonly version?
Make a ReadOnlyDataContext


Look to see what uses of each interface can be made into the readonly version of that interface.

Extract base classes for all new concretions.
Run code cleanup, but only on new types.

Remove this file.