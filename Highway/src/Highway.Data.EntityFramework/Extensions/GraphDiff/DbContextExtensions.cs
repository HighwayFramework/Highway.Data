/*
 * This code is provided as is with no warranty. If you find a bug please report it on github.
 * If you would like to use the code please leave this comment at the top of the page
 * License MIT (c) Brent McKendrick 2012
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Highway.Data.EntityFramework.Extensions.GraphDiff;
using Highway.Data.EntityFramework.Extensions.GraphDiff.Internal;
using Highway.Data.EntityFramework.Extensions.GraphDiff.Internal.Graph;

namespace Highway.Data
{
	public static class DbContextExtensions
	{
        /// <summary>
        /// Merges a graph of entities with the data store.
        /// </summary>
        /// <typeparam name="T">The type of the root entity</typeparam>
        /// <param name="context">The database context to attach / detach.</param>
        /// <param name="entity">The root entity.</param>
        /// <param name="mapping">The mapping configuration to define the bounds of the graph</param>
        /// <returns>The attached entity graph</returns>
	    public static T UpdateGraph<T>(this IDataContext context, T entity, Expression<Func<IUpdateConfiguration<T>, object>> mapping = null) where T : class
        {
            var dbContext = context as DbContext;
            if (dbContext == null)
            {
                return entity;
            }
            var root = mapping == null ? new GraphNode() : new ConfigurationVisitor<T>().GetNodes(mapping);
            var graphDiffer = new GraphDiffer<T>(root);
            return graphDiffer.Merge(dbContext, entity);
	    }

	    /// <summary>
	    /// Updates multiple entity graphs
	    /// </summary>
	    /// <param name="context">The database context to attach / detach objects from</param>
	    /// <param name="entities">the root entities</param>
	    /// <param name="mapping">the map for doing the graph update</param>
	    /// <typeparam name="T">the type of the root entities</typeparam>
	    /// <returns>the attached entity graphs</returns>
	    public static IEnumerable<T> UpdateGraphs<T>(this IDataContext context, IEnumerable<T> entities,
	        Expression<Func<IUpdateConfiguration<T>, object>> mapping = null) where T : class
	    {
	        return entities.Select(x => context.UpdateGraph(x, mapping));
	    }
        
	}
}
