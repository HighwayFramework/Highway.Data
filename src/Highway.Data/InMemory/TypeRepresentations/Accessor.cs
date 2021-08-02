// <copyright file="Accessor.cs" company="Enterprise Products Partners L.P. (Enterprise)">
// © Copyright 2012 - 2019, Enterprise Products Partners L.P. (Enterprise), All Rights Reserved.
// Permission to use, copy, modify, or distribute this software source code, binaries or
// related documentation, is strictly prohibited, without written consent from Enterprise.
// For inquiries about the software, contact Enterprise: Enterprise Products Company Law
// Department, 1100 Louisiana, 10th Floor, Houston, Texas 77002, phone 713-381-6500.
// </copyright>

using System;

namespace Highway.Data.Contexts.TypeRepresentations
{
    internal class Accessor
    {
        public Accessor(Action removeAction, Func<object, object, object> getterFunc)
        {
            RemoveAction = removeAction;
            GetterFunc = getterFunc;
        }

        internal Func<object, object, object> GetterFunc { get; set; }

        internal Action RemoveAction { get; set; }
    }
}
