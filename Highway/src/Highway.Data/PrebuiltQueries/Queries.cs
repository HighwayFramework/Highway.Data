#region

using System;

#endregion

namespace Highway.Data
{
    /// <summary>
    ///     Base Library of queries already built
    /// </summary>
    public static class Queries
    {
        public static GetById<int, T> GetById<T>(int id) where T : class, IIdentifiable<int>
        {
            return new GetById<int, T>(id);
        }

        public static GetById<Guid, T> GetById<T>(Guid id) where T : class, IIdentifiable<Guid>
        {
            return new GetById<Guid, T>(id);
        }

        public static GetById<long, T> GetById<T>(long id) where T : class, IIdentifiable<long>
        {
            return new GetById<long, T>(id);
        }

        [Obsolete(
            "Use with GREAT care, This returns the ENTIRE table into memory.  If you intend to do this, and wish to clear the warning, please instantiate Highway.Data.PreBuiltQueries.FindAll instead."
            )]
        public static FindAll<T> FindAll<T>() where T : class
        {
            return new FindAll<T>();
        }
    }
}