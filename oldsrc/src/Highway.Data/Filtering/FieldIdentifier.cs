﻿namespace Highway.Data.Filtering
{
    public class FieldIdentifier<T>
    {
        public FieldIdentifier(string fieldName)
        {
            FieldName = fieldName;
        }

        internal string FieldName { get; set; }
    }
}