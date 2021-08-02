Look at common base classes where direct inheritance doesn't work.

Should IRepository.Context move to IReadonlyRepository?
  No.  The IReadonlyRepository should expose a new Readonly type under the context property, if it needs one at all.

Should IUnitOfWork get a readonly version?
  No.  The IUnitOfWork is there for write / update operations only.  It serves no Readonly purpose.

Should IDomain get a readonly version?
  No.  IDomain doesn't expose things that are specifically concerned with reading / writing.  I exposes entity and mapping configuration.

Make a ReadOnlyDataContext?
  Yes.  Throw explicit exceptions in write-implicit overrides from DbContext.  This is so that people cannot explicitly cast to DbContext and start writing stuff.
  Probably still need to access base.Set<TEntity> in one or more operations, but ensure this is only for read purposes.

Look to see what uses of each interface can be made into the readonly version of that interface.

Extract base classes for all new concretions.
Run code cleanup, but only on new types.

Remove this file.
